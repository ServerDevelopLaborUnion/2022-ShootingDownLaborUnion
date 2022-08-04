﻿using System;
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
    private CharacterBase _base = null;

    [SerializeField]
    private CharacterMove _move = null;

    [SerializeField]
    private CharacterRenderer _renderer = null;
    private float _delay = 0;

    private void Awake()
    {
        OnStateAction += () =>
        {
            if(_target != null)
            {
                _delay += Time.deltaTime;
                if (_delay > 0.2f)
                {
                    if (_move != null)
                        _move.MoveAgent(_target.position);
                    _renderer.FlipCharacter(_target.position);
                    if(_target.position.x - transform.position.x > 0 )
                        WebSocket.Client.ApplyEntityAction(_base, "DoFilpRight");
                    else
                        WebSocket.Client.ApplyEntityAction(_base, "DoFilpLeft");

                    WebSocket.Client.ApplyEntityMove(_base);
                    _delay = 0;
                }
            }
        };
    }
}
