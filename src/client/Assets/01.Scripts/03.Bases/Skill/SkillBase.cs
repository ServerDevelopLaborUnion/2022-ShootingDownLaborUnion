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
    protected PlayerBase _base = null;
    protected Collider2D _playerCol = null;

    protected bool _isSkill = false;


    protected virtual void Awake() {
        _coolTimeImage = UIManager.Instance.SkillCoolTimeImage;
        _base = transform.parent.GetComponent<PlayerBase>();
        _playerCol = _base.GetComponent<Collider2D>();
    }

    public virtual void UseSkill()
    {
        
    }

    protected virtual IEnumerator UsedSkill(){
        _coolTimeImage.fillAmount = 0f;
        _coolTimeImage.color = Color.gray;

        for (; _coolTimeImage.fillAmount < 1f; ){
            _coolTimeImage.fillAmount += Time.deltaTime / _coolTime;
            yield return null;
        }

        _isSkill = false;
        _coolTimeImage.color = Color.white;
    }

    public float GetDistance(Vector2 pos1, Vector2 pos2)
    {
        float x = Mathf.Pow(pos1.x - pos2.x, 2);
        float y = Mathf.Pow(pos1.y * 2 - pos2.y * 2, 2);
        return Mathf.Sqrt(x + y);
    }
}
