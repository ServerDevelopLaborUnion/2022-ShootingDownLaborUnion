using static Yields;
using static Define;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DungeonEnter : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _directTransform = new List<Transform>();
    [SerializeField]
    private List<Transform> _playerSpawnTransform = new List<Transform>();

    [SerializeField]
    private List<BaseDungeonEnter> _directingObjs = new List<BaseDungeonEnter>();

    [SerializeField]
    private List<GameObject> _activeObjs = new List<GameObject>();

    [field: SerializeField]
    private List<Entity> _roleEntity = new List<Entity>();

    [SerializeField]
    private float _moveDuration = 0.5f;
    [SerializeField]
    private float _waitDuration = 0.6f;

    [SerializeField]
    private float _endMoveDuration = 0.6f;
    private void Start()
    {
        StartCoroutine(EnterMovement());
    }

    private IEnumerator EnterMovement()
    {
        yield return WaitForSeconds(1f);
        _activeObjs.ForEach(x => x.SetActive(false));

        FadeManager.Instance.ShowBar(true);

        DOTween.To(
            () => VCam.m_Lens.OrthographicSize,
            value => VCam.m_Lens.OrthographicSize = value,
            4f, 1f
        );

        for (int i = 0; i < _directingObjs.Count; ++i)
        {
            _directTransform[i].position = _playerSpawnTransform[i].position;
            Vector3 pos = _directTransform[i].position;
            pos.z = -10f;
            VCam.transform.DOMove(pos, _moveDuration);
            yield return WaitForSeconds(_waitDuration);
            _directTransform[i].gameObject.SetActive(true);
            _directingObjs[i].EnterDirecting();
            yield return WaitForSeconds(_directingObjs[i].GetAmountDuration());
        }
        yield return WaitForSeconds(_endMoveDuration);
        
        FadeManager.Instance.ShowBar(false);

        _activeObjs.ForEach(x => x.SetActive(true));

        //TOOD: 플레이어들 각각 위치로 스폰시켜주기

        for (int i = 0; i < Storage.CurrentRoom.Users.Count; ++i){
            WebSocket.Client.CreateEntityEvent(_roleEntity[(int)Storage.CurrentRoom.Users[i].Role]);
        }
        for (int i = 0; i < _roleEntity.Count; ++i){
            _roleEntity[i].Data.Position = _playerSpawnTransform[i].position;
        }

            VCam.transform.DOMove(new Vector3(0f, 0f, -10f), 1f);
        DOTween.To(
            () => VCam.m_Lens.OrthographicSize,
            value => VCam.m_Lens.OrthographicSize = value,
            10f, 1f
        );

    }
}
