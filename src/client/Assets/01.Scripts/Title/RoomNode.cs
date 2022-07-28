using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomNode : MonoBehaviour
{
    [SerializeField]
    private Text roomName = null;

    [SerializeField]
    private Text personnel = null;

    [SerializeField]
    private Image isPrivate = null;

    private Button _roomEnterButton = null;

    public void SetInfo(RoomInfo roomInfo)
    {
        _roomEnterButton = GetComponent<Button>();
        this.roomName.text = roomInfo.Name;
        this.personnel.text = $"{roomInfo.PlayerCount}/{RoomInfo.MaxPlayers}";
        _roomEnterButton.onClick.AddListener(() =>
        {
            if (isPrivate)
            {
                // TODO: 패스워드 입력
                LobbyManager.Instance.JoinRoom(roomInfo.UUID);
            }
            else
            {
                LobbyManager.Instance.JoinRoom(roomInfo.UUID);
            }
        });
    }
}
