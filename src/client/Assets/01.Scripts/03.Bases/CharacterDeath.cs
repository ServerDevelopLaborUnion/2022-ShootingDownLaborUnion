using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static CharacterBase;

public class CharacterDeath : MonoBehaviour
{
    [SerializeField] private UnityEvent OnCharacterDied;

    public void CharacterDead()
    {
        _isDying = true;
        OnCharacterDied?.Invoke();
    }

    public void EndDeath()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
