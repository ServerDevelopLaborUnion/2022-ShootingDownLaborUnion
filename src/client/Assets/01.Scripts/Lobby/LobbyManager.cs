using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lobby;

namespace Lobby
{
    public class LobbyManager : MonoBehaviour
    {
        [SerializeField]
        private LobbyCamera _lobbyCamera;
        [SerializeField]
        private MapController _mapController;

        void Start()
        {
            _lobbyCamera.Shake(3f, 0.1f);
            _mapController.Move(0, -2, 3f);
        }
    }
}