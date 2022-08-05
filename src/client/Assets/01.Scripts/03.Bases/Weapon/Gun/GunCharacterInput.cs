using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunCharacterInput : CharacterInput
{
    [SerializeField]
    private UnityEvent OnMouseKeyEvent;

    public UnityEvent GetOnMouseKeyEvent => OnMouseKeyEvent;

    protected override void Update() {
        base.Update();
        if(Input.GetMouseButton(0))
            OnMouseKeyEvent?.Invoke();  
    }
}
