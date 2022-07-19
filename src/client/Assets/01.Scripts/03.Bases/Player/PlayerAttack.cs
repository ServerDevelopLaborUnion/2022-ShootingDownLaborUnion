using System.Collections;
using System.Collections.Generic;
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
        List<Entity> enemies = NetworkManager.Instance.entityList.FindAll((entity) => entity.Data.Type == EntityType.Enemy);
        
        foreach (Entity enemy in enemies)
        {

        }
    }

    float GetDistance(Vector2 pos1, Vector2 pos2)
    {
        Vector2 temp = new Vector2(pos1.x - pos2.x, pos1.y / 2 - pos2.y / 2);
    }
}
