using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterAnimation : MonoBehaviour
{
    private CharacterBase _characterBase;
    private Animator _animator;
    private Animator _shadowAnimator;

    private int _doMove = Animator.StringToHash("DoMove");
    private int _doAttack = Animator.StringToHash("DoAttack");
    private int _doDie = Animator.StringToHash("DoDie");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterBase = transform.parent.GetComponent<CharacterBase>();
        _shadowAnimator = transform.GetChild(0).GetComponent<Animator>();
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

    public void PlayDeathAnime()
    {
        _animator.SetTrigger(_doDie);
        _shadowAnimator.SetTrigger(_doDie);
    }
}
