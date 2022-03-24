using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public int moveSpeed;
    public Transform target;

    void Update()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        Vector3 dir = target.position - transform.position;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }
}
