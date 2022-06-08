using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using static CharacterBase;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterBase))]
public class CharacterMove : MonoBehaviour
{
    private CharacterBase _base;
    private Rigidbody2D _rigid;

    protected Vector2 _movementDirection;

    [SerializeField] private float _knockbackPercent;


    public UnityEvent<bool> OnVelocityChange;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _base = GetComponent<CharacterBase>();
        _rigid = GetComponent<Rigidbody2D>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = _base.Stat.Speed;
    }

    private void Update()
    {
        if (_base.State.CurrentState.HasFlag(CharacterState.State.Died)) return;
        if(Vector3.Distance(_base.Data.TargetPosition, transform.position) > 0.1f)
        {
            //transform.position = Vector3.Lerp(transform.position, _base.Data.TargetPosition, Time.deltaTime * _base.Stat.Speed /     Vector3.Distance(_base.Data.TargetPosition, transform.position));
            OnVelocityChange?.Invoke(true);
        }
        else
        {
            StopImmediatelly();
            OnVelocityChange?.Invoke(false);
        }
    }

    public void MoveAgent(Vector3 goal)
    {
        _base.Data.TargetPosition = goal;
        
        agent.SetDestination(_base.Data.TargetPosition);
    }

    public void StopImmediatelly()
    {
        _base.Data.TargetPosition = transform.position;
        agent.SetDestination(_base.Data.TargetPosition);
    }

    public void Knockback(Collider2D col)
    {
        _rigid.position -= ((Vector2)(col.transform.position - transform.position).normalized) * _knockbackPercent * 0.01f;
    }
}
