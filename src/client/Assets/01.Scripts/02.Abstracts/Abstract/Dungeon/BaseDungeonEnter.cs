using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDungeonEnter : MonoBehaviour
{
    public abstract void EnterDirecting();

    protected float _amountDuration;

    public abstract float GetAmountDuration();
}
