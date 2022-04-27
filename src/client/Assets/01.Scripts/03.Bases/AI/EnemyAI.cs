using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FSM;

public class EnemyAI : MonoBehaviour
{
    private StateMachine fsm;
    public UnityEvent<Vector2> OnPlayerIn;
    public float _range = 6f;
    public float _stopRange = 1f;
    void Start()
    {
        fsm = new StateMachine();
        fsm.AddState("Idle",new State(onLogic: (state) => OnPlayerIn?.Invoke(Vector2.zero)));
        fsm.AddState("Chase", new State(onLogic: (state) => OnPlayerIn?.Invoke(GetNearestColliderInRange(_range).transform.position - transform.position)));
        fsm.AddState("Attack",new State());
        fsm.AddTransition("Idle", "Chase", (transition) => IsPlayerInrange(_range) && !IsPlayerInrange(_stopRange));
        fsm.AddTransition("Chase", "Idle", (transition) => IsPlayerInrange(_stopRange) && IsPlayerInrange(_range));
        fsm.Init();
    }

    private void Update()
    {
        fsm.OnLogic();
    }

    private bool IsPlayerInrange(float range)
    {
        bool result = false;
        if(GetNearestColliderInRange(range) != null)
            result = Vector2.Distance(GetNearestColliderInRange(range).transform.position, transform.position) > _stopRange;
        return result;
    }

    private List<Collider2D> GetCollidersInRange(float range)
    {
        List<Collider2D> temp = new List<Collider2D>();
        foreach(var a in Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Player")))
        {
            temp.Add(a);
        }
        return temp;
    }

    private Collider2D GetNearestColliderInRange(float range)
    {
        List<Collider2D> temp = GetCollidersInRange(range);
        if(temp.Count > 0)
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
