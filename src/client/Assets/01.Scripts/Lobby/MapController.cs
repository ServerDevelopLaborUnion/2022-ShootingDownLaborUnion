using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lobby;
using DG.Tweening;

namespace Lobby
{
    public class MapController : MonoBehaviour
    {
        public void Move(float x, float y, float duration)
        {
            transform.DOMove(new Vector3(x, y, 0), duration);
        }
    }
}