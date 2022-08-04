using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static Define;

public class SkillBase : MonoBehaviour
{
    [SerializeField]
    private float _coolTime = 3f;

    private Image _coolTimeImage;
    protected PlayerBase _base = null;
    protected Collider2D _playerCol = null;
    protected CharacterAnimation _anime = null;

    protected bool _isSkill = false;

    protected bool _isIUseSkill = false;

    protected virtual void Awake() {
        _coolTimeImage = UIManager.Instance.SkillCoolTimeImage;
        _base = transform.parent.GetComponent<PlayerBase>();
        _playerCol = _base.GetComponent<Collider2D>();
        _anime = GetComponent<CharacterAnimation>();
    }

    private void Start() {
       
    }

    public virtual void UseSkill()
    {

    }

    protected virtual void EventUseSkill()
    {
        _base.State.CurrentState |= CharacterState.State.Skill;
    }
    protected virtual void EventEndSkill()
    {
        _base.State.CurrentState &= ~CharacterState.State.Skill;
    }


    protected virtual IEnumerator UsedSkill(){
        if (Storage.CurrentUser.UUID != _base.Data.OwnerUUID)
            yield break;
        _coolTimeImage.fillAmount = 0f;
        _coolTimeImage.color = Color.gray;

        for (; _coolTimeImage.fillAmount < 1f; ){
            _coolTimeImage.fillAmount += Time.deltaTime / _coolTime;
            yield return null;
        }

        _isSkill = false;
        _coolTimeImage.color = Color.white;
    }
}
