using UnityEngine;

public class Border : MonoBehaviour
{
    [SerializeField] Collider2D[] colliders;
    [SerializeField] SpriteRenderer[] spriteRenderers;

    public void Collapse()
    {
        //will colapse the border visulay and disable its colider or colliders
    }
}