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
    [SerializeField]
    private List<PhaseData> phases = new List<PhaseData>();

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

            yield return new WaitForSeconds(1000f);
        }
    }
    
    private IEnumerator CircleSpawn(PhaseData phaseData)
    {
        int enemyCnt = 0;
        while(true)
        {
            for (float angle = 0; angle < 360; angle += 10)
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
        yield return null;
    }

    private IEnumerator LengthSpawn(PhaseData phaseData)
    {
        yield return null;
    }
}
