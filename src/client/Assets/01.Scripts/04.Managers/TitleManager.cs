using static Define;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{

    [SerializeField]
    private float _fadeDuration = 0.5f;

    private void Update() {
        if(Input.anyKeyDown){
            Tweener tweener = null;
            tweener = FadeManager.Instance.FadeObject.DOFade(1f, _fadeDuration).OnComplete(()=>{
                if(tweener != null){
                    tweener.Kill();
                }
                SceneLoader.Load(SceneType.Login);
            });
        }
    }

}
