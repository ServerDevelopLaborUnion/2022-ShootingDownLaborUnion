using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private UnityEvent OnAttacked;
    protected CharacterBase _base;

    protected Collider2D _playerCol;

    protected bool _isPlayer = false;

    protected Transform _transform = null;

    protected virtual void Start()
    {
        Transform par = transform;
        while(true){
            if(par.parent == null)break;
            par = par.parent;
        }
        _transform = transform;
        _base = par.GetComponent<CharacterBase>();
        _playerCol = par.GetComponent<Collider2D>();
        _isPlayer = par.CompareTag("Player");
    }
    public void DoAttack(bool clicked)
    {
        if (clicked && !(_base.State.CurrentState.HasFlag(CharacterState.State.Attack) 
            || _base.State.CurrentState.HasFlag(CharacterState.State.Died) ))
        {
            OnAttacked?.Invoke();
            Attack();
        }
    }

    protected virtual void Attack()
    {
        _base.State.CurrentState |= CharacterState.State.Attack;
    }

    public void DoAttackInAnimation()
    {
        if ((!_base.State.CurrentState.HasFlag(CharacterState.State.Attack) || !_base.State.CurrentState.HasFlag(CharacterState.State.Died)))
        {
            _base.State.CurrentState |= CharacterState.State.Attack;
            Attack();
        }
    }
    public void EndAttack()
    {
        if (_base == null) return;
        _base.State.CurrentState &= ~CharacterState.State.Attack;
    }

}
