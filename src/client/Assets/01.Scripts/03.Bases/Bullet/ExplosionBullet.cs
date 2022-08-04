using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBullet : BulletAttack
{
    [SerializeField]
    private float _range = 1;
    [SerializeField]
    private GameObject _impactObject = null;
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            List<Entity> enemies = NetworkManager.Instance.entityList.FindAll((entity) => entity.Data.Type == EntityType.Enemy);
            List<Entity> closeEnemies = enemies.FindAll((entity) => Define.GetDistance(transform.position, entity.transform.position) <= _range);
            closeEnemies.ForEach((enemy) => enemy.GetComponent<CharacterDamage>().GetDamaged(_damage / 2, _col));
            GameObject explosionObject = Instantiate(_impactObject, other.transform.position, Quaternion.identity);
            explosionObject.transform.localScale = transform.localScale;

            gameObject.SetActive(false);
        }
    }
}
