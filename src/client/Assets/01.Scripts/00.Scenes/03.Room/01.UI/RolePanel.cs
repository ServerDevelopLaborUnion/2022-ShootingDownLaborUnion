using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class RolePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _readyPanel;
    [SerializeField]
    private GameObject _startBtn;

    [SerializeField]
    private TMP_Text _nameText;

    [SerializeField]
    private int _roleNumber;

    [SerializeField]
    private float _duration = 4f;
    [SerializeField]
    private float _strength = 8f;
    [SerializeField]
    private int _vivrato = 10;
    [SerializeField]
    private float _randomness = 90;

    private Tweener _startTextShakeTween;
    public void ActiveReadyPanel(bool isActive)
    {
        _readyPanel.SetActive(isActive);

    }

    public void SetNameText(string name)
    {
        _nameText.text = name;
    }


    public void OnClickReadyOrCancel(bool isReady)
    {
        RoomManager.Instance.SetRole(_roleNumber, isReady);
        
    }

    public void OnClickStartGame()
    {

        RoomManager.Instance.ClickStartGame();
    }

    public void ActiveStartBtn(bool isActive)
    {
        _startBtn.SetActive(isActive);
        if (isActive)
        {
            _startTextShakeTween = _startBtn.transform.GetChild(0).DOShakePosition(_duration, _strength, _vivrato, _randomness).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        }
        else
        {
            if (_startTextShakeTween != null)
            {
                _startTextShakeTween.Kill();
            }
        }
    }


}
