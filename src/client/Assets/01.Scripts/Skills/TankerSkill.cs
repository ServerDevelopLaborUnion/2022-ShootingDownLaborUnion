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
    [SerializeField]
    private GameObject _hammerTrail = null;
    public UnityEvent OnSkillUsed = null;
    public UnityEvent OnSkillEnded = null;

    public override void UseSkill()
    {
        if (_isSkill)
            return;
        // TOOD: ?뚮젅?댁뼱 醫뚯슦 ?꾨뒗嫄?留됯린
        OnSkillUsed?.Invoke();
        _isSkill = true;
        _anime.PlaySkillAnime();
        _hammerTrail.SetActive(true);
        WebSocket.Client.ApplyEntityAction(_base, "DoSkill");
    }

    protected override void EventUseSkill()
    {
        base.EventUseSkill();
        Skill();
    }

    protected override void EventEndSkill()
    {
        //TODO: ?뚮젅?댁뼱 醫뚯슦 ?꾨뒗嫄??湲?
        base.EventEndSkill();
        WebSocket.Client.OnUserEvent["UserUsedSkill"].Invoke(Storage.CurrentUser.UUID);

        OnSkillEnded?.Invoke();
        _hammerTrail.SetActive(false);
    }

    private void Skill()
    {
        List<Entity> enemies = NetworkManager.Instance.entityList.FindAll((entity) => entity.Data.Type == EntityType.Enemy);
        List<Entity> closeEnemies = enemies.FindAll((entity) => GetDistance(transform.parent.position, entity.transform.position) <= _range);
        closeEnemies.ForEach((enemy)=> enemy.GetComponent<CharacterDamage>().GetDamaged(_base.Stat.AD, _playerCol));
    }




}
