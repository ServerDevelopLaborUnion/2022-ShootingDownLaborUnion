using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BulletAttack : MonoBehaviour
{
    public int _damage = 1;

    private Collider2D _col = null;

    protected virtual void Start() {
        _col = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy")){
            other.GetComponent<CharacterDamage>().GetDamaged(_damage, _col);
            gameObject.SetActive(false);
        }
    }
}
