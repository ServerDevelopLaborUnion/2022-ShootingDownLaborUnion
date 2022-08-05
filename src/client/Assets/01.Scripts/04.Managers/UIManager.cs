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

    [SerializeField]
    private Button _enemySummonBtn = null;

    [SerializeField]
    private Entity _enemyEntity = null;

    public Image SkillCoolTimeImage => _skillCoolTimeImage;


    private void Awake()
    {
        // _enemySummonBtn.onClick.AddListener(()=> WebSocket.Client.CreateEntityEvent(_enemyEntity));
    }

    public void CraeteEnemy()
    {
        Entity entity = _enemyEntity;
        entity.Data.Job = RoleType.Enemy;
        entity.Data.Type = EntityType.Enemy;
        
        WebSocket.Client.CreateEntityEvent(_enemyEntity);
    }

    public void SummonMoveImpact()
    {
        Instantiate(MoveImpact, Define.MousePos, Quaternion.identity);
    }
}
