using UnityEngine;

public class ResourceCarrier : MonoBehaviour
{
    public ResourceData CarriedResource { get; private set; } = null;


    void OnTriggerEnter2D(Collider2D other)
    {
        //pickup
        if (other.CompareTag("Resource") && CarriedResource == null)
        {
            ResourceNode node = other.GetComponent<ResourceNode>();
            if (node != null && !node.isPickedUp)
            {
                CarriedResource = node.data;
                node.OnPickUp();
                Debug.Log("Picked up: " + CarriedResource.resourceName);
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
            }
        }


        bool CompareResource(ResourceData resource)
        {
            return CarriedResource.resourceType == resource.resourceType;
        }
    }
}