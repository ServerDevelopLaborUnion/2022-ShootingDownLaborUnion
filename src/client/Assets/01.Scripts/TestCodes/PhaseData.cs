using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnType
{
    CIRCLE,
    WIDTH,
    LENGTH
}

public enum EnemyType
{
    NORMAL,
    FALSE_GOD,
    LENGTH
}

[Serializable]
public class SpawnData
{
    public SpawnType Shape;
    public EnemyType EnemyType;

    public int ShapeSpawnGoal;
    public int ShapeSpawnCount;
}

[Serializable]
public class PhaseData
{
    public List<SpawnData> SpawnDataList;
}
