using static Define;
using static Yields;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;
using System.Linq;

public class WizardSkill : SkillBase
{
    public UnityEvent OnSkillUsed = null;
    public UnityEvent OnSkillEnded = null;
    [SerializeField]
    private GameObject _meteorPrefab = null;
    List<Entity> enemies = new List<Entity>();
    public override void UseSkill()
    {

        if (_isSkill)
            return;
        enemies = NetworkManager.Instance.entityList.FindAll((entity) => entity.Data.Type == EntityType.Enemy);

        if (enemies.Count == 0)
            return;
        // TOOD: ?뚮젅?댁뼱 醫뚯슦 ?꾨뒗嫄?留됯린
        OnSkillUsed?.Invoke();
        _isSkill = true;
        _anime.PlaySkillAnime();
        EventUseSkill();
        if (WebSocket.Client.CheckIsOwnedEntity(_base))
        WebSocket.Client.ApplyEntityAction(_base, "DoSkill");
    }

    protected override void EventUseSkill()
    {
        base.EventUseSkill();
        StartCoroutine(Skill());
    }

    protected override void EventEndSkill()
    {
        //TODO: ?뚮젅?댁뼱 醫뚯슦 ?꾨뒗嫄??湲?
        base.EventEndSkill();
        StartCoroutine(UsedSkill());

        Debug.Log("실행함");
        OnSkillEnded?.Invoke();
    }

    private IEnumerator Skill()
    {
        for (int i = 0; i < 5; i++)
        {

            yield return WaitForSeconds(1);
            enemies = NetworkManager.Instance.entityList.FindAll((entity) => entity.Data.Type == EntityType.Enemy);
            if (enemies.Count == 0)
                break;
            Entity closestEnemy = enemies.OrderBy((enemy) => GetDistance(transform.position, enemy.transform.position)).FirstOrDefault();
            Vector2 target = closestEnemy.transform.position;
            GameObject bullet = Instantiate(_meteorPrefab, target + Vector2.up * 10, Quaternion.Euler(0, 0, -90));
            BulletBase bulletBase = bullet.GetComponent<BulletBase>();
            BulletAttack bulletAttack = bulletBase.GetComponent<BulletAttack>();
            bulletBase.InitBullet(target + Vector2.up, 21);
            bulletAttack._damage = _base.Stat.AD;

        }
        EventEndSkill();
    }
}
