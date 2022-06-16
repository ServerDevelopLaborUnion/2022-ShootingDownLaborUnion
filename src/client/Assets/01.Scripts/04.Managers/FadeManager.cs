using static Define;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public enum FADECHILDS
{
    FADEOBJECT,
    TOPBAR,
    BOTTOMBAR,
}

public class FadeManager : MonoSingleton<FadeManager>
{
    private RectTransform _topBar = null;

    private RectTransform _bottomBar = null;

    private float _hideBarY = 0f;
    private void Start()
    {
        _topBar = FadeParent.GetChild((int)FADECHILDS.TOPBAR).GetComponent<RectTransform>();
        _bottomBar = FadeParent.GetChild((int)FADECHILDS.BOTTOMBAR).GetComponent<RectTransform>();

        _hideBarY = Mathf.Abs(_topBar.anchoredPosition.y);

    }

    public void ShowBar(bool isShow)
    {
        if (isShow)
        {
            _topBar.DOAnchorPosY(_hideBarY, 1f);
            _bottomBar.DOAnchorPosY(-_hideBarY, 1f);
        }
        else
        {
            _topBar.DOAnchorPosY(-_hideBarY, 1f);
            _bottomBar.DOAnchorPosY(_hideBarY, 1f);
        }
    }
}
