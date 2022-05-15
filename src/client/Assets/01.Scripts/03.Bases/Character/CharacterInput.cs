using static Define;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterInput : MonoBehaviour
{
    [SerializeField] private UnityEvent<Vector2> OnMoveKeyInput;

    [SerializeField] private UnityEvent<bool> OnAttackKeyInput;

    public UnityEvent<bool> GetOnAttackKeyInput => OnAttackKeyInput;

    [SerializeField] private UnityEvent<Vector2> OnPointerPositionChange;
    protected virtual void Update()
    {
        OnMoveKeyInput?.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        OnAttackKeyInput?.Invoke(Input.GetMouseButtonDown(0));
        OnPointerPositionChange?.Invoke(MousePos - (Vector2)transform.position);
    }


}
