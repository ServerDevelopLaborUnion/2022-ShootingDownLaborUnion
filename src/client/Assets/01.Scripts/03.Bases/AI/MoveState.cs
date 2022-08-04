using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : AIState
{
    public override Action OnStateAction { get; set; }

    [field: SerializeField]

    public override List<AITransition> Transition { get; set; }

    [SerializeField]
    private Transform _target = null;

    public Transform Target { get => _target; set => _target = value; }

    [SerializeField]
    private CharacterMove _move = null;

    [SerializeField]
    private CharacterRenderer _renderer = null;

    private void Awake()
    {
        OnStateAction += () =>
        {
            if (_move != null)
            _move.MoveAgent(_target.position);
            _renderer.FlipCharacter(_target.position);
        };
    }
}
