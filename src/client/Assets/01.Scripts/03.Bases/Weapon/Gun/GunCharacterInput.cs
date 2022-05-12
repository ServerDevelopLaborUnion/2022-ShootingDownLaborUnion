using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunCharacterInput : CharacterInput
{
    [SerializeField]
    private UnityEvent<bool> OnMouseKeyEvent;

    public UnityEvent<bool> GetOnMouseKeyEvent => OnMouseKeyEvent;

    protected override void Update() {
        base.Update();
        OnMouseKeyEvent?.Invoke(Input.GetMouseButton(0));
    }
}
