﻿using static Define;
using static Yields;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public class GunnerSkill : SkillBase
{
    public UnityEvent OnSkillUsed = null;
    public UnityEvent OnSkillEnded = null;

    private Animator _animator;
    private PlayerBase _base = null;
    [SerializeField]
    private Light2D _light = null;
    [SerializeField]
    private UnTargetAttack _targetAttack = null;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = true;
        _base = transform.parent.GetComponent<PlayerBase>();
    }

    public override void UseSkill()
    {
        if (_isSkill)
            return;
        // TOOD: ?뚮젅?댁뼱 醫뚯슦 ?꾨뒗嫄?留됯린
        OnSkillUsed?.Invoke();
        _isSkill = true;
        _animator.Play("Skill");
    }

    protected void EventUseSkill()
    {
        StartCoroutine(Skill());
        StartCoroutine(UsedSkill());
    }

    protected void EventEndSkill()
    {
        //TODO: ?뚮젅?댁뼱 醫뚯슦 ?꾨뒗嫄??湲?
        _base.Stat.ChangeStat(CharacterStat.Stat.ATKSPEED, _base.Stat.AtkSpeed - 2);
        OnSkillEnded?.Invoke();
    }

    private IEnumerator Skill()
    {
        _base.Stat.ChangeStat(CharacterStat.Stat.ATKSPEED, _base.Stat.AtkSpeed + 2);
        DOTween.To(() => _light.intensity, (v) => _light.intensity = v, 1, 0.5f);
        _targetAttack.ActiveSkill();
        yield return WaitForSeconds(3);
        EventEndSkill();
        DOTween.To(() => _light.intensity, (v) => _light.intensity = v, 0, 0.5f);
        _targetAttack.DeActiveSkill();
    }
}
