using static Define;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterInput : MonoBehaviour
{
    private CharacterBase _base = null;
    private UnityEvent<Vector2> OnMoveKeyInput = new UnityEvent<Vector2>();

    private UnityEvent OnAttackKeyInput = new UnityEvent();

    public UnityEvent GetOnAttackKeyInput => OnAttackKeyInput;

    private Entity playerEntity = null;
    private Vector2 tempPosition = Vector2.zero;

    private CharacterMove _move = null;
    private CharacterRenderer _renderer = null;

    private float _delay = 0;


    private void Awake()
    {
        _move = GetComponent<CharacterMove>();
        _renderer = transform.GetChild(0).GetComponent<CharacterRenderer>();
    }

    public void InitEvent()
    {
        _base = GetComponent<CharacterBase>();
        playerEntity = GetComponent<Entity>();
        Transform visualTransform = transform.Find("Visual Sprite");
        OnMoveKeyInput.AddListener((goal) =>
        {
            _move.MoveAgent(goal);
            _renderer.FlipCharacter(goal);
        });
        OnAttackKeyInput.AddListener(() => visualTransform.GetComponent<PlayerAttack>().DoAttack());
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnAttackKeyInput?.Invoke();
        }
    }
}
