using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Newtonsoft.Json;

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

        // UI ?ÔøΩÔøΩ CurrentRoom ?ÔøΩÔøΩÔø?? ?ÔøΩÔøΩ?ÔøΩÔøΩ?ÔøΩÔøΩ?ÔøΩÔøΩ
        foreach (User user in Storage.CurrentRoom.Users)
        {
            WebSocket.Client.SubscribeRoomEvent("UserJoined", (data) =>
            {
                var user = JsonUtility.FromJson<User>(data);
                Storage.CurrentRoom.AddUser(user);
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
                OnUpdateRole(user, (int)user.Role, user.IsReady);
            });

            SetRole((int)user.Role, user.IsReady);
            if (user.IsMaster)
            {
                _masterUser = user;
            }
        }
        if(_masterUser == null){
            _masterUser = Storage.CurrentUser;
        }
        Debug.Log(Storage.CurrentRoom.Info.Name);
        UpdateText();
    }

    private void UpdateText()
    {
        _titletext.text = $"{_masterUser.Name}?ãò?ùò {Storage.CurrentRoom.Info.Name} Î∞?";
        _userCountText.text = $"{Storage.CurrentRoom.Users.Count}/4";
    }

    public void OnUserJoin(User user)
    {
        //_userNameTexts[Storage.CurrentRoom.Users.Count].text = user.Name;
        UpdateText();
    }

    public void OnUserLeave(User user)
    {
        UpdateText();
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
        Debug.Log($"{user.Name} - {role}, {isReady}");
        user.IsReady = isReady;
        _rolePanels[role].ActiveReadyPanel(user.IsReady);

        user.Role = (RoleType)role;

        if (user.IsReady)
        {
            _rolePanels[role].SetNameText(Storage.CurrentUser.Name);
            if (CheckAllUserIsReady() && Storage.CurrentUser.IsMaster)
            {
                _rolePanels[role].ActiveStartBtn(true);
            }
        }
        else
        {
            _rolePanels[role].SetNameText(string.Empty);
        }
    }

    public void SetRole(int role, bool isReady)
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
}
