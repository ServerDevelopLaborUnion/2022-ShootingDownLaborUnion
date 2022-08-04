using static Define;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneLoader.Load(SceneType.Login);
        }
    }

}
