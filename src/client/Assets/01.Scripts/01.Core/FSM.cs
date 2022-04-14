using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum State
{
    Idle,
    Chase,
}

[Serializable]
public class FSM
{
    public State _currentState = State.Idle;
    private Dictionary<State, UnityEvent> _eventDictionary = new Dictionary<State, UnityEvent>();
    public FSM()
    {
        _currentState = State.Idle;
    }
    public FSM(State init = State.Idle)
    {
        _currentState = init;
    }

    public void UpdateInvoke()
    {
        if(_eventDictionary.ContainsKey(_currentState))
            _eventDictionary[_currentState]?.Invoke();
        else
        {
            _eventDictionary.Add(_currentState, null);
            return;
        }
    }

    public State ChangeState(State state)
    {
        _currentState = state;
        return _currentState;
    }

    public void AddActionOnState(State state, UnityAction action)
    {
        if (!_eventDictionary.ContainsKey(state))
        {
            _eventDictionary.Add(state, new UnityEvent());
        }
        if(action != null)
            _eventDictionary[state].AddListener(action);
    }

    public void RemoveActionOnState(State state, UnityAction action)
    {
        _eventDictionary[state].RemoveListener(action);
    }
}
