using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterDamage : MonoBehaviour
{
    [SerializeField] private UnityEvent OnCharacterDamaged;
    [SerializeField] private UnityEvent<Collider2D> OnDamagedFeedBack;
    private CharacterBase _base;

    private void Start()
    {
        _base = GetComponent<CharacterBase>();
    }

    public void GetDamaged(int value, Collider2D col)
    {
        if (_base.PlayerStat._isDamaging) return;
        _base.PlayerStat._isDamaging = true;
        _base.PlayerStat.ChangeStat(CharacterStat.Stat.HP, _base.PlayerStat.HP - value);
        OnCharacterDamaged?.Invoke();
        OnDamagedFeedBack?.Invoke(col);
    }
}
