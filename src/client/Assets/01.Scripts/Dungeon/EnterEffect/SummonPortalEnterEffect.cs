using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SummonPortalEnterEffect : BaseDungeonEnter
{
    [SerializeField]
    private AnimationClip _animationClip = null;

    [SerializeField]
    private SpriteRenderer _player = null;

    [SerializeField]
    private float _fadeDuration = 1.5f;

    private int SUMMONPORTAL = Animator.StringToHash("SummonPortal");

    private Animator _animator = null;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    protected void EventSpawnPlayer()
    {
        _player.DOFade(1f, _fadeDuration);
    }

    public override void EnterDirecting()
    {
        _animator.Play(SUMMONPORTAL);
    }

    public override float GetAmountDuration()
    {
        _amountDuration = _animationClip.length;
        return _amountDuration;
    }
}
