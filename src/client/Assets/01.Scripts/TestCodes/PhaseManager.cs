using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PhaseManager : MonoBehaviour
{
    [SerializeField] private GameObject TargetPlayer;
    [SerializeField] private GameObject Enemey;
    [SerializeField] protected Text phaseTxt;
    [SerializeField] private float spawnDelay = 3f;
    [SerializeField] private float radius = 7f;
    [SerializeField] private List<PhaseData> phases = new List<PhaseData>();

    private List<EntityData> mapEntityDatas = new List<EntityData>();
    private int curPhase = 1;
    private float[,] spawnPoint = new float[4,2];

    private void Start()
    {
        spawnPoint[0, 0] = 10f;
        spawnPoint[0, 1] = 10f;
        
        spawnPoint[1, 0] = -10f;
        spawnPoint[1, 1] = 10f;

        spawnPoint[2, 0] = -10f;
        spawnPoint[2, 1] = -10f;

        spawnPoint[3, 0] = 10f;
        spawnPoint[3, 1] = -10f;

        //StartCoroutine(Spawn());
    }

    private void SetPhase()
    {
        PhaseData phase = new PhaseData(40);
        phases.Add(phase);
    }

    private IEnumerator SpawnEnemy(PhaseData data)
    {
        int enemyCnt = 0;
        while(true)
        {
            if (enemyCnt >= data.maxEnemy) break;
            enemyCnt++;
            float x = spawnPoint[enemyCnt % 4, 0];
            float y = spawnPoint[enemyCnt % 4, 1];
            Vector2 enemyPos = new Vector2(x, y);
            // TODO : uuid, owner설정
            EntityData entityData = new EntityData("uuid", "owner", "name", enemyPos, Quaternion.Euler(0f, 0f, 0f), EntityType.Enemy);
            Entity.EntityCreate(entityData);
            mapEntityDatas.Add(entityData);
            yield return new WaitForSeconds(spawnDelay);
        }
        CheckPhase();

    }

    private IEnumerator CheckPhase()
    {
        while(true)
        {
            if (mapEntityDatas.Count == 0)
            {
                if (curPhase == 3)
                    Debug.Log(0);
                    //TODO : ending
                else
                    ChangeFadeOut();

                break;
            }
        }
        yield return null;
    }

    private void ChangeFadeOut()
    {
        phaseTxt.text = string.Format($"Phase {curPhase}");
        phaseTxt.gameObject.SetActive(true);
        phaseTxt.material.DOFade(0, 1f).OnComplete(() =>
        {
            phaseTxt.gameObject.SetActive(false);
        });
    }

    /*private IEnumerator Spawn()
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
    }*/
}
