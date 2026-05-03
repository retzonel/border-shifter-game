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
        teleportLineRenderer; //for later visual feedback of teleportation path from pplayer{blue portal) to mouse position(orange portal)

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
            //enter/toggle teleportation process
            AttemptingTeleport = !AttemptingTeleport;
            PlacePortal(bluePorterPrefab, transform.position, ref currentBluePortal);
            if (!AttemptingTeleport)
            {
                //if teleportation process is canceled, remove the blue portal
                if (currentBluePortal != null)
                    Destroy(currentBluePortal);
            }
        };
        mouseClickAction.action.performed += ctx =>
        {
            //when mouse click is performed
            //check if teleportation process is active and mouse pointer is in a tel-portable area
            //and if so, attempt to teleport
            if (AttemptingTeleport)
            {
                TryTeleport();
            }
        };
        pointerPosition.action.performed += ctx => _pointerInput = ctx.ReadValue<Vector2>();
        currentCharges = maxCharges;
    }

    private void Update()
    {
        LineRendererSetup();
    }

    private void TryTeleport()
    {
        if (currentCharges <= 0)
        {
            Debug.Log("No charges left!");
            AttemptingTeleport = false;
            return;
        }

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(_pointerInput);
        
        
        PlacePortal(orangePorterPrefab, worldPos, ref currentOrangePortal);
        
        transform.position = worldPos;
        currentCharges--;
        AttemptingTeleport = false;

        Debug.Log("Teleported! Remaining charges: " + currentCharges);

        if (currentCharges <= 0)
            GameManager.Instance?.OnTeleportsExhausted();
    }

    void LineRendererSetup()
    {
        if (AttemptingTeleport && teleportLineRenderer != null)
        {
            // Show line from player to mouse position
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(_pointerInput);
            teleportLineRenderer.enabled = true;
            teleportLineRenderer.SetPosition(0, transform.position);
            teleportLineRenderer.SetPosition(1, worldPos);
        }
        else if (teleportLineRenderer != null)
        {
            teleportLineRenderer.enabled = false;
        }
    }
    
    private GameObject currentBluePortal;
    private GameObject currentOrangePortal;
    private void PlacePortal(GameObject portalPrefab, Vector2 position, ref GameObject currentPortal)
    {
        if (currentPortal != null)
            Destroy(currentPortal);
    
        if (portalPrefab != null)
            currentPortal = Instantiate(portalPrefab, position, Quaternion.identity);
    }
}