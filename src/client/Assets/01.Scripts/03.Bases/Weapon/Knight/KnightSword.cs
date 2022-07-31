using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class KnightSword : MonoBehaviour
{
    [SerializeField]
    private SwordInfo _swordInfo;

    [SerializeField]
    private Transform _swordLight;

    private void Start() {
        _swordLight.localPosition = _swordInfo.LightPos;
    }

}
