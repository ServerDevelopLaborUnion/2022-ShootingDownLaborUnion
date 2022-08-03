using static Define;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _nickNamePanel;
    [SerializeField]
    private CanvasGroup _titlePanel;

    [SerializeField]
    private GameObject _decidePanel;

    [SerializeField]
    private TMP_InputField _nickNameInput;

    [SerializeField]
    private TMP_Text _decideNickNameText;



    [SerializeField]
    private float _fadeDuration = 0.3f;

    private bool _isShowNicknamePanel;
    private void Update() {
        if(Input.anyKeyDown && !_isShowNicknamePanel){
            _titlePanel.DOFade(0f, _fadeDuration).OnComplete(()=>_titlePanel.gameObject.SetActive(false));

            _nickNamePanel.DOFade(1f, _fadeDuration);
            _isShowNicknamePanel = true;
        }
    }


    public void OnClickSelectNickname(){
        FadeManager.Instance.FadeObject.DOFade(0f, 1f).OnComplete(()=>{
            //TODO: 씬 로딩, 닉네임을 (_nickNameInput.text로 설정해주기)
        });
    }

    public void OnClickCancelNickname(){
        Sequence sequence;
        sequence = DOTween.Sequence();

        sequence.Append(_decidePanel.transform.DOScaleX(0.05f, 0.3f).SetEase(Ease.InElastic));
        sequence.Append(_decidePanel.transform.DOScaleY(0f, 0.3f).SetEase(Ease.InElastic));
        sequence.AppendCallback(
            () =>
            {
                _decidePanel.SetActive(false);
            }
        );
    }
}
