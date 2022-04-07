using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyDetect))]
public class EnemyMove : MonoBehaviour
{
    private Transform target = null;
    private EnemyDetect detect = null;

    private int moveSpeed = 1;

    private void Start()
    {
        detect = GetComponent<EnemyDetect>();
        target = detect.Target;
    }

    void Update()
    {
        if (target == null)
        {
            target = detect.Target;
        }
        else
        {
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        Vector3 dir = target.position - transform.position;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }
}
