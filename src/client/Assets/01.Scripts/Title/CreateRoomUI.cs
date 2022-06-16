using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomUI : MonoBehaviour
{
    [SerializeField]
    private Toggle _toggle = null;

    [SerializeField]
    private InputField _roomPassword = null;

    void Start()
    {
        _toggle.onValueChanged.AddListener(SetPasswordShow);
    }

    void SetPasswordShow(bool isShow)
    {
        _roomPassword.gameObject.SetActive(isShow);
    }
}
