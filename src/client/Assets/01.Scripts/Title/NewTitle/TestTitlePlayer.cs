using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TestTitlePlayer : MonoSingleton<TestTitlePlayer>
{
    [SerializeField]
    private float _moveSpeed = 8f;

    private Rigidbody2D _rb;

    private void Start() {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update() {
        Move();
    }

    private void Move(){
        float hori = Input.GetAxisRaw("Horizontal");

        float verti = Input.GetAxisRaw("Vertical");

        _rb.velocity = new Vector2(hori, verti).normalized * _moveSpeed;


    }
}
