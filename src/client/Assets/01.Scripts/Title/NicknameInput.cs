using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(TMP_InputField))]
public class NicknameInput : MonoBehaviour, IPointerClickHandler
{
    private TMP_InputField _inputField;

    private TMP_Text _placeholder;

    private string _tempPlaceholder;

    private void Start() {
        _inputField = GetComponent<TMP_InputField>();
        _placeholder = _inputField.placeholder.GetComponent<TMP_Text>();

        _tempPlaceholder = _placeholder.text;
    }

    public void OnEndEdit(string value){
        if(value == string.Empty){
            _placeholder.text = _tempPlaceholder;
            Debug.Log("호출!");
        }
        else{

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(_inputField.text == string.Empty){
            _placeholder.text = string.Empty;
        }
    }

    public void OnValueChange(string value)
    {
        Debug.Log("밸류 체인지");
        _inputField.text = value.Replace(" ", "");
    }

}
