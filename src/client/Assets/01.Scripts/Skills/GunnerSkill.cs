using static Define;
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
    [SerializeField]
    private Light2D _light = null;
    [SerializeField]
    private UnTargetAttack _targetAttack = null;

    public override void UseSkill()
    {
        if (_isSkill)
            return;
        // TOOD: ?뚮젅?댁뼱 醫뚯슦 ?꾨뒗嫄?留됯린
        OnSkillUsed?.Invoke();
        _isSkill = true;
        _anime.PlaySkillAnime();
        WebSocket.Client.ApplyEntityAction(_base, "DoSkill");
    }

    protected override void EventUseSkill()
    {
        StartCoroutine(Skill());
        StartCoroutine(UsedSkill());
    }

    protected override void EventEndSkill()
    {
        //TODO: ?뚮젅?댁뼱 醫뚯슦 ?꾨뒗嫄??湲?
        base.EventEndSkill();
        OnSkillEnded?.Invoke();
    }

    private IEnumerator Skill()
    {
        _base.Effect.GetEffectOnCharacter(CharacterStat.Stat.ATKSPEED, CharacterEffect.Effect.Fast, 3);
        DOTween.To(() => _light.intensity, (v) => _light.intensity = v, 1, 0.5f);
        _targetAttack.ActiveSkill();
        yield return WaitForSeconds(3);
        EventEndSkill();
        DOTween.To(() => _light.intensity, (v) => _light.intensity = v, 0, 0.5f);
        _targetAttack.DeActiveSkill();
    }
}
