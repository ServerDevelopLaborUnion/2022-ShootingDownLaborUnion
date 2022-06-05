using static Define;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleCameraMove : MonoBehaviour
{
    [SerializeField]
    private float _duration = 4f;

    [SerializeField]
    private float _endValue = 0.1f;

    [SerializeField]
    private AnimationCurve _tweenCurve;
    private void Start() {

        MainCam.transform.DOMoveY(_endValue, _duration).SetEase(_tweenCurve).SetLoops(-1, LoopType.Yoyo);
    }
}
