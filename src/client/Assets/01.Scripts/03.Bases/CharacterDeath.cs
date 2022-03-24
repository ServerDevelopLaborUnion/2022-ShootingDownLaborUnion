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
        OnCharacterDied?.Invoke();
    }

    public void EndDeath()
    {
        gameObject.SetActive(false);
    }
}
