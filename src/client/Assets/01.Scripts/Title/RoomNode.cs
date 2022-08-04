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
    private RoomInfo _roomInfo = null;

    public void SetInfo(RoomInfo roomInfo)
    {
        _roomInfo = roomInfo;
        _roomEnterButton = GetComponent<Button>();
        this.roomName.text = _roomInfo.Name;
        this.personnel.text = $"{_roomInfo.PlayerCount}/{RoomInfo.MaxPlayers}";
        isPrivate.gameObject.SetActive(_roomInfo.IsPrivate);
        _roomEnterButton.onClick.AddListener(() =>
        {
            if (_roomInfo.IsPrivate)
            {
                LobbyManager.Instance.PasswordForm.Show(_roomInfo);
            }
            else
            {
                LobbyManager.Instance.JoinRoom(_roomInfo.UUID);
            }
        });
    }
}
