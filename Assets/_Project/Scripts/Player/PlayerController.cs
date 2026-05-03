using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputActionReference moveAction, pointerPosition;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed = 5f;

    [Space] [SerializeField] private TeleportController teleportController;

    Vector2 _moveInput, _pointerInput;

    void OnEnable()
    {
        moveAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
    }

    private void Update()
    {
        _moveInput = teleportController.AttemptingTeleport ? Vector2.zero : moveAction.action.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = _moveInput.normalized * moveSpeed;
        //if teleportation process is active, ignore movement input and set velocity to zero
    }
}