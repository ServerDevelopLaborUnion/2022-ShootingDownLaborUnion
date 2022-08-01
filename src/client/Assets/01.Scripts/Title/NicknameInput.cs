using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NicknameInput : MonoBehaviour
{
    
    private TMP_InputField _nicknameInput;

    public void TrimNickName(string name){
        name.Trim();
    }
}
