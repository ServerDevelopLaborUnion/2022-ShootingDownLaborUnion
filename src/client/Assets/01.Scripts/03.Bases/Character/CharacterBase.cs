using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CharacterBase : MonoBehaviour
{
    [SerializeField] private CharacterStat _playerStat;


    public CharacterStat Stat { get { return _playerStat; } }


    private void Update()
    {
        
    }
}
