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
        if (_base.Stat._isDamaging || _base.Stat._isDying) return;
        _base.Stat._isDamaging = true;
        _base.Stat.ChangeStat(CharacterStat.Stat.HP, _base.Stat.HP - value);
        OnCharacterDamaged?.Invoke();
        OnDamagedFeedBack?.Invoke(col);
    }
}
