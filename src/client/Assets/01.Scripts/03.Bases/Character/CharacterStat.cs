using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;


[Serializable]
public class CharacterStat
{
    public enum Stat
    {
        HP,
        ATK,
        DEF,
        SPEED,
        ATKSPEED,
        ATKRANGE
    }
    [SerializeField] private int _hp = 0;
    [SerializeField] private int _atk = 0;
    [SerializeField] private int _def = 0;
    [SerializeField] private int _speed = 0;
    [SerializeField] private int _atkSpeed = 0;
    [SerializeField] private int _atkRange = 0;
    private CharacterJob.PlayerJob _playerJob = CharacterJob.PlayerJob.Base;

    [NonSerialized]
    public UnityEvent<CharacterJob.PlayerJob> OnJobChanged;

    public int HP { get { return _hp; } }
    public int AD { get { return _atk; } }
    public int Def { get { return _def; } }
    public int Speed { get { return _speed; } }
    public int AtkSpeed { get { return _atkSpeed; } }
    public int AtkRange { get { return _atkRange; } }

    public CharacterStat(int hp, int atk, int ap, int def, int speed, int atkSpeed, int atkrange, CharacterJob.PlayerJob job)
    {
        _hp = hp;
        _atk = atk;
        _def = def;
        _speed = speed;
        _atkSpeed = atkSpeed;
        _atkRange = atkrange;
        _playerJob = job;
    }

    public void ChangeJob(CharacterJob.PlayerJob job)
    {
        _playerJob = job;
        OnJobChanged?.Invoke(job);
    }

    public void ChangeStat(Stat stat, int value)
    {
        switch (stat)
        {
            case Stat.HP:
                _hp = value;
                break;
            case Stat.ATK:
                _atk = value;
                break;
            case Stat.DEF:
                _def = value;
                break;
            case Stat.SPEED:
                _speed = value;
                break;
            case Stat.ATKSPEED:
                _atkSpeed = value;
                break;
            case Stat.ATKRANGE:
                _atkRange = value;
                break;
            default:
                return;
        }
    }

    public static CharacterStat operator+(CharacterStat _base, CharacterStat value)
    {
        CharacterStat temp = _base;
        temp._hp += value._hp;
        temp._atk += value._atk;
        temp._def += value._def;
        temp._speed += value._speed;
        temp._atkSpeed += value._atkSpeed;
        temp._atkRange += value._atkRange;
        return temp;
    }
}
