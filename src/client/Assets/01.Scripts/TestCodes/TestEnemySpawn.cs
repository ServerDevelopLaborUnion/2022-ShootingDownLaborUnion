using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject TargetPlayer;
    [SerializeField]
    private GameObject Enemey;
    [SerializeField]
    private float spawnDelay = 3f;

    [SerializeField]
    private float radius = 7f;

    private void Start()
    {
        StartCoroutine("SpawnEnemey");
    }
    
    private IEnumerator SpawnEnemey()
    {
        while(true)
        {
            for (float angle = 0; angle < 360; angle += 10)
            {
                float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius + TargetPlayer.transform.position.x;
                float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius + TargetPlayer.transform.position.y;
                Vector2 enemyPos = new Vector2(x, y);
                // TODO : uuid, owner설정
                EntityData entityData = new EntityData("uuid", "owner", "name", enemyPos, Quaternion.Euler(0f, 0f, 0f), EntityType.Enemy);
                Entity.EntityCreate(entityData);
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }
}
