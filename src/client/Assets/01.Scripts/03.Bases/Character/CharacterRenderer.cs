using System;
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

    public void FlipCharacter(Vector2 pointerVec)
    {
        if (_characterBase.State.CurrentState.HasFlag(CharacterState.State.Attack)) return;
        if (pointerVec.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (pointerVec.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    public void RenderDamage()
    {
        StartCoroutine(DamageRenderCoroutine());
    }

    private IEnumerator DamageRenderCoroutine()
    {
        _spriteRenderer.material.SetColor("_MainColor", new Color(1, 1, 1, 1));
        yield return new WaitForSeconds(0.2f);
        _spriteRenderer.material.SetColor("_MainColor", new Color(1, 1, 1, 0));
        _characterBase.State.CurrentState &= ~CharacterState.State.Damaged;
    }
}
