using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;
using System;

public class PlayerLight : MonoBehaviour
{
    private Light2D _light2D;
    void Start()
    {
        _light2D = GetComponent<Light2D>();
    }

    public void TurnLight(bool isOn, Action callBack = null)
    {
        Sequence seq;
        seq = DOTween.Sequence();
        if(isOn)
            seq.Append(DOTween.To(() => _light2D.intensity, (v) => _light2D.intensity = v, 1, 1));
        else
            seq.Append(DOTween.To(() => _light2D.intensity, (v) => _light2D.intensity = v, 0, 1));
        seq.AppendCallback(() => callBack?.Invoke());
    }
}
