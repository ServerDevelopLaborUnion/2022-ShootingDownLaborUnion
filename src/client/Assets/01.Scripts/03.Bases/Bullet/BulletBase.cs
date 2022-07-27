using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField]
    private float _speed = 20f;
    [SerializeField]
    private Vector2 _dir = Vector2.up;

    protected Transform _transform = null;

    protected virtual void Start() {
        _transform = transform;
    }

    protected virtual void Update() {
        Move();
    }

    protected virtual void Move(){
        _transform.Translate(_dir * _speed * Time.deltaTime);
    }

    protected virtual void Limit(){
        //TODO: 오브젝트 한계구역 설정해주기
    }

    protected virtual void Despawn(){
        //TODO: 풀링
    }

    
}
