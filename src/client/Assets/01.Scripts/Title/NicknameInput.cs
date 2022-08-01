using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NicknameInput : MonoBehaviour
{
    
    private TMP_InputField _nicknameInput;

    private TMP_Text _placeholderText;

    private void Start() {
        _nicknameInput = GetComponent<TMP_InputField>();
        _placeholderText = _nicknameInput.placeholder.GetComponent<TMP_Text>();

        
    }

    public void TrimNickName(string name){
        Debug.Log(name.Replace(' ', string.Empty[0]));
    }
}
