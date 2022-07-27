using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkillBase : MonoBehaviour
{
    [SerializeField]
    private float _coolTime = 3f;

    private Image _coolTimeImage;

    protected bool _isSkill = false;


    protected virtual void Start() {
        _coolTimeImage = UIManager.Instance.SkillCoolTimeImage;
    }


    protected virtual IEnumerator UseSkilled(){
        _coolTimeImage.fillAmount = 0f;
        _coolTimeImage.color = Color.gray;

        for (; _coolTimeImage.fillAmount < 1f; ){
            _coolTimeImage.fillAmount += Time.deltaTime / _coolTime;
            yield return null;
        }

        _isSkill = true;
        _coolTimeImage.color = Color.white;
    }
}
