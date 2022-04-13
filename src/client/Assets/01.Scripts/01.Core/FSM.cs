using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum State
{
    Idle,
}

public class FSM : MonoBehaviour
{
    private State _currentState = State.Idle;
    private Dictionary<State, UnityEvent> _stateDictionary;
    private Dictionary<State, bool> _conditionDictionary;

    public FSM(State init = State.Idle)
    {
        _currentState = init;
    }

    private void Update()
    {
        if (_conditionDictionary[_currentState])
        {
            _stateDictionary[_currentState]?.Invoke();

        }
    }


}
