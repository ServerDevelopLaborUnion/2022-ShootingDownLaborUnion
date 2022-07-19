using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttack : CharacterAttack
{
    [SerializeField]
    private float _range = 0;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        List<Entity> enemies = NetworkManager.Instance.entityList.FindAll((entity) => entity.Data.Type == EntityType.Enemy).FindAll((entity) => GetDistance(transform.parent.position, entity.transform.position) <= _range);
        Entity closestEnemy = enemies.OrderBy((enemy) => GetDistance(Define.MainCam.ScreenToWorldPoint(Input.mousePosition), enemy.transform.position)).FirstOrDefault();

        closestEnemy?.GetComponent<CharacterDamage>().GetDamaged(_base.Data.entityStat.AD, _playerCol);
    }

    float GetDistance(Vector2 pos1, Vector2 pos2)
    {
        float x = Mathf.Pow(pos1.x - pos2.x, 2);
        float y = Mathf.Pow(pos1.y / 2 - pos2.y / 2, 2);
        return Mathf.Sqrt(x + y);
        
    }
}
