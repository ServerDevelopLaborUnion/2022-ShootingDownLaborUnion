using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System;

public class RoomManager : MonoSingleton<RoomManager>
{
    [SerializeField]
    private List<RolePanel> _rolePanels = new List<RolePanel>();

    [SerializeField]
    private TMP_Text _titletext;
    [SerializeField]
    private TMP_Text _userCountText;

    [SerializeField]
    private RectTransform _choosePanelRect;

    [SerializeField]
    private Transform _roomCanvasTransform;

    [SerializeField]
    private RectTransform _chooseBlockPanel;

    private User _masterUser;
    private void Start()
    {
        WebSocket.Client.SubscribeRoomEvent("UserJoined", (data) =>
        {
            var user = JsonUtility.FromJson<User>(data);
            if (Storage.CurrentRoom.Users.FirstOrDefault(x => x.UUID == user.UUID) == null)
            {
                Storage.CurrentRoom.Users.Add(user);
            }
            OnUserJoin(user);
        });

        WebSocket.Client.SubscribeRoomEvent("UserLeft", (data) =>
        {
            var user = JsonUtility.FromJson<User>(data);
            Storage.CurrentRoom.LeftUser(user);
            OnUserLeave(user);
        });

        WebSocket.Client.SubscribeRoomEvent("StartGame", (data) =>
        {
            OnStartGame();
        });

        WebSocket.Client.SubscribeRoomEvent("UserUpdated", (data) =>
        {
            Debug.Log($"data : {data}");
            var user = JsonConvert.DeserializeObject<User>(data);
            Debug.Log($"user : {JsonConvert.SerializeObject(user)}");
            OnUpdateRole(user, (int)user.Role, user.IsReady);
        });

        WebSocket.Client.SubscribeRoomEvent("Kicked", (data) =>
        {
            OnKicked();
        });

        foreach (User user in Storage.CurrentRoom.Users)
        {
            if (!user.IsReady) continue;
            OnUpdateRole(user, (int)user.Role, user.IsReady);
            if (user.IsMaster)
            {
                _masterUser = user;
            }
        }
        if (_masterUser == null)
        {
            _masterUser = Storage.CurrentUser;
        }
        UpdateText();
    }

    private void OnKicked()
    {
        LeaveRoom();
    }

    private void UpdateText()
    {
        _titletext.text = $"{_masterUser.Name} ¥‘¿« {Storage.CurrentRoom.Info.Name} πÊ";
        _userCountText.text = $"{Storage.CurrentRoom.Users.Count}/4";
    }

    public void OnUserJoin(User user)
    {
        //_userNameTexts[Storage.CurrentRoom.Users.Count].text = user.Name;
        UpdateText();
    }

    public void OnUserLeave(User user)
    {
        _rolePanels[(int)user.Role].ActiveReadyPanel(false);
        UpdateText();
        OnUpdateRole(user, (int)user.Role, false);
    }

    private void SetChoosePanel(int role, bool isActive)
    {
        _chooseBlockPanel.gameObject.SetActive(isActive);
        if (isActive)
        {
            RectTransform rect = _rolePanels[role].GetComponent<RectTransform>();

            _choosePanelRect.pivot = rect.pivot;
            _choosePanelRect.anchorMin = rect.anchorMin;
            _choosePanelRect.anchorMax = rect.anchorMax;
            _choosePanelRect.position = rect.position;

            _chooseBlockPanel.SetParent(_choosePanelRect);
        }
        else
        {
            _chooseBlockPanel.SetParent(_roomCanvasTransform);
        }
        //UI ????????? ????????????
        // ???????????? ????????? ??????????? UI????????????

    }

    public void OnUpdateRole(User user, int role, bool isReady)
    {
        user.IsReady = isReady;

        _rolePanels[role].ActiveReadyPanel(user.IsReady);

        user.Role = (RoleType)role;

        if (user.IsReady)
        {
            _rolePanels[role].SetNameText(user.Name);
            if (CheckAllUserIsReady() && Storage.CurrentRoom.Users.Count == RoomInfo.MaxPlayers)
            {
                _rolePanels[(int)_masterUser.Role].ActiveStartBtn(true);
            }
        }
        else
        {
            if (!CheckAllUserIsReady() && Storage.CurrentRoom.Users.Count != RoomInfo.MaxPlayers)
            {
                _rolePanels[role].ActiveStartBtn(false);
            }
            _rolePanels[role].SetNameText(string.Empty);
        }
    }

    public void UpdateUser(int role, bool isReady)
    {
        SetChoosePanel(role, isReady);

        Storage.CurrentUser.Role = (RoleType)role;
        Storage.CurrentUser.IsReady = isReady;

        WebSocket.Client.RoomEvent("UserUpdated", JsonConvert.SerializeObject(Storage.CurrentUser));
    }

    public void ClickStartGame()
    {
        WebSocket.Client.RoomEvent("StartGame", "");
    }

    public void OnStartGame()
    {
        SceneLoader.Load(SceneType.Game);
    }

    public void OnClickExit()
    {
        WebSocket.Client.RoomEvent("UserLeft", JsonConvert.SerializeObject(Storage.CurrentUser));
        LeaveRoom();
    }

    private bool CheckAllUserIsReady()
    {
        for (int i = 0; i < Storage.CurrentRoom.Users.Count; ++i)
        {
            if (!Storage.CurrentRoom.Users[i].IsReady)
            {
                return false;
            }
        }
        return true;
    }

    public void LeaveRoom()
    {
        SceneLoader.Load(SceneType.Lobby);
        Storage.CurrentRoom = null;
        Storage.CurrentUser.IsReady = false;
        Storage.CurrentUser.IsMaster = false;
    }
}
