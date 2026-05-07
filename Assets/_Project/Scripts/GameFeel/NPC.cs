using UnityEngine;
using DG.Tweening;

public class NPC : MonoBehaviour
{
    [SerializeField] private float bounceSpeed = 0.35f;
    [SerializeField] private float squashAmount = 0.2f;

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

        float delay = Random.Range(0f, 0.5f);

        Sequence bounce = DOTween.Sequence();
        bounce.AppendInterval(delay);
        bounce.Append(Squash());
        bounce.Append(Stretch());
        bounce.SetLoops(-1); // infinite
    }

    private Tween Squash() =>
        transform.DOScale(new Vector3(
            originalScale.x + squashAmount,
            originalScale.y - squashAmount * 0.5f,
            originalScale.z), bounceSpeed).SetEase(Ease.InOutSine);

    private Tween Stretch() =>
        transform.DOScale(new Vector3(
            originalScale.x - squashAmount * 0.5f,
            originalScale.y + squashAmount,
            originalScale.z), bounceSpeed).SetEase(Ease.InOutSine);

    private void OnDestroy()
    {
        transform.DOKill();
    }
}