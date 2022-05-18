using static Define;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterInput : MonoBehaviour
{
    private UnityEvent<Vector2> OnMoveKeyInput = new UnityEvent<Vector2>();

    private UnityEvent<bool> OnAttackKeyInput = new UnityEvent<bool>();

    public UnityEvent<bool> GetOnAttackKeyInput => OnAttackKeyInput;

    private UnityEvent<Vector2> OnPointerPositionChange = new UnityEvent<Vector2>();

    public void InitEvent()
    {
        Transform visualTransform = transform.Find("Visual Sprite");
        OnMoveKeyInput.AddListener((dir) => GetComponent<CharacterMove>().MoveAgent(dir));
        Debug.Log(OnMoveKeyInput.GetPersistentEventCount());
        OnAttackKeyInput.AddListener((x) => visualTransform.GetComponent<CharacterAttack>().DoAttack(x));
        OnPointerPositionChange.AddListener((dir) => visualTransform.GetComponent<CharacterRenderer>().FlipCharacter(dir));
    }
    protected virtual void Update()
    {
        OnMoveKeyInput?.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        OnAttackKeyInput?.Invoke(Input.GetMouseButtonDown(0));
        OnPointerPositionChange?.Invoke(MousePos - (Vector2)transform.position);
    }


}
