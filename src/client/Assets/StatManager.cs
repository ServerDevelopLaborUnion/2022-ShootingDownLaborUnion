using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    private GameObject _characterStatPanel = null;
    private Text _hpText = null;
    private Text _atkText = null;
    private Text _defText = null;
    private Text _speedText = null;
    private Text _atkSpeedText = null;
    private Text _atkRangeText = null;

    private void Awake()
    {
        _characterStatPanel = GameObject.Find("CharacterStatPanel");
        _hpText = GameObject.Find("hpText").GetComponent<Text>();
        _atkText = GameObject.Find("atkText").GetComponent<Text>();
        _defText = GameObject.Find("defText").GetComponent<Text>();
        _speedText = GameObject.Find("speedText").GetComponent<Text>();
        _atkSpeedText = GameObject.Find("atkSpeedText").GetComponent<Text>();
        _atkRangeText = GameObject.Find("atkRangeText").GetComponent<Text>();
        _characterStatPanel.SetActive(false);
    }

    public void UpdateText(PlayerBase _base)
    {
        _hpText.text = _base.Stat.HP.ToString();
        _atkText.text = _base.Stat.AD.ToString();
        _defText.text = _base.Stat.Def.ToString();
        _speedText.text = _base.Stat.Speed.ToString();
        _atkSpeedText.text = _base.Stat.AtkSpeed.ToString();
        _atkRangeText.text = _base.Stat.AtkRange.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _characterStatPanel.SetActive(!_characterStatPanel.activeInHierarchy);
        }
    }
}
