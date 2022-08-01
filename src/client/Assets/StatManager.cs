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

    private Image _weaponImage = null;
    private Text _weaponNameText = null;
    private Text _weaponExplainText = null;

    private void Awake()
    {
        _characterStatPanel = GameObject.Find("CharacterStatPanel");
        _hpText = GameObject.Find("hpText").GetComponent<Text>();
        _atkText = GameObject.Find("atkText").GetComponent<Text>();
        _defText = GameObject.Find("defText").GetComponent<Text>();
        _speedText = GameObject.Find("speedText").GetComponent<Text>();
        _atkSpeedText = GameObject.Find("atkSpeedText").GetComponent<Text>();
        _atkRangeText = GameObject.Find("atkRangeText").GetComponent<Text>();

        _weaponNameText = GameObject.Find("WeaponNameText").GetComponent<Text>();
        _weaponExplainText = GameObject.Find("WeaponExplainText").GetComponent<Text>();
        _weaponImage = GameObject.Find("WeaponImage").GetComponent<Image>();
        _characterStatPanel.SetActive(false);
    }

    public void UpdateText(PlayerBase _base, Weapon _weapon = null)
    {

        if(_weapon != null)
        {
            _hpText.text = $"{_base.Stat.HP} +({_weapon.Stat.HP})";
            _atkText.text = $"{_base.Stat.AD} +({_weapon.Stat.AD})";
            _defText.text = $"{_base.Stat.Def} +({_weapon.Stat.Def})";
            _speedText.text = $"{_base.Stat.Speed} +({_weapon.Stat.Speed})";
            _atkSpeedText.text = $"{_base.Stat.AtkSpeed} +({_weapon.Stat.AtkSpeed})";
            _atkRangeText.text = $"{_base.Stat.AtkRange} +({_weapon.Stat.AtkRange})";
            _weaponNameText.text = $"『{_weapon.WeaponName}』";
            _weaponExplainText.text = $"『{_weapon.WeaponExplain}』";
            _weaponImage.sprite = _weapon.WeaponImage;
        }
        else
        {
            _hpText.text = $"{_base.Stat.HP} +()";
            _atkText.text = $"{_base.Stat.AD} +()";
            _defText.text = $"{_base.Stat.Def} +()";
            _speedText.text = $"{_base.Stat.Speed} +()";
            _atkSpeedText.text = $"{_base.Stat.AtkSpeed} +()";
            _atkRangeText.text = $"{_base.Stat.AtkRange} +()";
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _characterStatPanel.SetActive(!_characterStatPanel.activeInHierarchy);
        }
    }
}
