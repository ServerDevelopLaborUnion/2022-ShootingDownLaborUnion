using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffect : MonoBehaviour
{
    public enum Effect
    {
        Fast,
        High,
        Slow,
        Low
    }
    private CharacterBase _base = null;

    private void Awake()
    {
        _base = GetComponent<CharacterBase>();
    }
    
    public void GetEffectOnCharacter(CharacterStat.Stat stat, Effect effect ,float during)
    {
        StartCoroutine(GetEffect(stat, effect, during));
    }

    private IEnumerator GetEffect(CharacterStat.Stat stat, Effect effect, float during)
    {
        switch (stat)
        {
            case CharacterStat.Stat.ATKSPEED:
            case CharacterStat.Stat.SPEED:
                switch (effect)
                {
                    case Effect.Fast:
                        _base.Stat.AddStat(stat, 1);
                        yield return Yields.WaitForSeconds(during);
                        _base.Stat.AddStat(stat, -1);
                        break;
                    case Effect.Slow:
                        _base.Stat.AddStat(stat, -1);
                        yield return Yields.WaitForSeconds(during);
                        _base.Stat.AddStat(stat, 1);
                        break;
                }
                break;
        }
    }

}
