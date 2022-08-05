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

    public void SummonMoveImpact()
    {
        Instantiate(MoveImpact, Define.MousePos, Quaternion.identity);
    }
}
