using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CharacterJob
{
    Base,
    Knight,
    Mage,
    Witch,
    Archer
}
public class CharacterBase : MonoBehaviour
{
    [SerializeField] private CharacterStat _playerStat = new CharacterStat();

    [SerializeField] private CharacterJob _currentJob;

    [SerializeField] private UnityEvent<Vector2> OnMoveKeyInput;

    [SerializeField] private UnityEvent<bool> OnAttackKeyInput;

    [SerializeField] private UnityEvent<Vector2> OnPointerPositionChange;

    [SerializeField] private UnityEvent OnCharacterDead;



    public CharacterStat PlayerStat { get { return _playerStat; } }

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
        OnAttackKeyInput?.Invoke(Input.GetMouseButtonDown(0));

        GetPointerInput();
        Die();
    }

    private void GetPointerInput()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        Vector2 mouseInWorldPos = Define.MainCam.ScreenToWorldPoint(mousePos);
        OnPointerPositionChange?.Invoke(mouseInWorldPos);
    }

    private void Die()
    {
        if(_playerStat.HP <= 0 && !_isDying)
        {
            OnCharacterDead?.Invoke();
        }
    }
}
