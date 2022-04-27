using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeText : MonoBehaviour
{
    private Text _text = null;

    [Header("Fade Settings")]
    [Tooltip("Fade in duration")]
    [SerializeField]
    private float _fadeTime = 1f;

    [SerializeField]
    [Tooltip("Execution delay of fade-in and fade-out")]
    private float _waitTime = 0.5f;

    private void Start()
    {
        _text = GetComponent<Text>();
        StartCoroutine(Fade());
    }

    public IEnumerator Fade()
    {
        while (true)
        {
            yield return StartCoroutine(FadeOut());
            yield return StartCoroutine(FadeIn());
        }
    }

    public IEnumerator FadeIn()
    {
        _text.CrossFadeAlpha(1, _fadeTime, true);
        yield return new WaitForSeconds(_fadeTime + _waitTime);
    }

    public IEnumerator FadeOut()
    {
        _text.CrossFadeAlpha(0, _fadeTime, true);
        yield return new WaitForSeconds(_fadeTime);
    }
}
