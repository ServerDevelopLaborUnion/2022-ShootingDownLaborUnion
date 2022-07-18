using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LobbyScene
{
    public class RoomGenerateManager : MonoSingleton<RoomGenerateManager>
    {
        [Header("Room Generate")]
        [Tooltip("방 생성시 사용되는 방이름")]
        [SerializeField]
        private InputField _roomNameInputField = null;

        [Tooltip("비공개 방 생성시 사용되는 패스워드 입력 필드")]
        [SerializeField]
        private InputField _roomPasswordInputField = null;

        [Tooltip("방 비공개 여부 선택하는 Toggle")]
        [SerializeField]
        private Toggle _isPrivateToggle = null;

        /// <summary>
        /// 씬에서 Create버튼 눌렀을때 실행되는 함수
        /// </summary>
        public void CreateRoom()
        {
            LobbyManager.Instance.CreateRoom(_roomNameInputField.text, _roomPasswordInputField.text);
        }

        /// <summary>
        /// Private Toggle이 눌렸을 때 실행되는 함수
        /// </summary>
        public void OnValueChanged()
        {
            _roomPasswordInputField.gameObject.SetActive(_isPrivateToggle.isOn);
            _roomPasswordInputField.text = "";
        }
    }
}