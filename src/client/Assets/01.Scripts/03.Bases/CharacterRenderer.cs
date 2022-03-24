using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharacterBase;


public class CharacterRenderer : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _shadowSpriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _shadowSpriteRenderer = _shadowTransform.GetComponent<SpriteRenderer>();
    }

    public void flipCharacter(Vector2 pointerVec)
    {
        if (_isAttacking) return;
        if(pointerVec.x - transform.position.x > 0)
        {
            _spriteRenderer.flipX = false;
            _shadowSpriteRenderer.flipX = false;
        }
        else if(pointerVec.x - transform.position.x < 0)
        {
            _spriteRenderer.flipX = true;
            _shadowSpriteRenderer.flipX = true;

        }
    }
}
