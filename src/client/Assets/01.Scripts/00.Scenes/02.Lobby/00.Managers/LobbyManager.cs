using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoSingleton<LobbyManager>
{
    private List<RoomInfo> _roomList => WebSocket.Client.RoomList;

    /// <summary>
    /// 방을 생성할 때 사용되는 함수입니다.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="password"></param>
    public void CreateRoom(string name, string password = "")
    {
        WebSocket.Client.CreateRoom(name, password);
    }

    /// <summary>
    /// 방에 입장할 때 사용되는 함수입니다.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="password"></param>
    public void JoinRoom(string name, string password = "")
    {
        WebSocket.Client.JoinRoom(name, password);
    }

    /// <summary>
    /// 지금 존재하는 방들의 정보를 요청할 때 사용되는 함수입니다.
    /// </summary>
    /// <returns></returns>
    public void GetRoomList()
    {
        // TODO : WebSocket.Client.GetRoomList()에서 값을 받았(을때 OnRoomListUpdate를 호출하도록 하기
        OnRoomListUpdated(WebSocket.Client.GetRoomList().ToList());
    }

    /// <summary>
    /// 방에서 나갈 때 사용되는 함수입니다.
    /// </summary>
    public void LeaveRoom()
    {
        WebSocket.Client.LeaveRoom();
    }

    /// <summary>
    /// 서버에서 방 리스트가 업데이트 되면 실행되는 함수입니다.
    /// ex) 추가, 삭제
    /// </summary>
    /// <param name="roomList"></param>
    public void OnRoomListUpdated(List<RoomInfo> roomList)
    {
        // UI에 방 정보를 업데이트
        RoomListManager.Instance.SetRoomList(roomList);
    }

    /// <summary>
    /// 방에 입장했을 때 실행되는 함수입니다.
    /// </summary>
    /// <param name="room"></param>
    public void OnJoinedRoom(Room room)
    {
        Storage.CurrentRoom = room;
        SceneLoader.Load(SceneType.Room);
    }
}
