using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class TeleportController : MonoBehaviour
{
    [SerializeField] InputActionReference interactAction, pointerPosition, mouseClickAction;

    Vector2 _pointerInput;
    public int maxCharges = 10;

    private int currentCharges;
    public bool AttemptingTeleport { get; private set; }

    [SerializeField] private GameObject bluePorterPrefab, orangePorterPrefab;

    [SerializeField] private LineRenderer
        teleportLineRenderer;

    [SerializeField] private Transform teleportGunTransform, teleportGunTip;
    [SerializeField] private GameObject teleportEffectGO;

    [Space] [SerializeField] private float maxTeleportDistance = 5f;
    [SerializeField] private float clearanceRadius = 0.4f;
    [SerializeField] private LayerMask teleportableLayer;

    [SerializeField] private AudioClip teleportSFX;
    [SerializeField] private AudioClip teleportFailSFX;
    [SerializeField] private AudioClip teleportMachineSFX;

    [Space] [SerializeField] private float screenShakeIntensity = 0.5f;

    void OnEnable()
    {
        interactAction.action.Enable();
        pointerPosition.action.Enable();
        mouseClickAction.action.Enable();
    }
  
    void OnDisable()
    {
        interactAction.action.Disable();
        pointerPosition.action.Disable();
        mouseClickAction.action.Disable();
    }

    void Start()
    {
        Physics2D.queriesHitTriggers = true;
        interactAction.action.started += ctx =>
        {
            AttemptingTeleport = !AttemptingTeleport;
            if (AttemptingTeleport)
                AudioManager.Instance?.PlaySoundLooped(teleportMachineSFX);
            else
                AudioManager.Instance?.StopLoopedSound();
        };
        mouseClickAction.action.performed += ctx =>
        {
            if (AttemptingTeleport)
            {
                TryTeleport();
            }
        };
        pointerPosition.action.performed += ctx => _pointerInput = ctx.ReadValue<Vector2>();
        currentCharges = maxCharges;
        GameplayUI.Instance?.UpdateCharges(currentCharges, maxCharges);

        Physics2D.queriesHitTriggers = true;
    }

    private void Update()
    {
        if (GameManager.Instance?.GetState() == GameState.GameOver)
        {
            if (AttemptingTeleport)
            {
                AttemptingTeleport = false;
                AudioManager.Instance?.StopLoopedSound();
            }

            teleportLineRenderer.enabled = false;
            teleportEffectGO.SetActive(false);
        }
        else
        {
            AimAtMouse();
            LineRendererSetup();

            CalculateTruePosition();
        }
    }

    private Vector2 truePos;

    void CalculateTruePosition()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(_pointerInput);
        Vector2 playerPos = transform.position;

        // clamp teleport target to max distance from player
        Vector2 direction = (worldPos - playerPos).normalized;
        float distance = Vector2.Distance(playerPos, worldPos);
        float clampedDistance = Mathf.Min(distance, maxTeleportDistance);
        Vector2 targetPos = playerPos + direction * clampedDistance;
        truePos = targetPos;
    }

    private void TryTeleport()
    {
        AudioManager.Instance?.StopLoopedSound();

        if (currentCharges <= 0)
        {
            AudioManager.Instance?.PlaySound(teleportFailSFX);
            AttemptingTeleport = false;
            teleportLineRenderer.enabled = false;
            teleportEffectGO.SetActive(false);
            return;
        }

        Collider2D hit = Physics2D.OverlapCircle(truePos, clearanceRadius, teleportableLayer);

        if (hit != null)
        {
            transform.position = truePos;
            currentCharges--;
            GameplayUI.Instance?.UpdateCharges(currentCharges, maxCharges);
            AttemptingTeleport = false;

            AudioManager.Instance?.PlaySound(teleportSFX);
            EventBus.GameE.OnScreenShake?.Invoke(screenShakeIntensity);

            if (currentCharges <= 0)
                EventBus.GameE.TeleportExausted?.Invoke();
        }
        else
        {
            AudioManager.Instance?.PlaySound(teleportFailSFX);
            AttemptingTeleport = false;
        }
    }

    void LineRendererSetup()
    {
        if (AttemptingTeleport && teleportLineRenderer != null)
        {
            teleportLineRenderer.enabled = true;
            teleportLineRenderer.SetPosition(0, teleportGunTip.position);
            teleportLineRenderer.SetPosition(1, truePos);
            teleportEffectGO.SetActive(true);
            teleportEffectGO.transform.position = truePos;
        }
        else if (teleportLineRenderer != null)
        {
            teleportLineRenderer.enabled = false;
            teleportEffectGO.SetActive(false);
        }
    }

    void AimAtMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(_pointerInput);
        Vector3 aimDir = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        teleportGunTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 localScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            localScale.y = -1;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            localScale.y = 1;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        teleportGunTransform.localScale = localScale;
    }

    //delete later o!!!
    void OnDrawGizmos()
    {
        if (!AttemptingTeleport) return;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(_pointerInput);
        Vector2 playerPos = transform.position;
        Vector2 direction = (worldPos - playerPos).normalized;
        float distance = Mathf.Min(Vector2.Distance(playerPos, worldPos), maxTeleportDistance);
        Vector2 targetPos = playerPos + direction * distance;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(targetPos, clearanceRadius);
        Gizmos.DrawLine(playerPos, targetPos);
    }
}