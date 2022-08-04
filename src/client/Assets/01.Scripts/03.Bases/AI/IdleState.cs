using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : AIState
{
    public override Action OnStateAction { get; set; }

    [field:SerializeField]
    public override List<AITransition> Transition { get; set; }

   
}
