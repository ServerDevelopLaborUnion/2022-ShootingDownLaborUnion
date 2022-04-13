using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : CharacterBase
{
    [SerializeField] new private Collider2D collider;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<CharacterDamage>().GetDamaged(1, collider);
        }
    }
}
