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
        GUI.color = Color.green;
        GUILayout.Label("●", labelStyle, GUILayout.Height(20), GUILayout.Width(15));
        GUI.color = color;
        GUILayout.Label("Connected: ", labelStyle, GUILayout.Height(20), GUILayout.Width(70));
        GUILayout.Label(WebSocket.Client.SessionID, boldLabelStyle, GUILayout.Height(20), GUILayout.Width(300));
        GUILayout.Button("Disconnect", EditorStyles.toolbarButton);
    }
}
