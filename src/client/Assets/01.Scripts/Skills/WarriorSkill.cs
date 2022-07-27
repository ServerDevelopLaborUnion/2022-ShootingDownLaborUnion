using static Yields;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill : SkillBase
{
    [SerializeField]
    private GameObject _light;

    [SerializeField]
    private float _lightGap = 0.5f;

    [SerializeField]
    private float _delayTime = 1f;

    [SerializeField]
    private int _lightCount = 5;


    private void Update() {
        if(Input.GetKeyDown(KeyCode.E) && !_isSkill){
            StartCoroutine(Skill());
        }    
    }

    protected override IEnumerator Skill()
    {
        base.Skill();

        float lightAngle = 360f / _lightCount;

        for (int i = 0; i < _lightCount; ++i){
            Vector2 _spawnPos = transform.position;
            _spawnPos.y -= 0.5f;

            GameObject g = Instantiate(_light, _spawnPos + new Vector2(Mathf.Cos(i * lightAngle * Mathf.Deg2Rad), Mathf.Sin(i * lightAngle * Mathf.Deg2Rad)) * _lightGap, Quaternion.Euler(0f, 0f, 90f));
            g.SetActive(true);
            yield return WaitForSeconds(_delayTime * 0.2f);
        }

        UsedSkill();
    }



}
