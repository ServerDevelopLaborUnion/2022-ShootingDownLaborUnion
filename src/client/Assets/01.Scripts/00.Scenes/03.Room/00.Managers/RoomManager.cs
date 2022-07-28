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

    private User _masterUser;
    private void Start()
    {
        // UI 에 CurrentRoom 정보 업데이트
        foreach(User user in Storage.CurrentRoom.Users){
            SetRole((int)user.Role, user.IsReady);
            if(user.IsMaster){
                _masterUser = user;
            }
        }

        _titletext.text = $"{_masterUser.Name}님의 {Storage.CurrentRoom.Info.Name}";
        _userCountText.text = $"{Storage.CurrentRoom.Users.Count}/4";
    }

    public void OnUserJoin(User user)
    {
        //  UI 인원수 업데이트
        // 플레이어 닉네임 가져와서 UI업데이트
        //_userNameTexts[Storage.CurrentRoom.Users.Count].text = user.Name;
    }

    public void OnUserLeave(User user)
    {
        //UI 인원수 업데이트
        //UI 플레이어들 땡기기
        // 플레이어 닉네임 가져와서 UI업데이트

    }

    public void OnUpdateRole(User user, int role, bool isReady)
    {
        user.IsReady = isReady;
        _rolePanels[role].ActiveReadyPanel(user.IsReady);
        
        user.Role = (RoleType)role;

        if (user.IsReady)
        {
            _rolePanels[role].SetNameText(Storage.CurrentUser.Name);
            // 대충 인원수 UI++
            if ( CheckAllUserIsReady() && Storage.CurrentUser.IsMaster)
            {
                //시작 버튼 생김
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
        // 무기를 눌렀을 때
        // 무기를 바꿨을 때 무기가 겹칠경우 실행이 안되게
        WebSocket.Client.SetRole(role, isReady);
    }

    public void ClickStartGame(){
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
