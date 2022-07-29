using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnTargetAttack : PlayerAttack
{
    [SerializeField]
    private Transform _firePos = null;
    [SerializeField] private GameObject _bulletPrefab = null;
    void Update()
    {
        
    }

    public override void DoAttack()
    {
        _renderer.FlipCharacter(Define.MousePos);
        OnAttacked?.Invoke();
        GameObject bullet = Instantiate(_bulletPrefab, _firePos.position, Quaternion.identity);
        Vector2 dir = (Vector2)bullet.transform.position - Define.MousePos;
        float z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.transform.eulerAngles = new Vector3(0, 0, z + 90);
        bullet.GetComponent<BulletBase>().InitBullet(transform.position - Vector3.up, _range);
        bullet.GetComponent<BulletAttack>()._damage = _base.Stat.AD;
    }
}
