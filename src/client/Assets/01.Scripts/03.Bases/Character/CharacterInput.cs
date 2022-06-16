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


    private float _delay = 0;
    public void InitEvent()
    {
        _base = GetComponent<CharacterBase>();
        playerEntity = GetComponent<Entity>();
        Transform visualTransform = transform.Find("Visual Sprite");
        OnMoveKeyInput.AddListener((goal) => GetComponent<CharacterMove>().MoveAgent(goal));
        OnAttackKeyInput.AddListener((x) => visualTransform.GetComponent<CharacterAttack>().DoAttack(x));
        OnPointerPositionChange.AddListener((dir) => visualTransform.GetComponent<CharacterRenderer>().FlipCharacter(dir));
    }
    protected virtual void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            OnMoveKeyInput?.Invoke(MousePos);
            WebSocket.Client.ApplyEntityMove(playerEntity);
        }
        else if (Input.GetMouseButton(1))
        {
            _delay += Time.deltaTime;
            if (_delay > 0.2f)
            {
                OnMoveKeyInput?.Invoke(MousePos);
                WebSocket.Client.ApplyEntityMove(playerEntity);
                _delay = 0;
            }
        }
        OnAttackKeyInput?.Invoke(Input.GetMouseButtonDown(0));
        OnPointerPositionChange?.Invoke((Vector2)transform.position - MousePos);
    }
}
