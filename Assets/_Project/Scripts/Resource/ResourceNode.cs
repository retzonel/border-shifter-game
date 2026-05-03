using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public ResourceData data;
    public bool isPickedUp = false;

    public void OnPickUp()
    {
        isPickedUp = true;
        gameObject.SetActive(false);
    }
}
