using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField]
    private PlayerBase player = null;
    
    private Image hpBar = null;
    private const float playerMaxHp = 5f;

    private void Start()
    {
        hpBar = GetComponent<Image>();
        Debug.Log(player.PlayerStat.HP);
    }

    public void SetHpBar()
    {
        Debug.Log(player.PlayerStat.HP);
        hpBar.fillAmount = player.PlayerStat.HP / playerMaxHp;
    }
}
