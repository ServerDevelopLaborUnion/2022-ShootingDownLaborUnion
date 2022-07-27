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
    private UnityEvent OnRangeKeyInput = new UnityEvent();

    public UnityEvent GetOnAttackKeyInput => OnAttackKeyInput;

    private Entity playerEntity = null;
    private Vector2 tempPosition = Vector2.zero;

    private CharacterMove _move = null;
    private CharacterRenderer _renderer = null;

    private float _delay = 0;

    private bool _isShowingRange = false;


    private void Awake()
    {
        _move = GetComponent<CharacterMove>();
        playerEntity = GetComponent<Entity>();
        _renderer = transform.GetChild(0).GetComponent<CharacterRenderer>();
    }

    public void InitEvent()
    {
        _base = GetComponent<CharacterBase>();
        Transform visualTransform = transform.Find("Visual Sprite");
        OnMoveKeyInput.AddListener((goal) =>
        {
            _move.MoveAgent(goal);
            _renderer.FlipCharacter(goal);
        });
        OnAttackKeyInput.AddListener(() => visualTransform.GetComponent<PlayerAttack>().DoAttack());
        OnRangeKeyInput.AddListener(() => visualTransform.GetComponent<PlayerAttack>().ToggleRange());
    }
    protected virtual void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            OnMoveKeyInput?.Invoke(MousePos + Vector2.up);
            WebSocket.Client.ApplyEntityMove(playerEntity);
        }
        else if (Input.GetMouseButton(1))
        {
            _delay += Time.deltaTime;
            if (_delay > 0.2f)
            {
                OnMoveKeyInput?.Invoke(MousePos + Vector2.up);
                WebSocket.Client.ApplyEntityMove(playerEntity);
                _delay = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnRangeKeyInput?.Invoke();
            _isShowingRange = !_isShowingRange;
        }

        if (_isShowingRange)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnAttackKeyInput?.Invoke();
                OnRangeKeyInput?.Invoke();
                _isShowingRange = !_isShowingRange;
            }
        }
    }
}
