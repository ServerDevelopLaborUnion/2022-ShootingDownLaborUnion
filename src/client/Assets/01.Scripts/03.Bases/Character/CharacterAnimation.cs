using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterAnimation : MonoBehaviour
{
    private CharacterBase _characterBase;
    private Animator _animator;

    private int _doMove = Animator.StringToHash("DoMove");
    private int _doAttack = Animator.StringToHash("DoAttack");
    private int _doDie = Animator.StringToHash("DoDie");
    private int _doDamage = Animator.StringToHash("DoDamage");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterBase = transform.parent.GetComponent<CharacterBase>();
    }

    public void PlayMoveAnime(float velocity)
    {
        _animator.SetBool(_doMove, velocity > 0.1f);
    }

    public void PlayAttackAnime()
    {
        _animator.SetTrigger(_doAttack);
    }

    public void PlayDeathAnime()
    {
        _animator.SetTrigger(_doDie);
    }

    public void PlayDamageAnime()
    {
        _animator.SetTrigger(_doDamage);
    }
}
