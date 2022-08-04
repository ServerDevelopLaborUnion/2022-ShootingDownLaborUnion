﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterDamage : MonoBehaviour
{
    [SerializeField] private UnityEvent OnCharacterDamaged;
    [SerializeField] private UnityEvent<Collider2D> OnDamagedFeedBack;
    [SerializeField]
    private GameObject _damageParticle = null;
    private CharacterBase _base;

    private void Start()
    {
        _base = GetComponent<CharacterBase>();
    }

    public void GetDamaged(int value, Collider2D col)
    {
        if (_base.State.CurrentState.HasFlag(CharacterState.State.Damaged) || _base.State.CurrentState.HasFlag(CharacterState.State.Died)) return;
        _base.State.CurrentState |= CharacterState.State.Damaged;
        _base.Stat.ChangeStat(CharacterStat.Stat.HP, _base.Stat.HP - Mathf.RoundToInt(Mathf.Pow(value, 2) / (value + _base.Stat.Def)));
        OnCharacterDamaged?.Invoke();
        Instantiate(_damageParticle,transform.position, Quaternion.identity);
        if(col != null)
            OnDamagedFeedBack?.Invoke(col);
    }

    internal void GetDamaged(object aD, object playerCol)
    {
        throw new NotImplementedException();
    }
}
