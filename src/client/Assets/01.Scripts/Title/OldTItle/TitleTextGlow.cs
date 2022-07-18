using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TitleTextGlow : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _tmpText;

    [SerializeField]
    private Color _startColor = new Color(0.8f, 0.8f, 0f, 0.5f);
    

    [SerializeField]
    private Color _endColor = new Color(0.8f, 0.8f, 0f, 0.5f);
    // Start is called before the first frame update
    void Start()
    {
        //_tmpText.fontSharedMaterial.DOColor(Color.red, 5f).OnComplete(() =>Debug.Log("끝"));
        //_tmpText.fontSharedMaterial.DOFloat(0f, ShaderUtilities.ID_GlowColor, 5f);

        
        _tmpText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, _startColor);
        DOTween.To(
            () => _tmpText.fontSharedMaterial.GetColor(ShaderUtilities.ID_GlowColor),
            color => _tmpText.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, color)
            , _endColor, 2f
        ).SetLoops(-1, LoopType.Yoyo);
    }

    
}
