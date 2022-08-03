using static Define;
using static Yields;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TankerSkill : SkillBase
{
    [SerializeField]
    private float _range = 3f;
    public UnityEvent OnSkillUsed = null;
    public UnityEvent OnSkillEnded = null;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = true;
    }

    public override void UseSkill()
    {
        if (_isSkill)
            return;
        // TOOD: ?뚮젅?댁뼱 醫뚯슦 ?꾨뒗嫄?留됯린
        OnSkillUsed?.Invoke();
        _isSkill = true;
        _animator.Play("Skill");
    }

    protected void EventUseSkill()
    {
        Skill();
    }

    protected void EventEndSkill()
    {
        //TODO: ?뚮젅?댁뼱 醫뚯슦 ?꾨뒗嫄??湲?
        StartCoroutine(UsedSkill());
        OnSkillEnded?.Invoke();
    }

    private void Skill()
    {
        List<Entity> enemies = NetworkManager.Instance.entityList.FindAll((entity) => entity.Data.Type == EntityType.Enemy);
        List<Entity> closeEnemies = enemies.FindAll((entity) => GetDistance(transform.parent.position, entity.transform.position) <= _range);
        closeEnemies.ForEach((enemy)=> enemy.GetComponent<CharacterDamage>().GetDamaged(_base.Stat.AD, _playerCol));
    }




}
