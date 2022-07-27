using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField]
    private Image _skillCoolTimeImage;

    public Image SkillCoolTimeImage => _skillCoolTimeImage;
}
