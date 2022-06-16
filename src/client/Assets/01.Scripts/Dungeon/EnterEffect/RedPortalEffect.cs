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
    private Transform _playerEndPos;


    [SerializeField]
    private float _playerMoveDuration;

    [SerializeField]
    private float _scaleDuration = 0.5f;
    [SerializeField]
    private float _fadeDuration = 1f;

    public override void EnterDirecting()
    {
        StartCoroutine(EnteringDirect());
    }

    public override float GetAmountDuration()
    {
        _amountDuration = _scaleDuration*2 + _fadeDuration + _playerMoveDuration;
        return _amountDuration;
    }

    private IEnumerator EnteringDirect()
    {
        transform.DOScale(Vector3.one, _scaleDuration);
        yield return WaitForSeconds(_scaleDuration);

        _player.DOFade(1f, _fadeDuration);
        yield return WaitForSeconds(_fadeDuration);

        _player.transform.DOMove(_playerEndPos.position, _playerMoveDuration);
        yield return WaitForSeconds(_playerMoveDuration);
        
        //TODO: SetActive를 꺼줘야할 듯
        transform.DOScale(Vector3.zero, _scaleDuration);

    }
}
