using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterBase : MonoBehaviour
{
    [SerializeField] private UnityEvent<Vector2> OnMoveKeyInput;

    [SerializeField] private UnityEvent<bool> OnAttackKeyInput;

    [SerializeField] private UnityEvent<Vector2> OnPointerPositionChange;

    public static Transform _shadowTransform;

    public static bool _isAttacking = false;

    void Awake()
    {
        _shadowTransform = transform.Find("Shadow");
    }

    void Update()
    {
        OnMoveKeyInput?.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        OnAttackKeyInput?.Invoke(Input.GetMouseButton(0));

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
