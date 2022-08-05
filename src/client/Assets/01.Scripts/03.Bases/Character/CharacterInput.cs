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
    private UnityEvent OnSkillKeyInput = new UnityEvent();
    private UnityEvent OnRangeKeyInput = new UnityEvent();

    public UnityEvent GetOnAttackKeyInput => OnAttackKeyInput;

    private Entity playerEntity = null;
    private Vector2 tempPosition = Vector2.zero;

    private CharacterMove _move = null;
    private PlayerAttack _attack = null;
    private SkillBase _skill = null;
    private CharacterRenderer _renderer = null;

    private float _clickDelay = 0.2f;
    private float _delay = 0;

    private bool _isShowingRange = false;

    Transform visualTransform = null;

    private void Awake()
    {
        _move = GetComponent<CharacterMove>();
        visualTransform = transform.Find("Visual Sprite");
        _attack = visualTransform.GetComponent<PlayerAttack>();
        _skill = visualTransform.GetComponent<SkillBase>();
        playerEntity = GetComponent<Entity>();
        _renderer = transform.GetChild(0).GetComponent<CharacterRenderer>();
    }

    public void InitEvent()
    {
        _base = GetComponent<CharacterBase>();
        OnMoveKeyInput.AddListener((goal) =>
        {
            _move.MoveAgent(goal);
            _renderer.FlipCharacter(goal);
        });
        OnAttackKeyInput.AddListener(() => _attack.DoAttack());
        OnRangeKeyInput.AddListener(() => _attack.ToggleRange());
        OnSkillKeyInput.AddListener(() => _skill.UseSkill());
    }
    protected virtual void Update()
    {
        if(_clickDelay >= 0)
            _clickDelay -= Time.deltaTime;
        if (Input.GetMouseButtonDown(1))
        {
            if(_clickDelay <= 0)
            {
                UIManager.Instance.SummonMoveImpact();
                OnMoveKeyInput?.Invoke(MousePos + Vector2.up);
                WebSocket.Client.ApplyEntityMove(playerEntity);
                _clickDelay = 0.2f;
            }
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
                if (_clickDelay <= 0)
                {
                    UIManager.Instance.SummonMoveImpact();
                    OnAttackKeyInput?.Invoke();
                    OnRangeKeyInput?.Invoke();
                    _isShowingRange = !_isShowingRange;
                    _clickDelay = 0.2f;

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnSkillKeyInput?.Invoke();
        }
    }
}
