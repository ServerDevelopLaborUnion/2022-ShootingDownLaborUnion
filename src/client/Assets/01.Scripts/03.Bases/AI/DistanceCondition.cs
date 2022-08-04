using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DistanceCondition : AICondition
{
    [SerializeField]
    private float _range = 5f;
    private bool IsPlayerInRange(float range)
    {
        bool result = false;
        List<Entity> playerInRange = NetworkManager.Instance.playerList.FindAll((player) => Define.GetDistance(player.transform.position, transform.position) <= range);
        result = playerInRange.Count != 0;
        return result;
    }

    public override bool CheckCondition()
    {
        return IsPlayerInRange(_range);
    }
}
