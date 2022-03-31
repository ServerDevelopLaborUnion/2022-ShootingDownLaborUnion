using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterRenderer : MonoBehaviour
{
    private CharacterBase _characterBase;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _characterBase = transform.parent.GetComponent<CharacterBase>();
    }

    public void flipCharacter(Vector2 pointerVec)
    {
        if (_characterBase._isAttacking) return;
        if (pointerVec.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (pointerVec.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);

        }
    }

    public void RenderDamage()
    {

        _spriteRenderer.material.SetColor("_MainTex", Color.white);
    }
}
