using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CharacterBase : MonoBehaviour
{
    [SerializeField] private CharacterStat _playerStat;
    [SerializeField] private CharacterState _playerState;


    public CharacterStat Stat => _playerStat;
    public CharacterState State { get { return _playerState; } set { _playerState = value; } }


}
