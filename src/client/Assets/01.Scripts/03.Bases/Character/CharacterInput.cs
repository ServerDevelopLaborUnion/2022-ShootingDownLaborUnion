using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterInput : MonoBehaviour
{

    [SerializeField] private UnityEvent<Vector2> OnMoveKeyInput;

    [SerializeField] private UnityEvent<bool> OnAttackKeyInput;

    [SerializeField] private UnityEvent<Vector2> OnPointerPositionChange;

    private void Update()
    {
        OnMoveKeyInput?.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        OnAttackKeyInput?.Invoke(Input.GetMouseButtonDown(0));
        GetPointerInput();
    }

    private void GetPointerInput()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        Vector2 mouseInWorldPos = Define.MainCam.ScreenToWorldPoint(mousePos);
        OnPointerPositionChange?.Invoke(mouseInWorldPos);
    }

}
