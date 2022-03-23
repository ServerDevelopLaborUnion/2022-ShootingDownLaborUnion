using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterBase : MonoBehaviour
{
    [SerializeField] private UnityEvent<Vector2> OnKeyInput;
    void Start()
    {
        
    }

    void Update()
    {
        OnKeyInput?.Invoke((new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))).normalized);
    }
}
