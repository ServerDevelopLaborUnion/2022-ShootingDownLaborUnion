using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : AIState
{
    public override Action OnStateAction { get; set; }

    [field: SerializeField]
    public override List<AITransition> Transition { get; set; }

    [SerializeField]
    private CharacterMove _move = null;
    [SerializeField]
    private CharacterBase _base = null;
    [SerializeField]
    private CharacterAnimation _anime = null;

    private void Awake()
    {
        OnStateAction += () =>
        {
            if(_base.State.CurrentState.HasFlag(CharacterState.State.Attack))
                return;

            _move.StopImmediatelly();
            _anime.PlayAttackAnime();
            WebSocket.Client.ApplyEntityAction(_base, "DoAttack");

            _base.State.CurrentState |= CharacterState.State.Attack;
        };
    }
}
