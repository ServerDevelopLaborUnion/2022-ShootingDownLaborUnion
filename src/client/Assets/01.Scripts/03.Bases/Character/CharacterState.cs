using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterState
{
    [SerializeField]
    private State _state = State.Null;
    public State CurrentState { get { return _state; } set { _state = value; } }
    [System.Flags]
    public enum State
    {
        Null = 0,
        Attack = 1 << 1,
        Damaged = 1 << 2,
        Died = 1 << 3,
        All = int.MaxValue
    }
}
