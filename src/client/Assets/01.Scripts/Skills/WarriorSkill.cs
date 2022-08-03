using static Define;
using static Yields;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WarriorSkill : SkillBase
{
    [SerializeField]
    private GameObject _swordBullet = null;

    [SerializeField]
    private Transform _shootPos = null;

    public UnityEvent OnSkillUsed = null;
    public UnityEvent OnSkillEnded= null;

    private Animator _animator;

    private bool _isRight = false;

    private void Start() {
        _animator = GetComponent<Animator>();
        _animator.enabled = true;
    }

    public override void UseSkill()
    {
        if (_isSkill)
            return;
        // TOOD: ?뚮젅?댁뼱 醫뚯슦 ?꾨뒗嫄?留됯린
        OnSkillUsed?.Invoke();
        _isSkill = true;
        _isRight = MousePos.x >= transform.position.x;
        _animator.Play("Skill");
    }

    protected void EventUseSkill(){
        Skill();
    }

    protected void EventEndSkill()
    {
        //TODO: ?뚮젅?댁뼱 醫뚯슦 ?꾨뒗嫄??湲?
        StartCoroutine(UsedSkill());
        OnSkillEnded?.Invoke();
    }

    private void Skill(){
        GameObject g = Instantiate(_swordBullet, _shootPos.position, (_isRight) ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f));
        g.GetComponent<BulletAttack>()._damage = 2 + _base.Stat.AD;
        g.SetActive(true);
    }




}
