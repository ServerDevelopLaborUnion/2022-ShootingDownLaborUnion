using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class RoomManager : MonoSingleton<RoomManager>
{
    [SerializeField]
    private List<RolePanel> _rolePanels = new List<RolePanel>();

    [SerializeField]
    private TMP_Text _titletext;
    [SerializeField]
    private TMP_Text _userCountText;

    [SerializeField]
    private RectTransform _choosePanel;

    [SerializeField]
    private RectTransform _testPanel;

    private User _masterUser;
    private void Start()
    {
        // _choosePanel
        // UI ?�� CurrentRoom ?���? ?��?��?��?��
        foreach(User user in Storage.CurrentRoom.Users){
            SetRole((int)user.Role, user.IsReady);
            if(user.IsMaster){
                _masterUser = user;
            }
        }

        _titletext.text = $"{_masterUser.Name}?��?�� {Storage.CurrentRoom.Info.Name}";
        _userCountText.text = $"{Storage.CurrentRoom.Users.Count}/4";
    }

    public void OnUserJoin(User user)
    {
        //  UI ?��?��?�� ?��?��?��?��
        // ?��?��?��?�� ?��?��?�� �??��????�� UI?��?��?��?��
        //_userNameTexts[Storage.CurrentRoom.Users.Count].text = user.Name;
    }

    public void OnUserLeave(User user)
    {
        //UI ?��?��?�� ?��?��?��?��
        // ?��?��?��?�� ?��?��?�� �??��????�� UI?��?��?��?��

    }

    public void OnUpdateRole(User user, int role, bool isReady)
    {
        user.IsReady = isReady;
        _rolePanels[role].ActiveReadyPanel(user.IsReady);
        
        user.Role = (RoleType)role;

        if (user.IsReady)
        {
            _rolePanels[role].SetNameText(Storage.CurrentUser.Name);
            // ???�? ?��?��?�� UI++
            if ( CheckAllUserIsReady() && Storage.CurrentUser.IsMaster)
            {
                //?��?�� 버튼 ?���?
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
        // 무기�? ?��????�� ?��
        // 무기�? 바꿨?�� ?�� 무기�? 겹칠경우 ?��?��?�� ?��?���?
        WebSocket.Client.SetRole(role, isReady);
    }

    public void ClickStartGame()
    {
        WebSocket.Client.StartGame();
    }

    public void OnStartGame()
    {
        SceneLoader.Load(SceneType.Game);
    }
    
    private bool CheckAllUserIsReady(){
        for (int i = 0; i < Storage.CurrentRoom.Users.Count; ++i){
            if(!Storage.CurrentRoom.Users[i].IsReady){
                return false;
            }
        }
        return true;
    }
}
