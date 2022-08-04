using static Yields;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RedPortalEffect : BaseDungeonEnter
{
    [SerializeField]
    private SpriteRenderer _player;
    [SerializeField]
    private Sprite _playerEndSprite;

    [SerializeField]
    private Transform _playerEndPos;


    [SerializeField]
    private float _playerMoveDuration;

    [SerializeField]
    private float _scaleDuration = 0.5f;
    [SerializeField]
    private float _fadeDuration = 1f;

    private Animator _playerAnimator;
    // [Header("카메?��")]
    // [SerializeField]
    // private float _camDuration = 1f;
    // [SerializeField]
    // private float _camStrength = 3f;
    // [SerializeField]
    // private float _camRandomness = 90f;

    // [SerializeField]
    // private int _camVibrato = 10;

    private void Awake() {
        _playerAnimator = _player.GetComponent<Animator>();
    }

    public override void EnterDirecting()
    {
        StartCoroutine(EnteringDirect());
    }

    public override float GetAmountDuration()
    {
        _amountDuration = _scaleDuration + _fadeDuration + _playerMoveDuration + _scaleDuration;
        return _amountDuration;
    }

    private IEnumerator EnteringDirect()
    {
        // MainCam.DOShakePosition(_camDuration, _camStrength, _camVibrato, _camRandomness);
        // yield return WaitForSeconds(_camDuration + 0.5f);

        transform.DOScale(Vector3.one, _scaleDuration);
        yield return WaitForSeconds(_scaleDuration);

        _player.DOFade(1f, _fadeDuration).OnComplete(()=>{
            _playerAnimator.enabled = true;
        });
        yield return WaitForSeconds(_fadeDuration);

        _player.transform.DOMove(_playerEndPos.position, _playerMoveDuration).OnComplete(()=>{
            _playerAnimator.enabled = false;
            _player.sprite = _playerEndSprite;
        });
        yield return WaitForSeconds(_playerMoveDuration);
        
        transform.DOScale(Vector3.zero, _scaleDuration);
    }


}
