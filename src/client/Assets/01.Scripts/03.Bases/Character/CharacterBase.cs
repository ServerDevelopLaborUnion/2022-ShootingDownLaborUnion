using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CharacterBase : MonoBehaviour
{
    [SerializeField] private CharacterStat _playerStat;

    [SerializeField] private UnityEvent OnCharacterDead;

    [SerializeField] private UnityEvent OnCharacterDamaged;

    public CharacterStat PlayerStat { get { return _playerStat; } }

    public bool _isAttacking = false;
    public bool _isDying = false;
    public bool _isDamaging = false;

    private void Update()
    {
        
    }

    private void Die()
    {
        OnCharacterDead?.Invoke();
    }

    public void GetDamaged(int value)
    {
        if (_isDamaging) return;
        _isDamaging = true;
        OnCharacterDamaged?.Invoke();
        _playerStat.ChangeStat(CharacterStat.Stat.HP, _playerStat.HP - value);
        if (_playerStat.HP <= 0 && !_isDying)
        {
            Die();
        }
    }
}
