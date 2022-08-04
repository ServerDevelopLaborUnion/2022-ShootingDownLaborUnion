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

public enum EnemyType
{
    normal
}

[Serializable]
public class SpawnData
{
    public SpawnType Shape;
    public EnemyType EnemyType;
}

[Serializable]
public class PhaseData
{
    public List<SpawnData> SpawnDataList;
}
