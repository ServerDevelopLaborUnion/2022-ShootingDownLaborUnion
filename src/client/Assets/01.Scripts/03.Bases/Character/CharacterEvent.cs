using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvent : MonoBehaviour
{
    private CharacterBase _base = null;
    public UnityEvent DoAttack = new UnityEvent();
    public UnityEvent DoMove = new UnityEvent();
    public UnityEvent DoFlipLeft = new UnityEvent();
    public UnityEvent DoFlipRight = new UnityEvent();
    public UnityEvent DoDie = new UnityEvent();

    private void Awake()
    {
        _base = GetComponent<CharacterBase>();
        Transform visualTransform = transform.Find("Visual Sprite");
        CharacterAttack characterAttack = visualTransform.GetComponent<CharacterAttack>();
        CharacterRenderer characterRenderer = visualTransform.GetComponent<CharacterRenderer>();
        CharacterAnimation characterAnimation = visualTransform.GetComponent<CharacterAnimation>();
        CharacterMove characterMove = GetComponent<CharacterMove>();
        CharacterDeath characterDeath = visualTransform.GetComponent<CharacterDeath>();
        DoAttack.AddListener(() => characterAttack.DoAttack());
        DoMove.AddListener(() => characterAnimation.PlayMoveAnime(true));
        DoFlipLeft.AddListener(() => characterRenderer.FlipCharacter(Vector2.left));
        DoFlipRight.AddListener(() => characterRenderer.FlipCharacter(Vector2.right));
        DoDie.AddListener(() => characterDeath.CharacterDead());
    }
    public void InvokeAction(string eventName)
    {
        switch (eventName)
        {
            case "DoMove":
                DoMove?.Invoke();
                break;
            case "DoAttack":
                DoAttack.Invoke();
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

        }
    }
}
