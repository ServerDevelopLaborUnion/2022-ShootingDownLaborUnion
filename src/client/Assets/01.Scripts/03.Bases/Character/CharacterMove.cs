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

    protected float _currentVelocity = 3;

    protected Vector2 _movementDirection;

    [SerializeField] private float _acceleration;
    [SerializeField] private float _deAcceleration;

    public UnityEvent<float> OnVelocityChange;

    private void Awake()
    {
        _base = GetComponent<CharacterBase>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    public void MoveAgent(Vector2 movementInput)
    {
        if (movementInput.sqrMagnitude > 0)
        {
            if (Vector2.Dot(movementInput, _movementDirection) < 0)
            {
                _currentVelocity = 0;
            }

            _movementDirection = movementInput.normalized;

        }

        _currentVelocity = CalculateSpeed(movementInput);
    }

    private float CalculateSpeed(Vector2 movementInput)
    {
        if (movementInput.sqrMagnitude > 0)
        {
            _currentVelocity += _acceleration * Time.deltaTime;
        }
        else
        {
            _currentVelocity -= _deAcceleration * Time.deltaTime;
        }
        return Mathf.Clamp(_currentVelocity, 0, _base.PlayerStat.Speed);
    }

    protected virtual void FixedUpdate()
    {
        OnVelocityChange?.Invoke(_currentVelocity);
        if (!(_base._isAttacking || _base._isDying))
        {
            _rigid.velocity = _movementDirection * _currentVelocity;
        }
        else
        {
            StopImmediatelly();
        }
    }

    public void StopImmediatelly()
    {
        _currentVelocity = 0;
        _rigid.velocity = Vector2.zero;
    }
}
