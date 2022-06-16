using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnType
{
    circle,
    width,
    length
}
[Serializable]
public class PhaseData
{
    public int maxEnemy;
    public SpawnType spawnType;
    public PhaseData(int maxEnemy, SpawnType spawnType)
    {
        this.maxEnemy = maxEnemy;
        this.spawnType = spawnType;
    }
}
