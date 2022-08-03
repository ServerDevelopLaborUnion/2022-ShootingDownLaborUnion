using static Define;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public enum FADECHILDS
{
    FADEOBJECT,
    TOPBAR,
    BOTTOMBAR,
}

public class FadeManager : MonoSingleton<FadeManager>
{
    private RectTransform TopBar
    {
        get
        {
            if (_topBar == null)
            {
                _topBar = FadeParent.GetChild((int)FADECHILDS.TOPBAR).GetComponent<RectTransform>();
            }
            return _topBar;
        }
    }

    private RectTransform _topBar;

    private RectTransform BottomBar
    {
        get
        {
            if (_bottomBar == null)
            {
                _bottomBar = FadeParent.GetChild((int)FADECHILDS.BOTTOMBAR).GetComponent<RectTransform>();
            }
            return _bottomBar;
        }
    }

    private RectTransform _bottomBar;

    public Image FadeObject
    {
        get
        {
            if (_fadeObject == null)
            {
                _fadeObject = FadeParent.GetChild((int)FADECHILDS.FADEOBJECT).GetComponent<Image>();
            }
            return _fadeObject;
        }
    }

    private Image _fadeObject;

    private float HideBarY
    {
        get{
            if(_hideBarY != Mathf.Abs(TopBar.anchoredPosition.y)){
                _hideBarY = Mathf.Abs(TopBar.anchoredPosition.y);
            }
            return _hideBarY;
        }
    }

    private float _hideBarY = 60f;

    public void ShowBar(bool isShow, float duration = 1f)
    {
        if (isShow)
        {
            TopBar.DOAnchorPosY(-HideBarY, duration);
            BottomBar.DOAnchorPosY(HideBarY, duration);
        }
        else
        {
            TopBar.DOAnchorPosY(HideBarY, duration);
            BottomBar.DOAnchorPosY(-HideBarY, duration);
        }
    }

}
