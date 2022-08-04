using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackBase : CharacterAttack
{

    [SerializeField] private float _atkRange = 5;


    protected override void Attack()
    {
        base.Attack();

        List<Entity> closeEnemies = NetworkManager.Instance.playerList.FindAll((player) => Define.GetDistance(player.transform.position, transform.position) <= _atkRange);
        List<Entity> enemies = closeEnemies.FindAll((enemy) => FaceEnemy(enemy.transform.position));
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

    bool FaceEnemy(Vector3 _target)
    {
        Vector3 targetDir = _target - transform.position;
        Vector3 crossVec = Vector3.Cross(targetDir, transform.up);

        float dot = Vector3.Dot(crossVec, Vector3.up);

        if (dot >= 0)
        {

            if (transform.localScale.x < 0)
                return true;
        }
        else if (dot <= 0)
        {
            if (transform.localScale.x > 0)
                return true;
        }

        return false;
    }
}
