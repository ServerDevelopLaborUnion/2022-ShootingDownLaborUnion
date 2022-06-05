using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    [SerializeField]
    private Vector2 _offsetSpeed;

    private MeshRenderer _meshRenderer = null;

    private Vector2 _offset = Vector2.zero;

    private void Start() {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Update() {
        SetOffset();
    }
    private void SetOffset(){
        _offset += _offsetSpeed * Time.deltaTime;
        _meshRenderer.material.SetTextureOffset("_MainTex",_offset);
    }
}
