using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputActionReference moveAction, restartAction;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveSpeed = 5f;

    [Space] [SerializeField] private TeleportController teleportController;

    Vector2 _moveInput, _pointerInput;

    void OnEnable()
    {
        moveAction.action.Enable();
        restartAction.action.Enable();
        restartAction.action.performed += RequestRestart;
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        restartAction.action.Disable();
        restartAction.action.performed -= RequestRestart;
    }

    private void RequestRestart(InputAction.CallbackContext ctx)
    {
        EventBus.GameE.OnRestartRequested?.Invoke();
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