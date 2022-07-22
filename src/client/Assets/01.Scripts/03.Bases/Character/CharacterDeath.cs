using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterDeath : MonoBehaviour
{
    [SerializeField] private UnityEvent OnCharacterDied;
    protected CharacterBase _base;
    private void Start()
    {
        _base = transform.parent.GetComponent<CharacterBase>();
    }

    public void NoHP()
    {
        if(_base.Stat.HP <= 0 && !_base.State.CurrentState.HasFlag(CharacterState.State.Died))
        {
            CharacterDead();
        }
    }

    public void CharacterDead()
    {
        _base.State.CurrentState |= CharacterState.State.Died;
        OnCharacterDied?.Invoke();
        Entity.DeleteEntity(_base.Data.UUID);
    }

    public virtual void EndDeath()
    {
        if (_base == null) return;
        transform.parent.gameObject.SetActive(false);
    }
}
