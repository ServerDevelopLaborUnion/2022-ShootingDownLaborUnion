using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterDeath : MonoBehaviour
{
    [SerializeField] private UnityEvent OnCharacterDied;
    private CharacterBase _base;
    private void Start()
    {
        _base = transform.parent.GetComponent<CharacterBase>();
    }

    public void NoHP()
    {
        if(_base.Stat.HP <= 0 && !_base.Stat._isDying)
        {
            CharacterDead();
        }
    }

    public void CharacterDead()
    {
        _base.Stat._isDying = true;
        OnCharacterDied?.Invoke();
    }

    public void EndDeath()
    {
        if (_base == null) return;
        transform.parent.gameObject.SetActive(false);
    }
}
