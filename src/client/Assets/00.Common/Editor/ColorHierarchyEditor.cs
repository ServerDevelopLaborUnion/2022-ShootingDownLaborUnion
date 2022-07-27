using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ColorHierarchyEditor : MonoBehaviour
{
    [MenuItem("GameObject/ColorHierarchyasdf %H")]
    private static void CreateColorHierarchy()
    {
        Debug.Log("컬러 하이어라키 들어감");
        GameObject[] obj = Selection.gameObjects;


        for (int i = 0; i < obj.Length; i++)
        {
            Undo.AddComponent<ColorHierarchy>(obj[i]);
        }
    }
}