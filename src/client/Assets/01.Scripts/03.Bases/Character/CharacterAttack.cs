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

    private void Start()
    {
        _base = transform.parent.GetComponent<CharacterBase>();
        _playerCol = transform.parent.GetComponent<Collider2D>();
    }
    public void DoAttack(bool clicked)
    {
        if (clicked && !( _base.Stat._isAttacking || _base.Stat._isDying))
        {
            OnAttacked?.Invoke();
            _base.Stat._isAttacking = true;
            Collider2D[] enemies = Physics2D.OverlapBoxAll(transform.position + new Vector3(_playerCol.bounds.size.x * transform.localScale.x, _playerCol.offset.y * 0.5f), new Vector3(_playerCol.bounds.size.x, _playerCol.bounds.size.y * 2), 0, LayerMask.GetMask("Enemy"));
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
        _base.Stat._isAttacking = false;
    }

    private void OnDrawGizmos()
    {

        Gizmos.DrawWireCube(transform.position + new Vector3(_playerCol.bounds.size.x * transform.localScale.x, _playerCol.offset.y * 0.5f), new Vector3(_playerCol.bounds.size.x,_playerCol.bounds.size.y * 2));
    }
}
