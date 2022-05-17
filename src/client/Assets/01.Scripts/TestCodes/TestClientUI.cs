using System.Net.Http;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocket;
using System;
using UnityEngine.UI;

namespace TestCodes
{
    public class TestClientUI : MonoBehaviour
    {
        [SerializeField] InputField _username;
        [SerializeField] InputField _password;
        [SerializeField] Text _connectionState;


        private void Start()
        {
            Client.OnLoginResponseMessage += OnLoginResponseMessage;
            Client.OnConnectionStateChanged += OnConnectionStateChanged;
        }

        private void OnConnectionStateChanged(ConnectionState connectionState)
        {
            _connectionState.text = connectionState.ToString();
            switch (connectionState)
            {
                case ConnectionState.Connected:
                    _connectionState.color = Color.green;
                    break;
                case ConnectionState.Connecting:
                    _connectionState.color = Color.yellow;
                    break;
                case ConnectionState.Disconnected:
                    _connectionState.color = Color.red;
                    break;
                case ConnectionState.LoggedIn:
                    _connectionState.color = Color.cyan;
                    break;
                case ConnectionState.None:
                    _connectionState.color = Color.gray;
                    break;
                default:
                    break;
            }
        }

        private void OnLoginResponseMessage(object sender, LoginResponseEventArgs e)
        {
            if (e.Success)
            {
                Debug.Log("Login Success! Token: " + e.Token);
            }
            else
            {
                Debug.Log("Login Failed");
            }
        }

        public void OnClickLogin()
        {
            if (_username.text.Length == 0 || _password.text.Length == 0)
            {
                Debug.Log("Username or password is empty");
                return;
            }

            StartCoroutine(Login());
        }

        private IEnumerator Login()
        {
            if (Client.ConnectionState == ConnectionState.LoggedIn)
            {
                Debug.LogWarning("Already logged in");
                yield break;
            }
            Debug.Log("Logining...");
            Client.Login(_username.text, _password.text);
            while (true)
            {
                yield return null;
                if (Client.ConnectionState != ConnectionState.Connected) break;
            }
            if (Client.ConnectionState == ConnectionState.LoggedIn)
                Debug.Log("Logined");
        }
    }
}
