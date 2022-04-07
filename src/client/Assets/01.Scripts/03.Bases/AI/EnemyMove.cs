using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyDetect))]
public class EnemyMove : CharacterMove
{
    private Transform target = null;
    private EnemyDetect detect = null;

    private int moveSpeed = 1;

    private void Start()
    {
        detect = GetComponent<EnemyDetect>();
        target = detect.Target;
    }

    private void Update()
    {
        if (target == null)
        {
            target = detect.Target;
        }
        else
        {
            MoveAgent(target.position);
        }
    }
}
