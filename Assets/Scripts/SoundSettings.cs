using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private string _activeLabel;
    [SerializeField] private string _nonactiveLabel;
    [SerializeField] private SoundMixType _soundtype;

    private void Awake()
    {
        var newLabel = AudioManager.Instance.CheckSettings(_soundtype) ? _activeLabel : _nonactiveLabel;
        _text.text = newLabel;
    }

    public void OnClickButton()
    {
        AudioManager.Instance.SetSetting(_soundtype);
        var newLabel = AudioManager.Instance.CheckSettings(_soundtype) ? _activeLabel : _nonactiveLabel;
        _text.text = newLabel;
    }
}
