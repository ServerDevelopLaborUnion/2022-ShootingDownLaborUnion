using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnTargetAttack : PlayerAttack
{
    [SerializeField]
    private Transform _firePos = null;
    [SerializeField] private GameObject _bulletPrefab = null;
    private BulletAttack _bullet = null;
    private bool _isSkill = false;
    protected override void Start()
    {
        base.Start();    
    }
    void Update()
    {

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
                _renderer.FlipCharacter(dir);
                GameObject bullet = Instantiate(_bulletPrefab, _firePos.position, Quaternion.identity);
                if (_isSkill)
                {
                    _bullet = bullet.GetComponent<BulletAttack>();
                    _bullet._col = _playerCol;
                }
                Vector2 target = bullet.transform.position - closestEnemy.transform.position;
                float z = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
                bullet.transform.eulerAngles = new Vector3(0, 0, z + 90);
                bullet.GetComponent<BulletBase>().InitBullet(transform.position - Vector3.up, _range);
                bullet.GetComponent<BulletAttack>()._damage = _base.Stat.AD;

                base.SetAttack();
            }

            else
            {
                _move.MoveAgent(Define.MainCam.ScreenToWorldPoint(Input.mousePosition + Vector3.up));
                _renderer.FlipCharacter(dir);
                WebSocket.Client.ApplyEntityMove(_base);
            }
        }
        else
        {
            _move.MoveAgent(Define.MainCam.ScreenToWorldPoint(Input.mousePosition + Vector3.up));

            WebSocket.Client.ApplyEntityMove(_base);
        }
    }

    public void ActiveSkill()
    {
        _isSkill = true;
    }

    public void DeActiveSkill()
    {
        _isSkill = false;
    }
}
