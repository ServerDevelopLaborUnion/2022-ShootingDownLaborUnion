using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharacterStat
{
    [SerializeField] private int _hp = 0;
    [SerializeField] private int _ad = 0;
    [SerializeField] private int _ap = 0;
    [SerializeField] private int _def = 0;
    [SerializeField] private int _speed = 0;

    public int HP { get { return _hp; } }
    public int AD { get { return _ad; } }
    public int AP { get { return _ap; } }
    public int Def { get { return _def; } }
    public int Speed { get { return _speed; } }
}
