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


        private void Start()
        {
            Client.OnLoginResponseMessage += OnLoginResponseMessage;
        }

        private void OnLoginResponseMessage(object sender, LoginResponseEventArgs e)
        {
            Debug.Log(e.Token);
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
            
            Client.Login(_username.text, _password.text);
            while (true)
            {
                yield return null;
                if (Client.ConnectionState == ConnectionState.LoggedIn)
                {
                    break;
                }
            }
            Debug.Log("Logined");
        }
    }
}
