using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvent : MonoBehaviour
{
    private CharacterBase _base = null;
    private UnityEvent DoAttack = new UnityEvent();
    private UnityEvent PlayAttack = new UnityEvent();
    private UnityEvent DoMove = new UnityEvent();
    private UnityEvent DoFlipLeft = new UnityEvent();
    private UnityEvent DoFlipRight = new UnityEvent();
    private UnityEvent DoDie = new UnityEvent();
    private UnityEvent DoSkill = new UnityEvent();

    private void Awake()
    {
        _base = GetComponent<CharacterBase>();
        Transform visualTransform = transform.Find("Visual Sprite");
        CharacterAttack characterAttack = visualTransform.GetComponent<CharacterAttack>();
        CharacterRenderer characterRenderer = visualTransform.GetComponent<CharacterRenderer>();
        CharacterAnimation characterAnimation = visualTransform.GetComponent<CharacterAnimation>();
        CharacterMove characterMove = GetComponent<CharacterMove>();
        CharacterDeath characterDeath = visualTransform.GetComponent<CharacterDeath>();
        SkillBase skillBase = visualTransform.GetComponent<SkillBase>();
        DoAttack.AddListener(() => characterAttack.DoAttack());
        PlayAttack.AddListener(()=> characterAnimation.PlayAttackAnime());
        DoMove.AddListener(() => characterAnimation.PlayMoveAnime(true));
        DoFlipLeft.AddListener(() => characterRenderer.FlipCharacter(Vector2.left));
        DoFlipRight.AddListener(() => characterRenderer.FlipCharacter(Vector2.right));
        DoDie.AddListener(() => characterDeath.CharacterDead());
        DoSkill.AddListener(() => skillBase.UseSkill());
    }
    public void InvokeAction(string eventName)
    {
        Debug.Log(eventName);
        switch (eventName)
        {
            case "DoMove":
                DoMove?.Invoke();
                break;
            case "DoAttack":
                DoAttack.Invoke();
                break;
            case "PlayAttack":
                PlayAttack.Invoke();
                break;
            case "DoFlipLeft":
                DoFlipLeft?.Invoke();
                break;
            case "DoFlipRight":
                DoFlipRight?.Invoke();
                break ;
            case "DoDie":
                DoDie?.Invoke();
                break;
            case "DoSkill":
                DoSkill?.Invoke();
                break;

        }
    }
}
