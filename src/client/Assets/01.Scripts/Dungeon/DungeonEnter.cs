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
    private List<BaseDungeonEnter> _directingObjs = new List<BaseDungeonEnter>();


    [SerializeField]
    private float _moveDuration = 0.5f;
    [SerializeField]
    private float _waitDuration = 0.6f;

    [SerializeField]
    private float _endMoveDuration = 0.6f;
    private void Start()
    {
        //Time.timeScale = 2f;
        StartCoroutine(EnterMovement());
    }

    private IEnumerator EnterMovement()
    {
        FadeManager.Instance.ShowBar(true);

        MainCam.DOOrthoSize(4f, 1f);

        for (int i = 0; i < _directingObjs.Count; ++i)
        {
            Vector3 pos = _directTransform[i].position;
            pos.z = -10f;
            MainCam.transform.DOMove(pos, _moveDuration);
            yield return WaitForSeconds(_waitDuration);
            _directTransform[i].gameObject.SetActive(true);
            _directingObjs[i].EnterDirecting();
            yield return WaitForSeconds(_directingObjs[i].GetAmountDuration());
        }
        yield return WaitForSeconds(_endMoveDuration);
        
        FadeManager.Instance.ShowBar(false);

        MainCam.transform.DOMove(new Vector3(0f, 0f, -10f), 1f);
        MainCam.DOOrthoSize(10f, 1f);

    }
}
