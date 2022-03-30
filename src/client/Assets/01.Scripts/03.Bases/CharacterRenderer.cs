using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterRenderer : MonoBehaviour
{
    private CharacterBase _characterBase;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _shadowSpriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _characterBase = transform.parent.GetComponent<CharacterBase>();
        _shadowSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
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
}
