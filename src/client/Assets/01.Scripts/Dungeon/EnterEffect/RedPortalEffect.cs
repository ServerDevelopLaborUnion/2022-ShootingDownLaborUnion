using static Yields;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RedPortalEffect : BaseDungeonEnter
{
    [SerializeField]
    private Transform _playerTransform;

    [SerializeField]
    private Transform _playerEndPos;


    [SerializeField]
    private float _playerMoveDuration;

    [SerializeField]
    private float _scaleDuration = 0.5f;

    public override void EnterDirecting()
    {

    }

    private IEnumerator EnteringDirect()
    {
        transform.DOScale(Vector3.one, _scaleDuration);
        yield return WaitForSeconds(_scaleDuration);

        _playerTransform.DOMove(_playerEndPos.position, _playerMoveDuration);
        yield return WaitForSeconds(_playerMoveDuration);
        
        transform.DOScale(Vector3.zero, _scaleDuration);

    }
}
