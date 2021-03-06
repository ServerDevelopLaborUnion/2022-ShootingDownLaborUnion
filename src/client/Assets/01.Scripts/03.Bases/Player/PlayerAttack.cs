using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttack : CharacterAttack
{
    [SerializeField]
    protected float _range = 0;

    public float Range
    {
        get { return _range; }
    }

    public Action OnGetRangeBtnClick = null;


    private GameObject _rangeObject = null;

    protected CharacterMove _move;
    protected CharacterRenderer _renderer;
    protected Entity closestEnemy = null;
    private Vector2 _targetPos = Vector2.zero;
    protected override void Start()
    {
        base.Start();

        _range = _base.Stat.AtkRange;
        if (_base.Data.Type == EntityType.Player)
        {
            _rangeObject = transform.parent.Find("Range").gameObject;
            _rangeObject.GetComponent<RangeRenderer>().SetRange(_range);
            _rangeObject.SetActive(false);
        }
        OnGetRangeBtnClick += ToggleRange;
        _move = transform.parent.GetComponent<CharacterMove>();
        _renderer = GetComponent<CharacterRenderer>();
    }

    public void ToggleRange()
    {
        _rangeObject.SetActive(!_rangeObject.activeInHierarchy);
    }

    protected override void Attack()
    {
        List<Entity> enemies = NetworkManager.Instance.entityList.FindAll((entity) => entity.Data.Type == EntityType.Enemy);
        if (enemies.Count != 0)
        {
            closestEnemy = enemies.OrderBy((enemy) => GetDistance(Define.MainCam.ScreenToWorldPoint(Input.mousePosition), enemy.transform.position)).FirstOrDefault();
        }
        Vector2 dir = Define.MainCam.ScreenToWorldPoint(Input.mousePosition);
        if (closestEnemy != null)
        {

            if (GetDistance(transform.parent.position, closestEnemy.transform.position) <= _range)
            {
                dir = closestEnemy.transform.position;

                OnAttacked?.Invoke();
                Debug.Log($"{closestEnemy.name}, {GetDistance(transform.parent.position, closestEnemy.transform.position)}");
                closestEnemy.GetComponent<CharacterDamage>().GetDamaged(_base.Stat.AD, _playerCol);

                base.Attack();
            }

            else
            {
                _targetPos = Define.MousePos;
                _move.MoveAgent(Define.MousePos + Vector2.up);
                _renderer.FlipCharacter(dir);
                WebSocket.Client.ApplyEntityMove(_base);
            }
        }
        else
        {
            _targetPos = Define.MousePos;
            _move.MoveAgent(Define.MousePos + Vector2.up);

            WebSocket.Client.ApplyEntityMove(_base);
        }

        _renderer.FlipCharacter(dir);
    }

    private void Update()
    {
        if(closestEnemy != null)
        {
            if(GetDistance(transform.parent.position, closestEnemy.transform.position) <= _range)
            {
                DoAttack();
                closestEnemy = null;
            }
        }

        if(GetDistance(_targetPos, transform.parent.position) <= 0.01f)
        {
            closestEnemy = null;
        }
    }

    public float GetDistance(Vector2 pos1, Vector2 pos2)
    {
        float x = Mathf.Pow(pos1.x - pos2.x, 2);
        float y = Mathf.Pow(pos1.y * 2 - pos2.y * 2, 2);
        return Mathf.Sqrt(x + y);
    }

    public override void EndAttack()
    {
        base.EndAttack();
    }
}
