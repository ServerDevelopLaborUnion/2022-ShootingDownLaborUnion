using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityData
{
    public string Owner;
    public GameObject prefab = null;
    public CharacterStat entityStat = null;
}
