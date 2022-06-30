using static Yields;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ElectricEffect : BaseDungeonEnter
{
    [SerializeField]
    private AnimationClip _animationClip;

    [SerializeField]
    private SpriteRenderer _player;

    private Animator _animator;

    private float _testTimer;

    private bool _isStart;

    private int ANIMATIONHASH;

    private void Awake()
    {
        ANIMATIONHASH = Animator.StringToHash(_animationClip.name);
        _animator = GetComponent<Animator>();
    }

    protected void EventSpawnPlayer()
    {
        _player.DOFade(1f, 1f);
    }

    public override void EnterDirecting()
    {
        _animator.Play(ANIMATIONHASH);
        Debug.Log("엥/");
        StartCoroutine(ActiveFalse());
    }

        private IEnumerator ActiveFalse(){
        yield return WaitForSeconds(_animationClip.length);
        gameObject.SetActive(false);
    }


    public override float GetAmountDuration()
    {
        _amountDuration = _animationClip.length;
        return _amountDuration;
    }
}
