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
    private int _atkSpd = Animator.StringToHash("AttackSpeed");
    private int _moveSpd = Animator.StringToHash("MoveSpeed");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterBase = transform.parent.GetComponent<CharacterBase>();
    }

    public void PlayMoveAnime(bool isMove)
    {
        if (!(_characterBase.State.CurrentState.HasFlag(CharacterState.State.Attack) || _characterBase.State.CurrentState.HasFlag(CharacterState.State.Damaged) || _characterBase.State.CurrentState.HasFlag(CharacterState.State.Died)))
        {
            SetAnimationSpeed();
            _animator.SetBool(_doMove, isMove);
        }
    }

    public void PlayAttackAnime()
    {
        if (_characterBase.State.CurrentState.HasFlag(CharacterState.State.Died) || _characterBase.State.CurrentState.HasFlag(CharacterState.State.Attack)) return;
        SetAnimationSpeed();
        _animator.SetTrigger(_doAttack);
    }

    public void PlayDeathAnime()
    {
        SetAnimationSpeed();
        _animator.SetTrigger(_doDie);
    }

    public void PlayDamageAnime()
    {
        if (_characterBase.State.CurrentState.HasFlag(CharacterState.State.Damaged) || _characterBase.State.CurrentState.HasFlag(CharacterState.State.Died)) return;

        SetAnimationSpeed();
        _animator.SetTrigger(_doDamage);
    }

    private void SetAnimationSpeed()
    {
        _animator.SetFloat(_atkSpd, _characterBase.Stat.AtkSpeed);
        _animator.SetFloat(_moveSpd, (float)_characterBase.Stat.Speed / 5);
    }
}
