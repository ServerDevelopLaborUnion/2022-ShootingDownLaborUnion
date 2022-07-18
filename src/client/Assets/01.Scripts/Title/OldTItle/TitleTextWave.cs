using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TitleTextWave : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _tmpText = null;

    [SerializeField]
    private float _margin = 1f;
    [SerializeField]
    private float _duration = 1f;

    private void Start()
    {
         _tmpText.ForceMeshUpdate();
        TMP_TextInfo textInfo = _tmpText.textInfo;
        
        for (int i = 0; i < textInfo.characterCount; ++i){
            TMP_CharacterInfo characterInfo = textInfo.characterInfo[i];
            
            if(!characterInfo.isVisible){
                continue;
            }
            
            Vector3[] v = textInfo.meshInfo[characterInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; ++j){
                //TODO:대충 다트윈썌 를 사용하라는 소리
                Vector3 orig = v[characterInfo.vertexIndex + j];
                v[characterInfo.vertexIndex + j] = orig + Vector3.one * j * 400;
                Debug.Log(v[characterInfo.vertexIndex + j]);
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; ++i){
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            _tmpText.UpdateGeometry(meshInfo.mesh, i);
        }

        _tmpText.ForceMeshUpdate();
    }

    // private void OnDrawGizmos() {
    //     for (int i = 0; i < _tmpText.mesh.vertices.Length-1; ++i){
    //         Gizmos.DrawLine(new Vector2(_tmpText.mesh.vertices[i].x, _tmpText.mesh.vertices[i].y),
    //         new Vector2(_tmpText.mesh.vertices[i + 1].x, _tmpText.mesh.vertices[i + 1].y));
    //     }
    // }
}
