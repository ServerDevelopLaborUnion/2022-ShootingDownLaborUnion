using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnter : MonoBehaviour
{
    [SerializeField]
    private List<AnimationClip> _animationClips = new List<AnimationClip>();

    [SerializeField]
    private List<GameObject> _directingObjs = new List<GameObject>();

    private void Start() {
        
    }

    private IEnumerator EnterMovement(){
        FadeManager.Instance.ShowBar(true);

        for (int i = 0; i < _animationClips.Count; ++i)
        {
            _directingObjs[i].SetActive(true);
            yield return null;
        }

    }
}
