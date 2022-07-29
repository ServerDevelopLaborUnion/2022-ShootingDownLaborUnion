﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : CharacterBase
{
    [SerializeField]
    private Weapon weapon;
    public Weapon Weapon { get => weapon; }
    public bool RoomHost = false;
    public StatManager statManager = null;

    public Action OnWeaponChanged = null;
    

    private void Start()
    {
        statManager = GetComponent<StatManager>();
        OnWeaponChanged += GetWeaponStat;
        //for Debug
        OnWeaponChanged?.Invoke();
    }

    public void GetWeaponStat()
    {
        if (weapon != null)
            _playerStat += weapon.Stat;
    }
}
