using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private UnityEvent OnAttacked;
    private CharacterBase _base;

    private void Start()
    {
        _base = transform.parent.GetComponent<CharacterBase>();
    }
    public void DoAttack(bool clicked)
    {
        if (clicked && ! _base._isAttacking || _base._isDying)
        {
            OnAttacked?.Invoke();
            _base._isAttacking = true;
        }
    }

    public void EndAttack()
    {
        if (_base == null) return;
        _base._isAttacking = false;
    }
}
