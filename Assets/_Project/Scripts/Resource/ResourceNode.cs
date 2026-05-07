using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class ResourceNode : MonoBehaviour
{
    public ResourceData data;
    public bool isPickedUp = false;
    [SerializeField] private AudioClip onPickupSFX;

    [SerializeField] private float bounceSpeed = 0.5f;
    [SerializeField] private float squashAmount = 0.08f;
    [SerializeField] private float hoverAmount = 0.08f;

    private Vector3 _originalScale;

    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (data != null && data.sprite != null)
            spriteRenderer.sprite = data.sprite;
        else
            Debug.LogWarning("nulref for " + gameObject.name);

        _originalScale = transform.localScale;

        float randomDelay = Random.Range(0f, 0.4f);
        AnimateIdle(randomDelay);
    }

    private void AnimateIdle(float delay)
    {
        Sequence s = DOTween.Sequence();
        s.AppendInterval(delay);

        s.Append(transform.DOMoveY(transform.position.y + hoverAmount, bounceSpeed)
            .SetEase(Ease.InOutSine));

        s.Join(transform.DOScale(new Vector3(
            _originalScale.x + squashAmount,
            _originalScale.y - squashAmount * 0.5f,
            _originalScale.z), bounceSpeed).SetEase(Ease.InOutSine));

        s.Append(transform.DOMoveY(transform.position.y, bounceSpeed)
            .SetEase(Ease.InOutSine));

        s.Join(transform.DOScale(new Vector3(
            _originalScale.x - squashAmount * 0.5f,
            _originalScale.y + squashAmount,
            _originalScale.z), bounceSpeed).SetEase(Ease.InOutSine));

        s.SetLoops(-1);
    }

    public void OnPickUp()
    {
        isPickedUp = true;
        AudioManager.Instance?.PlaySound(onPickupSFX);

        transform.DOKill();
        transform.DOScale(_originalScale * 1.3f, 0.08f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => gameObject.SetActive(false));
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}