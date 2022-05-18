using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NetworkObject))]
public class NetworkObjectEditor : Editor
{
    static class CustomStyle
    {
        static bool initialized = false;

        public static GUIStyle LeftAlignLabel { get { Initialize(); return _leftAlignLabel; } }
        public static GUIStyle RightAlignLabel { get { Initialize(); return _rightAlignLabel; } }
        public static GUIStyle CenterAlign { get { Initialize(); return _centerAlign; } }

        static GUIStyle _leftAlignLabel;
        static GUIStyle _rightAlignLabel;
        static GUIStyle _centerAlign;

        public static void Initialize()
        {
            if (initialized)
                return;

            _leftAlignLabel = new GUIStyle(EditorStyles.label);
            _leftAlignLabel.alignment = TextAnchor.MiddleLeft;
            _rightAlignLabel = new GUIStyle(EditorStyles.label);
            _rightAlignLabel.alignment = TextAnchor.MiddleRight;
            _centerAlign = new GUIStyle();
            _centerAlign.alignment = TextAnchor.MiddleCenter;
        }
    }
    bool initialized = false;

    NetworkObject networkObject;

    private void Initialize()
    {
        if (initialized)
            return;

        initialized = true;
        networkObject = target as NetworkObject;
    }

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        Initialize();

        GUILayout.Label("Network Object", new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter, fontSize = 18 });
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Object ID: ", new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleRight });
        GUILayout.TextField(networkObject.ObjectID, new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold });
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Owner ID: ", CustomStyle.RightAlignLabel);
        GUILayout.TextField(networkObject.OwnerID, new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold });
        GUILayout.EndHorizontal();
    }
}