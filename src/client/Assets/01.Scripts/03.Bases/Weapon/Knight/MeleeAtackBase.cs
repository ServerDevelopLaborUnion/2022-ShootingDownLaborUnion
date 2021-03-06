using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAtackBase : CharacterAttack
{

    [SerializeField] private float _atkRange = 5;


    protected override void Attack()
    {
        base.Attack();
        Collider2D[] enemies = Physics2D.OverlapBoxAll(transform.position + Vector3.right * transform.localScale.x * _atkRange * 0.5f, new Vector2(_atkRange, _playerCol.bounds.size.y), 0, LayerMask.GetMask(_isPlayer ? "Enemy" : "Player"));
        if (enemies.Length <= 0) return;
        foreach (var enemy in enemies)
        {
            Debug.Log(enemy.name);
            enemy.GetComponent<CharacterDamage>().GetDamaged(_base.Stat.AD, _playerCol);
        }
    }

    private void OnDrawGizmos()
    {
        if(_playerCol == null)return;
        Gizmos.DrawWireCube(transform.position + Vector3.right * transform.localScale.x * _atkRange * 0.5f, new Vector2(_atkRange, _playerCol.bounds.size.y));
    }
}
