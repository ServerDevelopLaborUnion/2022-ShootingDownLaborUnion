using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BulletAttack : MonoBehaviour
{
    public int _damage = 1;

    public Collider2D _col = null;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy")){
            other.GetComponent<CharacterDamage>().GetDamaged(_damage, _col);
            gameObject.SetActive(false);
        }
    }
}
