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

    private GameObject _rangeObject = null;
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
        if(_base.Data.Type == EntityType.Player)
        {
            _rangeObject = transform.parent.Find("Range").gameObject;
            _rangeObject.SetActive(false);
        }
    }
    public void ToggleRange()
    {
        _rangeObject.SetActive(_rangeObject.activeInHierarchy);
    }
    public void DoAttack()
    {
        if (!(_base.State.CurrentState.HasFlag(CharacterState.State.Attack) 
            || _base.State.CurrentState.HasFlag(CharacterState.State.Died) ))
        {
            OnAttacked?.Invoke();
            Attack();
        }
    }

    protected virtual void Attack()
    {
        _base.State.CurrentState |= CharacterState.State.Attack;
        WebSocket.Client.ApplyEntityEvent(_base, "DoAttack");
    }

    public void DoAttackInAnimation()
    {
        if ((!_base.State.CurrentState.HasFlag(CharacterState.State.Attack) || !_base.State.CurrentState.HasFlag(CharacterState.State.Died)))
        {
            Attack();
        }
    }
    public void EndAttack()
    {
        if (_base == null) return;
        _base.State.CurrentState &= ~CharacterState.State.Attack;
    }

}
