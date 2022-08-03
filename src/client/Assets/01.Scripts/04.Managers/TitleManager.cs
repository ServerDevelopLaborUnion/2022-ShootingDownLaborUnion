using static Define;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TitleManager : MonoBehaviour
{

    [SerializeField]
    private float _fadeDuration = 0.5f;

    private void Update() {
        if(Input.anyKeyDown){
            FadeManager.Instance.FadeObject.DOFade(1f, _fadeDuration).OnComplete(()=>{
                SceneLoader.Load(SceneType.Login);
            });
        }
    }

}
