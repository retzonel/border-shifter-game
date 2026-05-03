using UnityEngine;
using DG.Tweening;

public class Border : MonoBehaviour
{
    [SerializeField] Collider2D[] colliders;
    [SerializeField] SpriteRenderer[] spriteRenderers;

    public void Collapse()
    {
        // Disable colliders immediately so player can pass through
        foreach (var col in colliders)
            col.enabled = false;

        // Fade + shrink each sprite
        foreach (var sr in spriteRenderers)
        {
            sr.DOFade(0f, 0.8f);
            sr.transform.DOScaleY(0f, 0.8f).SetEase(Ease.InBack);
        }
    }
}