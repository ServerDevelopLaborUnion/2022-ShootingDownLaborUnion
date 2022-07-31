using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField]
    private Image _skillCoolTimeImage;

    public Image SkillCoolTimeImage => _skillCoolTimeImage;



    public void CopyRectTransformSize(RectTransform copyFrom, RectTransform copyTo)
    {
        copyTo.pivot = copyFrom.pivot;
        copyTo.anchoredPosition = copyFrom.anchoredPosition;
        copyTo.anchorMin = copyFrom.anchorMin;
        copyTo.anchorMax = copyFrom.anchorMax;
        copyTo.sizeDelta = copyFrom.sizeDelta;
    }
}
