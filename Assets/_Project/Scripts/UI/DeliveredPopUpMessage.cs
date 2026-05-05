using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class DeliveredPopUpMessage : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float showDuration = 0.3f;
    [SerializeField] private float displayDuration = 1.5f;
    [SerializeField] private float hideDuration = 0.3f;
    [SerializeField] private float moveOffset = 50f;

    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Vector2 _originalPosition;
    private Sequence _currentSequence;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _originalPosition = _rectTransform.anchoredPosition;
    }

    private void Start()
    {
        EventBus.GameE.OnResourceDelivered += ShowMessage;
        Hide();
    }

    private void OnDestroy()
    {
        EventBus.GameE.OnResourceDelivered -= ShowMessage;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        _canvasGroup.alpha = 0f;
        _rectTransform.anchoredPosition = _originalPosition;
    }

    private void ShowMessage(ResourceData _)
    {
        _currentSequence?.Kill();

        gameObject.SetActive(true);
        _canvasGroup.alpha = 0f;
        _rectTransform.anchoredPosition = _originalPosition;

        _currentSequence = DOTween.Sequence();
        
        _currentSequence.Append(_canvasGroup.DOFade(1f, showDuration).SetEase(Ease.OutQuad));
        _currentSequence.Join(_rectTransform.DOAnchorPosY(_originalPosition.y + moveOffset, showDuration).SetEase(Ease.OutQuad));
        
        _currentSequence.AppendInterval(displayDuration);
        
        _currentSequence.Append(_canvasGroup.DOFade(0f, hideDuration).SetEase(Ease.InQuad));
        _currentSequence.OnComplete(Hide);
    }
}