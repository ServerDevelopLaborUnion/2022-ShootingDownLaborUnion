using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CharacterBase : MonoBehaviour
{
    [SerializeField] private CharacterStat _playerStat;


    public CharacterStat PlayerStat { get { return _playerStat; } }

    public bool _isAttacking = false;
    public bool _isDying = false;
    public bool _isDamaging = false;

    private void Update()
    {
        
    }
}
