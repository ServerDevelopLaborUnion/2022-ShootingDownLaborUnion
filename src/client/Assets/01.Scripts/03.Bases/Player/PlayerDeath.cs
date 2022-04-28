using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : CharacterDeath
{
    public override void EndDeath()
    {
        if (_base == null) return;
        transform.parent.Find("Light 2D").GetComponent<PlayerLight>().TurnLight(false, () => transform.parent.gameObject.SetActive(false));
    }
}
