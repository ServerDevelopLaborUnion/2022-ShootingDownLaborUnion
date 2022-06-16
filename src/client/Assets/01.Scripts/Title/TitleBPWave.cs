using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleBPWave : MonoBehaviour
{
    private TMP_Text _tmpText = null;

    private Mesh _textMesh;

    private Vector3[] _vertices;

    private void Start() {
        _tmpText = GetComponent<TMP_Text>();
        _tmpText.ForceMeshUpdate();
        _textMesh = _tmpText.mesh;
        _vertices = _textMesh.vertices;
        Debug.Log(_textMesh.vertices.Length);
    }

    private void Update() {
        _tmpText.ForceMeshUpdate();
        for (int i = 0; i < _vertices.Length; ++i){
            Debug.Log("잉");
            Vector3 offset = Wobble(Time.time + i);
            _vertices[i] += offset;
        }
        _textMesh.vertices = _vertices;
        _tmpText.canvasRenderer.SetMesh(_textMesh);
    }

    Vector2 Wobble(float time){
        return new Vector2(0, Mathf.Cos(time * 0.8f));
    }

}
