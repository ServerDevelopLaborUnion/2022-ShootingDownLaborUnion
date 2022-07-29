using static Define;
using static Yields;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill : SkillBase
{
    [SerializeField]
    private GameObject _swordBullet = null;

    [SerializeField]
    private Transform _shootPos = null;

    private Animator _animator;

    private bool _isRight = false;

    private void Start() {
        _animator = GetComponent<Animator>();
        _animator.enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !_isSkill)
        {
            // TOOD: 플레이어 좌우 도는거 막기
            _isSkill = true;
            _isRight = MousePos.x >= transform.position.x;
            _animator.Play("Skill");
        }
    }

    protected void EventUseSkill(){
        Skill();
    }

    protected void EventEndSkill()
    {
        //TODO: 플레이어 좌우 도는거 풀기
        StartCoroutine(UsedSkill());
    }

    private void Skill(){
        GameObject g = Instantiate(_swordBullet, _shootPos.position, (_isRight) ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f));
        g.SetActive(true);
    }




}
