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


    private void Start()
    {
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
            MainCam.transform.DOMove(pos, 1f);
            yield return WaitForSeconds(1.2f);
            _directingObjs[i].gameObject.SetActive(true);
            _directingObjs[i].EnterDirecting();
            yield return WaitForSeconds(_directingObjs[i].GetAmountDuration() + 0.5f);
        }

        MainCam.transform.DOMove(new Vector3(0f, 0f, -10f), 1f);
        MainCam.DOOrthoSize(10f, 1f);

    }
}
