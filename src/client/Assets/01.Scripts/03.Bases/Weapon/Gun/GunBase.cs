using static Yields;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : CharacterAttack
{

    public enum GunType
    {
        NONE,
        SINGLESHOT,
        REPEATER,
        LENGTH,
    }

    [SerializeField]
    private GameObject _bullet = null;

    [SerializeField]
    private Transform _shootPos = null;

    [SerializeField]
    protected GunType _gunType = GunType.NONE;

    [SerializeField]
    private float _attackDelay = 0.1f;


    private CharacterBase _characterBase = null;

    protected Transform _playerPos = null;

    private GunCharacterInput _gunCharacterInput = null;

    public float _attackTimer = 0f;

    private bool _isCoolDown = false;

    protected override void Start()
    {
        base.Start();
        _playerPos = _transform;
        while (true)
        {
            if (_playerPos.parent == null) break;
            _playerPos = _playerPos.parent;
        }
        _gunCharacterInput = _playerPos.GetComponent<GunCharacterInput>();
        _characterBase = GetComponentInParent<CharacterBase>();
        SetShootType();
    }


    [ContextMenu("TypeChange")]
    private void SetShootType()
    {
        switch (_gunType)
        {
            case GunType.SINGLESHOT:
                _gunCharacterInput.GetOnAttackKeyInput.AddListener(DoAttack);
                break;
            case GunType.REPEATER:
                _gunCharacterInput.GetOnMouseKeyEvent.AddListener(DoAttack);
                break;
            default:
                Debug.LogError("Type Undefined");
                break;
        }
    }

    protected override void Attack()
    {
        base.Attack();
        SpawnBullet();
        StartCoroutine(EndToAttack());
    }

    private IEnumerator EndToAttack()
    {
        yield return WaitForSeconds(_attackDelay);
        _base.State.CurrentState &= ~CharacterState.State.Attack;
    }


    protected void SpawnBullet()
    {
        GameObject bullet = Instantiate(_bullet, _shootPos);
        bullet.transform.SetParent(null);
        bullet.SetActive(true);
    }

}
