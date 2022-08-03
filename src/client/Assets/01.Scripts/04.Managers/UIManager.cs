using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField]
    private Image _skillCoolTimeImage;

    [SerializeField]
    private GameObject MoveImpact = null;

    public Image SkillCoolTimeImage => _skillCoolTimeImage;

    public void SummonMoveImpact()
    {
        Instantiate(MoveImpact, Define.MousePos, Quaternion.identity);
    }
}
