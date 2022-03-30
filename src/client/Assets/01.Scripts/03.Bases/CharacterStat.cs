using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[Serializable]
public class CharacterStat
{
    [SerializeField] private int _hp = 0;
    [SerializeField] private int _ad = 0;
    [SerializeField] private int _ap = 0;
    [SerializeField] private int _def = 0;
    [SerializeField] private int _speed = 0;
    [SerializeField] private CharacterJob.PlayerJob _playerJob = CharacterJob.PlayerJob.Base;
    public UnityEvent<CharacterJob.PlayerJob> OnJobChanged;

    public int HP { get { return _hp; } }
    public int AD { get { return _ad; } }
    public int AP { get { return _ap; } }
    public int Def { get { return _def; } }
    public int Speed { get { return _speed; } }

    public CharacterStat(int hp, int ad, int ap, int def, int speed, CharacterJob.PlayerJob job)
    {
        _hp = hp;
        _ad = ad;
        _ap = ap;
        _def = def;
        _speed = speed;
        _playerJob = job;
    }

    public void ChangeJob(CharacterJob.PlayerJob job)
    {
        _playerJob = job;
        OnJobChanged?.Invoke(job);
    }
}
