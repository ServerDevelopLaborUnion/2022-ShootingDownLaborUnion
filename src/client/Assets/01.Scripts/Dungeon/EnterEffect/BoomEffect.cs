using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoomEffect : BaseDungeonEnter
{
    [SerializeField]
    private AnimationClip _animationClip = null;

    [SerializeField]
    private SpriteRenderer _player = null;

    [SerializeField]
    private Color _whiteColor = Color.white;

    [SerializeField]
    private float _fadeDuration = 1.5f;

    private int SMOKEEFFECT = Animator.StringToHash("SmokeEffect");

    private Animator _animator = null;

    private void Start() {
        _animator = GetComponent<Animator>();
    }

    protected void EventSpawnPlayer(){
        _player.DOColor(_whiteColor, 1f);
    }

    public override void EnterDirecting()
    {
        _animator.Play(SMOKEEFFECT);
    }

    public override float GetAmountDuration()
    {
        _amountDuration = _animationClip.length;
        return _amountDuration;
    }
}
