using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharacterBase;

public class CharacterAnimation : MonoBehaviour
{
    private Animator _animator;
    private Animator _shadowAnimator;

    private int _doMove = Animator.StringToHash("DoMove");
    private int _doAttack = Animator.StringToHash("DoAttack");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _shadowAnimator = _shadowTransform.GetComponent<Animator>();
    }

    public void PlayMoveAnime(float velocity)
    {
        _animator.SetBool(_doMove, velocity > 0);
        _shadowAnimator.SetBool(_doMove, velocity > 0);
    }

    public void PlayAttackAnime()
    {
        _animator.SetTrigger(_doAttack);
        _shadowAnimator.SetTrigger(_doAttack);
    }
}
