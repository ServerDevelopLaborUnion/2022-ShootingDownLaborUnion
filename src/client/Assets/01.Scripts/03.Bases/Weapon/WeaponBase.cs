using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{

    protected Transform _transform = null;


    protected bool isFacingRight = false;

    private void Start()
    {
        _transform = transform;
    }
    protected virtual void Update()
    {
        CheckFacing();
    }

    public void SetDirection(Vector2 mousePos)
    {
        Vector2 dir = mousePos;
        dir.Normalize();
        float rotationZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Vector3 angle = _transform.eulerAngles;
        angle.z = rotationZ+(isFacingRight ? 0f : 180f);
        _transform.rotation = Quaternion.Euler(angle );

    }


    private void CheckFacing()
    {
        if(_transform.parent.localScale.x > 0){
            isFacingRight = true;
        }
        else{
            isFacingRight = false;
        }
    }
}
