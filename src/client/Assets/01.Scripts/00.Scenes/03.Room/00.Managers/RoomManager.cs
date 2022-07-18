using UnityEngine;

public class RoomManager : MonoSingleton<RoomManager>
{
    private void Start()
    {
        // UI 에 CurrentRoom 정보 업데이트
        
    }

    public void OnUserJoin(User user)
    {
        //  UI 인원수 업데이트
        // 플레이어 닉네임 가져와서 UI업데이트

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
        }
        else{
            //TODO: 대충 인원수 UI--
        }

    }

    public void OnChangeWeapon(User user){
        // TODO: 대충 직업 UI 바꾸기
    }

    public void SetReady(bool isReady)
    {
        //레디를 눌렀을 때
        WebSocket.Client.SetReady(isReady);
    }

    public void SetWeapon(int weapon)
    {
        //무기를 눌렀을 때
        // 무기를 바꿨을 때 무기가 겹칠경우 실행이 안되게
        WebSocket.Client.SetWeapon(weapon);
    }

    public void OnStartGame(){
        // 게임 시작 버튼을 눌렀을 때
        // 인원수가 맞지 않거나, 겹치는 무기가 있을 경우 불가 메세지 출력
        // 방장이 아니면 못누름
        if(IsOverlapJob()){
            // TODO: 대충 직업이 겹쳐서 실행 못한다는 UI
            return;
        }
        if(Storage.CurrentRoom.Players.Count < Storage.CurrentRoom.Info.PlayerCount){
            // TODO: 대충 인원수가 부족하다는 UI
        }
        
    }

    private bool IsOverlapJob(){
        for (int i = 0; i < Storage.CurrentRoom.Players.Count; ++i){
            for (int j = 0; j < Storage.CurrentRoom.Players.Count; ++j){
                if (Storage.CurrentRoom.Players[i].Job == Storage.CurrentRoom.Players[j].Job){
                    return true;
                }
            }
        }
        return false;
    }
}
