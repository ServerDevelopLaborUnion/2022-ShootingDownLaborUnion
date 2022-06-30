using static Yields;

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
    [SerializeField]
    private float _smokeFadeDuration = 0.5f;

    private int SMOKEEFFECT = Animator.StringToHash("SmokeEffect");

    private Animator _animator = null;

    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    protected void EventSpawnPlayer(){
        _player.DOColor(_whiteColor, 1f);
    }

    public override void EnterDirecting()
    {
        _animator.Play(SMOKEEFFECT);
        StartCoroutine(SmokeDisappear());
    }
    private IEnumerator SmokeDisappear(){
        yield return WaitForSeconds(_animationClip.length);
        _spriteRenderer.DOFade(0f, _smokeFadeDuration);
    }

    public override float GetAmountDuration()
    {
        _amountDuration = _animationClip.length + _smokeFadeDuration;
        return _amountDuration;
    }
}
