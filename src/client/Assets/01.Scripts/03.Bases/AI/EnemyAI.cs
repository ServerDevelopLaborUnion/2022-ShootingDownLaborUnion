using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FSM;

public class EnemyAI : MonoBehaviour
{
    private StateMachine fsm;
    public UnityEvent<Vector3> OnPlayerIn;
    public UnityEvent DoStop;
    public UnityEvent<bool> OnPlayerAtkRangeIn;
    public float _range = 6f;
    public float _stopRange = 1f;
    private float _delay = 0f;
    [SerializeField]
    private EnemyBase _base = null;
    void Start()
    {
        fsm = new StateMachine();
        fsm.AddState("Idle", new State(onLogic: (state) => {DoStop?.Invoke(); _delay = 0.6f; }));
        fsm.AddState("Chase", new State(onLogic: (state) =>
        { 
            OnPlayerIn?.Invoke(GetNearestColliderInRange(_range).transform.position);
            _delay += Time.deltaTime;
            if(_delay > 0.5f)
            {
                _delay = 0f;
                WebSocket.Client.ApplyEntityMove(_base);
            }
        }));
        fsm.AddState("Attack", new State(onLogic: (state) => OnPlayerAtkRangeIn?.Invoke(true)));
        fsm.AddTransition("Idle", "Chase", (transition) => IsPlayerInRange(_range) && !IsPlayerInRange(_stopRange));
        fsm.AddTransition("Chase", "Idle", (transition) => !IsPlayerInRange(_range));
        fsm.AddTransition("Chase", "Attack", (transition) => IsPlayerInRange(_stopRange) && IsPlayerInRange(_range));
        fsm.AddTransition("Idle", "Attack", (transition) => IsPlayerInRange(_stopRange) && IsPlayerInRange(_range));
        fsm.AddTransition("Attack", "Idle", (transition) => !IsPlayerInRange(_range) || !IsPlayerInRange(_stopRange));
        fsm.AddTransition("Attack", "Chase", (transition) => IsPlayerInRange(_range) || !IsPlayerInRange(_stopRange));
        fsm.Init();
    }

    private void Update()
    {
        fsm.OnLogic();
    }

    private bool IsPlayerInRange(float range)
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
