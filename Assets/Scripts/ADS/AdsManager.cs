using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsShowListener, IUnityAdsLoadListener
//IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOsAdUnitId = "Interstitial_iOS";
    string _adUnitId;
    public static AdsManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // Get the Ad Unit ID for the current platform:
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;
        DontDestroyOnLoad(gameObject);
    }

    // Load content to the Ad Unit:
    public void LoadAd(bool isFromGameplay = false)
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        if (!IsTimeToLoadAds(isFromGameplay)) return;
        adShowing = true;
        Advertisement.Load(_adUnitId, this);
        ShowAd();
    }

    // Show the loaded content in the Ad Unit:
    public void ShowAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        
        Advertisement.Show(_adUnitId, this);
        
    }

    // Implement Load Listener and Show Listener interface methods: 

    public IEnumerator WaitingForAds()
    {
        yield return new WaitUntil(() => !adShowing);
    }

    private bool adShowing = false;
    public void OnUnityAdsShowStart(string adUnitId) 
    {
        adShowing = true;
        Time.timeScale = 0;
    }
    //public void OnUnityAdsShowClick(string adUnitId) { Time.timeScale = 1; adShowing = false; }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) 
    {
        switch(showCompletionState)
        {
            case UnityAdsShowCompletionState.SKIPPED:
                adShowing = false;
                Time.timeScale = 1;
                break;
            case UnityAdsShowCompletionState.COMPLETED:
                adShowing = false;
                Time.timeScale = 1;
                break;

        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        adShowing = false;
        //Time.timeScale = 1;
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        adShowing = false;
        //Time.timeScale = 1;
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        adShowing = false;
    }
    float lastTimeLoadAds = 0f;
    private int gameplaycount = 0;
    private bool IsTimeToLoadAds(bool isFromGameplay = false)
    {
        var cTime = Time.unscaledTime;
        var deltaTime = cTime - lastTimeLoadAds;
        Debug.Log($"[HiDE] {cTime} | {deltaTime} | {lastTimeLoadAds} | {gameplaycount} ");
        if (isFromGameplay) gameplaycount++;
        // Load ads if played more than 4mins
        if (deltaTime > 240)
        {
            gameplaycount = 0;
            lastTimeLoadAds = cTime;
            return true;
        }
        // 2 tries and not within 60 sec
        if (gameplaycount >= 2 && deltaTime > 45)
        {
            gameplaycount = 0;
            lastTimeLoadAds = cTime;
            return true;
        }

        return false;
    }
}