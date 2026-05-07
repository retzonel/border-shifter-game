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

    [SerializeField] private AudioClip walkSFX;
    [SerializeField] private float walkSFXInterval = 0.35f;
    private float _walkSFXTimer;

    void Start()
    {
        moveAction.action.Enable();
        restartAction.action.Enable();
        restartAction.action.started += ctx => { RequestRestart(ctx); };
    }

    void OnDestroy()
    {
        moveAction.action.Disable();
        restartAction.action.Disable();
        restartAction.action.started -= RequestRestart;
    }

    private void RequestRestart(InputAction.CallbackContext ctx)
    {
        EventBus.GameE.OnRestartRequested?.Invoke();
    }

    private void Update()
    {
        _moveInput = teleportController.AttemptingTeleport ? Vector2.zero : moveAction.action.ReadValue<Vector2>();

        HandleWalkSFX();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance?.GetState() != GameManager.GameState.GameOver)
        {
            rb.linearVelocity = _moveInput.normalized * moveSpeed;
        }
        else
        {
            _moveInput = Vector2.zero;
            rb.linearVelocity = _moveInput;
        }
        //if teleportation process is active, ignore movement input and set velocity to zero
    }


    private void HandleWalkSFX()
    {
        if (_moveInput.magnitude > 0.1f)
        {
            _walkSFXTimer -= Time.deltaTime;
            if (_walkSFXTimer <= 0f)
            {
                AudioManager.Instance?.PlaySound(walkSFX);
                _walkSFXTimer = walkSFXInterval;
            }
        }
        else
        {
            // reset so SFX plays immediately on next movement
            _walkSFXTimer = 0f;
        }
    }
}