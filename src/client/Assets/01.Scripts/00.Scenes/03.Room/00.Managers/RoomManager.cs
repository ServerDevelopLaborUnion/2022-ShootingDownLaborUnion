using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class RoomManager : MonoSingleton<RoomManager>
{
    [SerializeField]
    private List<RolePanel> _rolePanels = new List<RolePanel>();


    private void Start()
    {
        // UI 에 CurrentRoom 정보 업데이트
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


        if (user.IsReady)
        {
            _rolePanels[role].SetNameText(Storage.CurrentUser.Name);
            //TODO: 대충 인원수 UI++
            if ( CheckAllUserIsReady() && Storage.CurrentUser.IsMaster)
            {
                // TODO: 시작 버튼 생김
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
        //WebSocket.Client.SetRole(role);
    }

    public void ClickStartGame(){
        //TODO: 씬 로딩
        //WebSocket.Client.ClickStartGame;
    }

    public void OnStartGame()
    {
        // 게임 시작 버튼을 눌렀을 때
        // 인원수가 맞지 않거나, 겹치는 무기가 있을 경우 불가 메세지 출력
        // 방장이 아니면 못누름
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
