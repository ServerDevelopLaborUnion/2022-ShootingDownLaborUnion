using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private FSM _thisFSM = new FSM();
    [SerializeField] private UnityEvent<Vector2> OnMoveSignal;
    [SerializeField] private UnityEvent OnStopSignal;
    // Start is called before the first frame update
    void Awake()
    {
        _thisFSM.AddActionOnState(State.Idle, StopAgent);
        _thisFSM.AddActionOnState(State.Chase, MoveAgent);
    }
    private void MoveAgent()
    {
        OnMoveSignal?.Invoke(
            (Physics2D.OverlapCircle(transform.position, 5,
            LayerMask.GetMask("Player")).transform.position - transform.position)
            .normalized);
    }
    private void StopAgent()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, 5, LayerMask.GetMask("Player")) != null)
        {
            _thisFSM.ChangeState(State.Chase);
        }
        else
        {
            _thisFSM.ChangeState(State.Idle);

        }
        _thisFSM.UpdateInvoke();
    }
}
