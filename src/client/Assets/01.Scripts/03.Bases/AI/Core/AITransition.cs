using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AITransition : MonoBehaviour
{
    public abstract AIState StartState { get; set; }
    public abstract List<AICondition> PositiveConditions { get; set;}
    public abstract List<AICondition> NegativeConditions { get; set;}
    public abstract bool IsOr { get; set;}
    public abstract AIState NextState { get; set;}
}
