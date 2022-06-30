using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityToolbarExtender;

[InitializeOnLoad]
public class ToolbarLeftButton
{
    static ToolbarLeftButton()
    {
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
    }

    private static void OnToolbarGUI(IMGUIEvent evt)
    {
        Debug.Log($"OnToolbarGUI {evt.target}");
    }

    static void OnToolbarGUI()
    {
        GUIStyle labelStyle = EditorStyles.label;
        labelStyle.alignment = TextAnchor.MiddleLeft;
        labelStyle.fixedWidth = 0;

        GUIStyle boldLabelStyle = new GUIStyle(labelStyle);
        boldLabelStyle.fontStyle = FontStyle.Bold;

        Color color = GUI.color;
        GUILayout.Space(10);
        Color color2 = GUI.color;
        switch (WebSocket.Client.ConnectionState)
        {
        case ConnectionState.Connected:
            color2 = Color.green;
            break;
        case ConnectionState.Disconnected:
            color2 = Color.red;
            break;
        case ConnectionState.Connecting:
            color2 = Color.yellow;
            break;
        case ConnectionState.None:
            color2 = Color.gray;
            break;
        }
        GUI.color = color2;
        GUILayout.Label("●", labelStyle, GUILayout.Height(20), GUILayout.Width(15));
        GUI.color = color;
        string label = "";
        switch (WebSocket.Client.ConnectionState)
        {
        case ConnectionState.Connected:
            label = $"Connected : {WebSocket.Client.SessionID}";
            break;
        case ConnectionState.Disconnected:
            label = "Disconnected";
            break;
        case ConnectionState.Connecting:
            label = "Connecting";
            break;
        case ConnectionState.None:
            label = "Waiting for start";
            break;
        }
        GUILayout.Label(label, labelStyle, GUILayout.Height(20));
    }
}
