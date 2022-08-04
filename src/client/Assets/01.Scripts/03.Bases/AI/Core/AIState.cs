using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState : MonoBehaviour
{
    public abstract Action OnStateAction { get; set; }
    public abstract List<AITransition> Transition { get; set; }
}
