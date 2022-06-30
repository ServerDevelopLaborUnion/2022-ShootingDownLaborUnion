using UnityEngine;
using UnityEditor;
using System;

[InitializeOnLoad]
public static class ClientKiller
{
    static ClientKiller()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            WebSocket.Client.Disconnect();
        }
    }
}
