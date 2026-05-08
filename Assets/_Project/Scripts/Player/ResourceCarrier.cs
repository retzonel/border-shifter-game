using UnityEngine;

public class ResourceCarrier : MonoBehaviour
{
    public ResourceData CarriedResource { get; private set; } = null;

    [SerializeField] AudioClip pickupSFX, deliverySFX, alreadyCarryingSFX;
    [SerializeField] private GameObject deliveryVFXPrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        //pickup
        if (other.CompareTag("Resource"))
        {
            if (CarriedResource != null)
            {
                Debug.LogWarning("Already carrying a resource! Can't pick up another.");
                AudioManager.Instance?.PlaySound(alreadyCarryingSFX);
                return;
            }

            ResourceNode node = other.GetComponent<ResourceNode>();
            if (node != null && !node.isPickedUp)
            {
                CarriedResource = node.data;
                node.OnPickUp();
                Debug.Log("Picked up: " + CarriedResource.resourceName);
                AudioManager.Instance?.PlaySound(pickupSFX);
                EventBus.GameE.OnScreenShake?.Invoke(0.2f);
                GameplayUI.Instance?.UpdateCarriedResource(CarriedResource);
            }
        }

        // deliver
        if (other.CompareTag("DeliveryZone") && CarriedResource != null)
        {
            DeliveryZone zone = other.GetComponent<DeliveryZone>();
            if (zone != null && zone.Accepts(CarriedResource))
            {
                zone.Deliver(CarriedResource);
                CarriedResource = null;
                Debug.Log("Delivered!");
                AudioManager.Instance?.PlaySound(deliverySFX);
                EventBus.GameE.PlayVFX?.Invoke(deliveryVFXPrefab, transform.position);
                GameplayUI.Instance?.UpdateCarriedResource(CarriedResource);
            }
        }
    }

    bool CompareResource(ResourceData resource)
    {
        return CarriedResource.resourceType == resource.resourceType;
    }
}