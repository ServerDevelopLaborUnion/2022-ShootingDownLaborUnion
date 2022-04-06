using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : CharacterBase
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetDamaged(1);
        }
    }
}
