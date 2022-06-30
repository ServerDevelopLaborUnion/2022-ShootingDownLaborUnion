using UnityEngine;

public class ToolbarControl
{
    public static void UpdateUI()
    {
        #if UNITY_EDITOR
        if (UnityToolbarExtender.ToolbarCallback.RootVisualElement != null)
        {
            UnityToolbarExtender.ToolbarCallback.RootVisualElement.MarkDirtyRepaint();
        }
        #endif
    }
}
