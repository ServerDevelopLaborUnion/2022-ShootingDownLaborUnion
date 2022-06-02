using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using static CharacterBase;

[RequireComponent(typeof(CharacterBase))]
public class CharacterMove : MonoBehaviour
{
    private CharacterBase _base;
    private Rigidbody2D _rigid;

    protected Vector2 _movementDirection;

    [SerializeField] private float _knockbackPercent;


    public UnityEvent<bool> OnVelocityChange;

    private Vector3 _goal = Vector3.zero;

    private void Awake()
    {
        _base = GetComponent<CharacterBase>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_base.State.CurrentState.HasFlag(CharacterState.State.Died)) return;
        if(Vector3.Distance(_goal, transform.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, _goal, Time.deltaTime * _base.Stat.Speed /     Vector3.Distance(_goal, transform.position));
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
        _goal = goal;
    }

    public void StopImmediatelly()
    {
        _goal = transform.position;
    }

    public void Knockback(Collider2D col)
    {
        _rigid.position -= ((Vector2)(col.transform.position - transform.position).normalized) * _knockbackPercent * 0.01f;
    }
}
