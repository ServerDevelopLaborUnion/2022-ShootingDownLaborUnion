using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [SerializeField]
    private InputField _chatInput = null;
    [SerializeField]
    private TMP_Text _chatLog = null;

    private bool _isChatWritable = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_isChatWritable == true)
            {
                SendChatMessage(_chatInput.text);
                _chatInput.text = null;
                _chatInput.interactable = false;
                _isChatWritable = false;
            }
            else
            {
                _chatInput.interactable = true;
                _isChatWritable = true;
                _chatInput.Select();
            }
        }
    }

    public static void SendChatMessage(string message)
    {
        WebSocket.Client.SendChatMessage(message);
    }

    public void GetChatMessage(string message)
    {
        _chatLog.text += Environment.NewLine + message;
    }
}
