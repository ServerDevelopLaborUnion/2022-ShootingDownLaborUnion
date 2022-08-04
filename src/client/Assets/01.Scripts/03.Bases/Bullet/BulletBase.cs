using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField]
    private float _speed = 20f;
    [SerializeField]
    private Vector2 _dir = Vector2.up;

    public float _range = 3;
    private float _movedDistance = 0;
    [SerializeField]
    private Vector2 _summonedPos;

    protected Transform _transform = null;

    protected virtual void Start() 
    {
        _transform = transform;
    }

    public void InitBullet(Vector2 summonedPos, float range)
    {
        _summonedPos = summonedPos;
        _range = range;
        gameObject.SetActive(true);
    }

    protected virtual void Update() 
    {
        Move();
        Limit();
    }

    protected virtual void Move()
    {
        _transform.Translate(_dir * _speed * Time.deltaTime);
    }

    protected virtual void Limit()
    {

        _movedDistance = Define.GetDistance(_summonedPos, transform.position);
        if(_movedDistance >= _range)
            Despawn();
    }

    protected virtual void Despawn()
    {
        //TODO: ?留?

        gameObject.SetActive(false);
    }
}
