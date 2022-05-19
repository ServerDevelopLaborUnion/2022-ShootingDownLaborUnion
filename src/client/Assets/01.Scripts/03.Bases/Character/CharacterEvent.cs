using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvent : MonoBehaviour
{
    public UnityEvent DoAttack = new UnityEvent();
    public UnityEvent DoFlipLeft = new UnityEvent();
    public UnityEvent DoFlipRight = new UnityEvent();

    private void Awake()
    {
        DoAttack.AddListener(() => GetComponent<CharacterAttack>().DoAttack(true));
        DoFlipLeft.AddListener(() => GetComponent<CharacterRenderer>().FlipCharacter(Vector2.left));
        DoFlipRight.AddListener(() => GetComponent<CharacterRenderer>().FlipCharacter(Vector2.right));
    }
    public void InvokeEvent(string eventName)
    {
        Type type = this.GetType();
        var action = type.GetProperty(eventName).GetValue(this, null);
        if (action != null)
            action.GetType().GetMethod(eventName).Invoke(action, null);
    }
}
