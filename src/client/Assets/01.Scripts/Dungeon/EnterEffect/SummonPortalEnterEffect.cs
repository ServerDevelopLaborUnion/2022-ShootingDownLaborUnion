using static Yields;

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


    private int SUMMONPORTAL = Animator.StringToHash("SummonPortal 2");

    private Animator _animator = null;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.DOFade(0f, 0f);
    }

    protected void EventSpawnPlayer()
    {
        _player.DOFade(1f, _fadeDuration);
    }


    // bool isCan;
    // float timer = 0f;
    // private void Update()
    // {
    //     if (isCan)
    //     {
    //         timer += Time.deltaTime;
    //     }
    // }

    // protected void EventTimer()
    // {
    //     isCan = true;
    // }

    // protected void DebugTimer()
    // {
    //     Debug.Log(timer);
    // }

    public override void EnterDirecting()
    {
        _spriteRenderer.DOFade(1f, 0f);
        _animator.Play(SUMMONPORTAL);
    }

    protected void EventFalseObject(){
        transform.localScale = Vector3.zero;
        Debug.Log("žÍ±î");
    }



    public override float GetAmountDuration()
    {
        _amountDuration = _animationClip.length;
        return _amountDuration;
    }
}
