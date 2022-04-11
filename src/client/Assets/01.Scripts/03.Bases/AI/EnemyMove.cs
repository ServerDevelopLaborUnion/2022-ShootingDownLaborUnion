using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyDetect))]
public class EnemyMove : CharacterMove
{
    private Transform _target = null;
    private EnemyDetect _detect = null;

    private int _moveSpeed = 1;

    private void Start()
    {
        _detect = GetComponent<EnemyDetect>();
        _target = _detect.Target;
    }

    private void Update()
    {
        if (_target == null)
        {
            _target = _detect.Target;
        }
        else
        {
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        Vector3 dir = _target.position - transform.position;
        transform.position += dir * _moveSpeed * Time.deltaTime;
    }
}
