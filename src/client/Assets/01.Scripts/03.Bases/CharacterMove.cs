using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterMove : MonoBehaviour
{
    public float _moveSpeed = 0f;
    public void MoveCharacter(Vector2 dir)
    {
        transform.position += (Vector3)dir * _moveSpeed * Time.deltaTime;
    }
}
