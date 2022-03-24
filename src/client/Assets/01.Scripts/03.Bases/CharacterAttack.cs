using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static CharacterBase; 

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private UnityEvent OnAttacked;

    public void DoAttack(bool clicked)
    {
        if (clicked && !_isAttacking)
        {
            OnAttacked?.Invoke();
            _isAttacking = true;
        }
    }

    public void EndAttack()
    {
        _isAttacking = false;
    }
}
