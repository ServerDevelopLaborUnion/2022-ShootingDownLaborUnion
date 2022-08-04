using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSetter : MonoBehaviour
{
    [SerializeField]
    private float _range = 5f;

    MoveState moveState;

    private void Awake()
    {
        moveState = GetComponent<MoveState>();
    }

    private void Update()
    {
        if(GetNearestColliderInRange(_range) != null)
            moveState.Target = GetNearestColliderInRange(_range).transform;
        else
            moveState.Target = null;
    }

    private List<Collider2D> GetCollidersInRange(float range)
    {
        List<Collider2D> temp = new List<Collider2D>();
        foreach (var a in Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Player")))
        {
            temp.Add(a);
        }
        return temp;
    }

    private Collider2D GetNearestColliderInRange(float range)
    {
        List<Collider2D> temp = GetCollidersInRange(range);
        if (temp.Count > 0)
        {

            temp.Sort((a, b) =>
            {
                if (Vector2.Distance(a.transform.position, transform.position) > Vector2.Distance(b.transform.position, transform.position))

                {

                    return 1;

                }

                else if (Vector2.Distance(a.transform.position, transform.position) < Vector2.Distance(b.transform.position, transform.position))

                {

                    return -1;

                }

                else

                {

                    return 0;

                }
            });
            return temp[0];
        }
        else
            return null;
    }
}
