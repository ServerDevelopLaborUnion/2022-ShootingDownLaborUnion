using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject TargetPlayer;
    [SerializeField] private GameObject Enemey;
    [SerializeField] private float spawnDelay = 3f;
    [SerializeField] private float radius = 7f;
    [SerializeField] private List<PhaseData> phases = new List<PhaseData>();

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private void SetPhase()
    {
        PhaseData phase = new PhaseData(40, SpawnType.circle);
        phases.Add(phase);
    }

    private IEnumerator Spawn()
    {
        for(int i =0; i<phases.Count; i++)
        {
            switch(phases[i].spawnType)
            {
                case SpawnType.circle:
                    yield return StartCoroutine(CircleSpawn(phases[i]));
                    break;
                case SpawnType.width:
                    yield return StartCoroutine(WidthSpawn(phases[i]));
                    break;
                case SpawnType.length:
                    yield return StartCoroutine(LengthSpawn(phases[i]));
                    break;
            }

            yield return new WaitForSeconds(10f);
        }
    }
    
    private IEnumerator CircleSpawn(PhaseData phaseData)
    {
        int enemyCnt = 0;
        while(true)
        {
            for (float angle = 0; angle < 360; angle += 360f / phaseData.maxEnemy)
            {
                if (enemyCnt >= phaseData.maxEnemy) yield break;
                enemyCnt++;
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

    private IEnumerator WidthSpawn(PhaseData phaseData)
    {
        for(int i = 0; i < phaseData.maxEnemy / 2; i++)
        {
            float x = -8f + 16f / (phaseData.maxEnemy - 1) * i * 2;
            float y = TargetPlayer.transform.position.y -2f;
            Vector2 enemyPos = new Vector2(x, y);
            // TODO : uuid, owner설정
            EntityData entityData = new EntityData("uuid", "owner", "name", enemyPos, Quaternion.Euler(0f, 0f, 0f), EntityType.Enemy);
            Entity.EntityCreate(entityData);

            enemyPos = new Vector2(enemyPos.x, enemyPos.y * -1);
            entityData = new EntityData("uuid", "owner", "name", enemyPos, Quaternion.Euler(0f, 0f, 0f), EntityType.Enemy);
            Entity.EntityCreate(entityData);
        }
        yield return null;
    }

    private IEnumerator LengthSpawn(PhaseData phaseData)
    {
        for (int i = 0; i < phaseData.maxEnemy / 2; i++)
        {
            float x = TargetPlayer.transform.position.x - 2f;
            float y = -5f + 12f / (phaseData.maxEnemy - 1) * i * 2;
            Vector2 enemyPos = new Vector2(x, y);
            // TODO : uuid, owner설정
            EntityData entityData = new EntityData("uuid", "owner", "name", enemyPos, Quaternion.Euler(0f, 0f, 0f), EntityType.Enemy);
            Entity.EntityCreate(entityData);

            enemyPos = new Vector2(enemyPos.x * -1, enemyPos.y);
            entityData = new EntityData("uuid", "owner", "name", enemyPos, Quaternion.Euler(0f, 0f, 0f), EntityType.Enemy);
            Entity.EntityCreate(entityData);


        }
        yield return null;
    }
}
