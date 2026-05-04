using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private GameObject currentBluePortal;
    private GameObject currentOrangePortal;

    [Space]
    [SerializeField] private float maxTeleportDistance = 5f;
    [SerializeField] private float clearanceRadius = 0.4f;
    [SerializeField] private LayerMask obstacleLayer;
    
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
            if (!AttemptingTeleport)
            {
                if (currentBluePortal != null)
                    Destroy(currentBluePortal);
            }
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

    }

    private void Update()
    {
        AimAtMouse();
        LineRendererSetup();
        
        CalculateTruePosition();
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
        if (currentCharges <= 0)
        {
            Debug.Log("No charges left!");
            AttemptingTeleport = false;
            return;
        }
        
        Vector2 targetPos = truePos;

        // circle cast to check for obstacles at target position
        Collider2D hit = Physics2D.OverlapCircle(targetPos, clearanceRadius, obstacleLayer);

        if (hit != null)
        {
            Debug.Log("Teleport blocked by: " + hit.name);
            AttemptingTeleport = false;
            return;
        }

        // clear area, perform teleport
        transform.position = targetPos;
        currentCharges--;
        GameplayUI.Instance?.UpdateCharges(currentCharges, maxCharges);
        AttemptingTeleport = false;

        Debug.Log("Teleported! Remaining charges: " + currentCharges);

        if (currentCharges <= 0)
            GameManager.Instance?.OnTeleportsExhausted();
    }

    void LineRendererSetup()
    {
        if (AttemptingTeleport && teleportLineRenderer != null)
        {
            teleportLineRenderer.enabled = true;
            teleportLineRenderer.SetPosition(0, teleportGunTip.position);
            teleportLineRenderer.SetPosition(1, truePos);
        }
        else if (teleportLineRenderer != null)
        {
            teleportLineRenderer.enabled = false;
        }
    }


    private void PlacePortal(GameObject portalPrefab, Vector2 position, ref GameObject currentPortal)
    {
        if (currentPortal != null)
            Destroy(currentPortal);

        if (portalPrefab != null)
            currentPortal = Instantiate(portalPrefab, position, Quaternion.identity);
    }
    
    void AimAtMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(_pointerInput);
        Vector3 aimDir = (mousePos - transform.position).normalized;

        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        teleportGunTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 localScale = Vector3.one;
        if(angle > 90 || angle < -90)
        {
            localScale.y = -1;
            transform.eulerAngles = new Vector3(0, 180, 0);
        } else
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