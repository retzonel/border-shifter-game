using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    public ResourceType acceptedType;
    private bool delivered = false;
    
    public bool Accepts(ResourceData resource)
    {
        return !delivered && resource.resourceType == acceptedType;
    }

    public void Deliver(ResourceData resource)
    {
        delivered = true;
        GameManager.Instance?.OnResourceDelivered(resource);
        //the resource has been delivered
    }
}
