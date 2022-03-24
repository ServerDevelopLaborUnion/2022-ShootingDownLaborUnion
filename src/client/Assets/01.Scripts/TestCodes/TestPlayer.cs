using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (mpos - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            rb.velocity = Vector2.zero;
        }
    }
}
