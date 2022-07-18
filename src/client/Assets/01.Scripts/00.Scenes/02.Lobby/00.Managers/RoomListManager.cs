using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListManager : MonoSingleton<RoomListManager>
{
    [Tooltip("방 목록을 표시할 위치")]
    [SerializeField]
    private Transform _roomList = null;

    // 방 노드 오브젝트
    private GameObject _roomNodePrefab = null;

    private void Awake()
    {
        // Resources에서 리스트 안에 표시할 오브젝트 불러오기
        _roomNodePrefab = Resources.Load<GameObject>("Prefabs/RoomContent");
    }

    /// <summary>
    /// 방 목록 새로고침에 사용되는 함수
    /// </summary>
    public void UpdateRoomList()
    {
        LobbyManager.Instance.GetRoomList();
    }

    /// <summary>
    /// 서버에서 방 정보를 가져와 클라이언트에서 초기화하는 함수
    /// </summary>
    /// <param name="roomList"></param>
    public void SetRoomList(List<RoomInfo> roomList)
    {
        foreach (Transform child in _roomList)
        {
            Destroy(child.gameObject);
        }

        foreach (var room in roomList)
        {
            var roomNode = Instantiate(_roomNodePrefab, _roomList);
            // TODO : room.MaxPlayers 불러와서 매개변수 4 들어가 있는자리 대체하기
            roomNode.GetComponent<RoomNode>().SetInfo(room.Name, 4, room.PlayerCount, room.IsPrivate);
        }
    }
}