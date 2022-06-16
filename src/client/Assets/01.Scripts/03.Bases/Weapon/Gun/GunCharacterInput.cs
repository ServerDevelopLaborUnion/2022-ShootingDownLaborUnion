using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunCharacterInput : CharacterInput
{
    [SerializeField]
    private UnityEvent<bool> OnMouseKeyEvent;

    public UnityEvent<bool> GetOnMouseKeyEvent => OnMouseKeyEvent;

    protected override void LateUpdate() {
        base.LateUpdate();
        OnMouseKeyEvent?.Invoke(Input.GetMouseButton(0));
    }
}
