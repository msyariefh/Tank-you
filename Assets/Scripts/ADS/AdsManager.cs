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
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        adShowing = true;
        Advertisement.Load(_adUnitId, this);
        ShowAd();
    }

    // Show the loaded content in the Ad Unit:
    public void ShowAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        
        Debug.Log("Showing Ad: " + _adUnitId);
        Advertisement.Show(_adUnitId, this);
        
    }

    // Implement Load Listener and Show Listener interface methods: 

    public IEnumerable WaitingForAds()
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
        Time.timeScale = 1;
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        adShowing = false;
        Time.timeScale = 1;
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        
    }
}