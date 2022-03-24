using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterBase : MonoBehaviour
{
    [SerializeField] private UnityEvent<Vector2> OnMoveKeyInput;

    [SerializeField] private UnityEvent<bool> OnAttackKeyInput;

    [SerializeField] private UnityEvent<Vector2> OnPointerPositionChange;

    [SerializeField] private UnityEvent OnCharacterDead;

    public static Transform _shadowTransform;

    public static bool _isAttacking = false;
    public static bool _isDying = false;

    void Awake()
    {
        _shadowTransform = transform.Find("Shadow");
    }

    void Update()
    {
        OnMoveKeyInput?.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        OnAttackKeyInput?.Invoke(Input.GetMouseButton(0));

        GetPointerInput();
        TestDie();
    }

    private void GetPointerInput()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        Vector2 mouseInWorldPos = Define.MainCam.ScreenToWorldPoint(mousePos);
        OnPointerPositionChange?.Invoke(mouseInWorldPos);
    }

    private void TestDie()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnCharacterDead?.Invoke();
        }
    }
}
