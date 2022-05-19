using static Define;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterInput : MonoBehaviour
{
    private CharacterBase _base = null;
    private UnityEvent<Vector2> OnMoveKeyInput = new UnityEvent<Vector2>();

    private UnityEvent<bool> OnAttackKeyInput = new UnityEvent<bool>();

    public UnityEvent<bool> GetOnAttackKeyInput => OnAttackKeyInput;

    private UnityEvent<Vector2> OnPointerPositionChange = new UnityEvent<Vector2>();

    private Entity playerEntity = null;
    private Vector2 tempPosition = Vector2.zero;

    public void InitEvent()
    {
        _base = GetComponent<CharacterBase>();
        playerEntity = GetComponent<Entity>();
        Transform visualTransform = transform.Find("Visual Sprite");
        OnMoveKeyInput.AddListener((dir) => GetComponent<CharacterMove>().MoveAgent(dir));
        OnAttackKeyInput.AddListener((x) => visualTransform.GetComponent<CharacterAttack>().DoAttack(x));
        OnPointerPositionChange.AddListener((dir) => visualTransform.GetComponent<CharacterRenderer>().FlipCharacter(dir));
    }
    protected virtual void Update()
    {
        OnMoveKeyInput?.Invoke(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
        OnAttackKeyInput?.Invoke(Input.GetMouseButtonDown(0));
        OnPointerPositionChange?.Invoke(MousePos - (Vector2)transform.position);
    }

    private void FixedUpdate()
    {
        if(tempPosition != (Vector2)transform.position)
        {
            tempPosition = transform.position;
            WebSocket.Client.ApplyEntityMove(playerEntity);
            WebSocket.Client.ApplyEntityEvent(_base, "DoMove");
        }
    }
}
