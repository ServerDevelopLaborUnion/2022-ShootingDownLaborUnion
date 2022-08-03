using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(TMP_InputField))]
public class NicknameInput : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject _decidePanel;
    [SerializeField]
    private TMP_Text _decideNickname;


    [Header("흔들기")]
    [SerializeField]
    private float _shakeDuration = 0.5f;
    [SerializeField]
    private float _strength = 1f;
    [SerializeField]
    private int _vivrato = 10;
    [SerializeField]
    private float _randomness = 90;


    private TMP_InputField _inputField;

    private TMP_Text _placeholder;

    private string _tempPlaceholder;


    private readonly string[] RANDOMFRONTNAMES = { "눈치보는", "고약한", "멋진", "무서운", "놀라운", "대단한", "멋있는", "예쁜", "화난", "긴장한", "못생긴", "잘생긴" };

    private readonly string[] RANDOMLASTNAMES = { "바지", "티셔츠", "고블린", "대장군", "핸드폰", "라떼", "한국인", "사람", "마우스", "쓰레기" };

    private bool _isShaking;
    private void Start()
    {
        WebSocket.Client.SubscribeUserEvent("ChangeNickname", (data) =>
        {
            Storage.CurrentUser.SetUserName(data);
        });


        _inputField = GetComponent<TMP_InputField>();
        _placeholder = _inputField.placeholder.GetComponent<TMP_Text>();

        _tempPlaceholder = _placeholder.text;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnClickChoose();
        }
    }

    public void OnEndEdit(string value)
    {
        if (value == string.Empty)
        {
            _placeholder.text = _tempPlaceholder;
        }
    }

    public void OnClickSelect(){
        WebSocket.Client.UserEvent("ChangeNickname", _inputField.text);
        Debug.Log(FadeManager.Instance.FadeObject);
        FadeManager.Instance.FadeObject.DOFade(1f, 1f).OnComplete(() =>
        {
            SceneLoader.Load(SceneType.Lobby);
        });
    }

    public void OnClickRandomlName()
    {
        int firstRandom = Random.Range(0, RANDOMFRONTNAMES.Length);

        int lastRandom = Random.Range(0, RANDOMLASTNAMES.Length);

        _inputField.text = $"{RANDOMFRONTNAMES[firstRandom]}{RANDOMLASTNAMES[lastRandom]}";
    }

    public void OnClickChoose()
    {
        if(_isShaking)return;

        
        if(!CheckSuitableNickname()){
            _isShaking = true;
            _inputField.text = "잘못된 이름입니다";
            _inputField.textComponent.color = Color.red;
            _inputField.textComponent.transform.DOShakePosition(_shakeDuration, _strength, _vivrato, _randomness).OnComplete(() =>
            {
                _isShaking = false;
                _inputField.text = string.Empty;
                _inputField.textComponent.color = Color.white;
            });
            return;
        }

        _decidePanel.SetActive(true);

        _decideNickname.text = _inputField.text;
        _decidePanel.transform.localScale = new Vector3(0.01f, 0f);
        Sequence sequence;
        sequence = DOTween.Sequence();

        sequence.Append(_decidePanel.transform.DOScaleY(1f, 0.1f).SetEase(Ease.InElastic));
        sequence.Append(_decidePanel.transform.DOScaleX(1f, 0.2f).SetEase(Ease.InElastic));

    }

    private bool CheckSuitableNickname(){
        if(_inputField.text == string.Empty){
            return false;
        }

        for (int i = 0; i < _inputField.text.Length; ++i){
            if (_inputField.text[i] == '　' || _inputField.text[i] == 'ㅤ'){
                return false;
            }
        }
        return true;
    }

    public void OnClickCancelNickname()
    {
        _decidePanel.transform.localScale = Vector3.one;
        Sequence sequence;
        sequence = DOTween.Sequence();

        sequence.Append(_decidePanel.transform.DOScaleX(0.01f, 0.1f).SetEase(Ease.InElastic));
        sequence.Append(_decidePanel.transform.DOScaleY(0f, 0.2f).SetEase(Ease.InElastic));
        sequence.AppendCallback(
            () =>
            {
                _decidePanel.SetActive(false);
            }
        );
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_inputField.text == string.Empty)
        {
            _placeholder.text = string.Empty;
        }
    }

    public void OnValueChange(string value)
    {
        _inputField.text = value.Replace(" ", "");
    }

}
