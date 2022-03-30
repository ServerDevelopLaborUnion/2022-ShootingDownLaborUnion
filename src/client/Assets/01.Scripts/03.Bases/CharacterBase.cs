using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CharacterBase : MonoBehaviour
{
    [SerializeField] private CharacterStat _playerStat;

    [SerializeField] private UnityEvent OnCharacterDead;

    public CharacterStat PlayerStat { get { return _playerStat; } }

    public bool _isAttacking = false;
    public bool _isDying = false;

    private void Update()
    {
        Die();
    }

    private void Die()
    {
        if(_playerStat.HP <= 0 && !_isDying)
        {
            OnCharacterDead?.Invoke();
        }
    }
}
