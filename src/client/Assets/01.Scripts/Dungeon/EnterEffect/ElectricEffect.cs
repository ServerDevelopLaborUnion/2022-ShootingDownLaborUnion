using static Yields;
using static Define;

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

    [SerializeField]
    private float _playerFadeDuration = 1f;

    [Header("카메라")]
    [SerializeField]
    private float _camDuration = 1f;
    [SerializeField]
    private float _camStrength = 0.1f;
    [SerializeField]
    private float _camRandomness = 90f;

    [SerializeField]
    private int _camVibrato = 30;

    private Animator _animator;

    private float _testTimer;

    private bool _isStart;

    private int ANIMATIONHASH;

    private Tweener _tweener;
    private void Awake()
    {
        ANIMATIONHASH = Animator.StringToHash(_animationClip.name);
        _animator = GetComponent<Animator>();
    }


    protected void EventSpawnPlayer()
    {
        _tweener = VCam.transform.DOShakePosition(_camDuration, _camStrength, _camVibrato, _camRandomness);
        _player.DOFade(1f, _playerFadeDuration);
    }

    public override void EnterDirecting()
    {
        _animator.Play(ANIMATIONHASH);
        StartCoroutine(ActiveFalse());
    }

    private IEnumerator ActiveFalse()
    {
        yield return WaitForSeconds(_animationClip.length);
        gameObject.SetActive(false);
        if(_tweener != null){
            _tweener.Kill();
        }
    }


    public override float GetAmountDuration()
    {
        _amountDuration = _animationClip.length + _playerFadeDuration;
        return _amountDuration;
    }
}
