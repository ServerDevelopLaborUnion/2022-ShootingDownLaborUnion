using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private UnityEvent OnAttacked;
    private CharacterBase _base;

    [SerializeField]
    [HideInInspector]
    private Collider2D _playerCol;

    [SerializeField] private float _atkRange;
    private bool _isPlayer = false;

    private void Start()
    {
        _base = transform.parent.GetComponent<CharacterBase>();
        _playerCol = transform.parent.GetComponent<Collider2D>();
        _isPlayer = transform.parent.CompareTag("Player");
    }
    public void DoAttack(bool clicked)
    {
        if (clicked && (!_base.State.CurrentState.HasFlag(CharacterState.State.Attack) || !_base.State.CurrentState.HasFlag(CharacterState.State.Died)))
        {
            OnAttacked?.Invoke();
            _base.State.CurrentState |= CharacterState.State.Attack;
            Collider2D[] enemies = Physics2D.OverlapBoxAll(transform.position + new Vector3(_playerCol.bounds.size.x * transform.localScale.x, _playerCol.offset.y * 0.5f), new Vector3(_playerCol.bounds.size.x, _playerCol.bounds.size.y * 2), 0, LayerMask.GetMask(_isPlayer ? "Enemy" : "Player"));
            if (enemies.Length <= 0) return;
            foreach (var enemy in enemies)
            {
                Debug.Log(enemy.name);
                enemy.GetComponent<CharacterDamage>().GetDamaged(_base.Stat.AD, _playerCol);
            }
        }
    }

    public void EndAttack()
    {
        if (_base == null) return;
        _base.State.CurrentState &= ~CharacterState.State.Attack;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + Vector3.right * transform.localScale.x * _atkRange * 0.5f, new Vector2(_atkRange, _playerCol.bounds.size.y));
    }
}
