using UnityEngine;
using TMPro;

public class RoomManager : MonoSingleton<RoomManager>
{
    [SerializeField]
    private TMP_Text _userCountText;

    [SerializeField]
    private TMP_Text[] _userNameTexts;

    private void Start()
    {
        // UI 에 CurrentRoom 정보 업데이트
        for (int i = 0; i < Storage.CurrentRoom.Users.Count; ++i){
            _userNameTexts[i].text = Storage.CurrentRoom.Users[i].Name;
        }
        
    }

    public void OnUserJoin(User user)
    {
        //  UI 인원수 업데이트
        // 플레이어 닉네임 가져와서 UI업데이트
        _userNameTexts[Storage.CurrentRoom.Users.Count].text = user.Name;
        _userCountText.text = $"{Storage.CurrentRoom.Users} / {RoomInfo.MaxPlayers}";
    }

    public void OnUserLeave(User user)
    {
        //UI 인원수 업데이트
        //UI 플레이어들 땡기기
        // 플레이어 닉네임 가져와서 UI업데이트

    }

    public void OnReadyStateUpdate(User user){
        if(user.IsReady){
            //TODO: 대충 인원수 UI++
            if(Storage.CurrentRoom.Users.Count == RoomInfo.MaxPlayers && user.IsMaster){
                // TODO: 시작 버튼 생김
            }
        }
        else{
            //TODO: 대충 인원수 UI--
        }

    }

    public void OnChangeRole(User user){
        // TODO: 대충 직업 UI 바꾸기
    }

    public void SetReady(bool isReady)
    {
        //레디를 눌렀을 때
        WebSocket.Client.SetReady(isReady);
    }

    public void SetRole(int role)
    {
        //무기를 눌렀을 때
        // 무기를 바꿨을 때 무기가 겹칠경우 실행이 안되게
        WebSocket.Client.SetRole(role);
    }

    public void OnStartGame(){
        // 게임 시작 버튼을 눌렀을 때
        // 인원수가 맞지 않거나, 겹치는 무기가 있을 경우 불가 메세지 출력
        // 방장이 아니면 못누름
        if(IsOverlapJob()){
            // TODO: 대충 직업이 겹쳐서 실행 못한다는 UI
            return;
        }
        if(Storage.CurrentRoom.Users.Count < RoomInfo.MaxPlayers){
            // TODO: 대충 인원수가 부족하다는 UI
            return;
        }

        
    }

    private bool IsOverlapJob(){
        for (int i = 0; i < Storage.CurrentRoom.Users.Count; ++i){
            for (int j = 0; j < Storage.CurrentRoom.Users.Count; ++j){
                if(i==j)continue;
                if (Storage.CurrentRoom.Users[i].Role == Storage.CurrentRoom.Users[j].Role){
                    return true;
                }
            }
        }
        return false;
    }
}
