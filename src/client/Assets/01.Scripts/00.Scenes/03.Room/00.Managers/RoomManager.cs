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

    public void OnStateUpdated(User user)
    {
        // 무기, 레디 등 스테이트를 업데이트 할 떄 사용
    }

    public void SetReady(bool isReady)
    {
        //레디를 눌렀을 때
    }

    public void SetWeapon(int weapon)
    {
        //무기를 눌렀을 때
        // 무기를 바꿨을 때 무기가 겹칠경우 실행이 안되게
    }

    public void OnStartGame(){
        // 게임 시작 버튼을 눌렀을 때
        // 인원수가 맞지 않거나, 겹치는 무기가 있을 경우 불가 메세지 출력
        // 방장이 아니면 못누름

    }
}
