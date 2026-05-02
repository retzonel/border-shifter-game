using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputActionReference moveAction, pointerPosition;
    
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed = 5f;
    
    Vector2 _moveInput, _pointerInput;
    
    void Start()
    {
        moveAction.action.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        moveAction.action.canceled += ctx => _moveInput = Vector2.zero;

        pointerPosition.action.performed += ctx => _pointerInput = ctx.ReadValue<Vector2>();
    }
    
    void FixedUpdate()
    {
        rb.linearVelocity = _moveInput.normalized * moveSpeed;
    }
}
