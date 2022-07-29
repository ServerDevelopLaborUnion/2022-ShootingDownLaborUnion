using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordInfo", menuName = "ScriptableObject/SwordInfo", order = 0)]
public class SwordInfo : ScriptableObject {
    [SerializeField]
    private Vector3 _lightPos;

    public Vector3 LightPos => _lightPos;
}