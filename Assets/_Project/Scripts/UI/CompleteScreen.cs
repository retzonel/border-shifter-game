using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CompleteScreen : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private RectTransform panel;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Settings")] [SerializeField] private float delayBeforeShow = 0.5f;
    [SerializeField] private float animDuration = 0.6f;

    [Space] [SerializeField] private AudioClip showPanelClip, buttonClickClip;

    private void Start()
    {
        menuButton.onClick.AddListener(OnMenuButtonClicked);
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        panel.localScale = Vector3.zero;

        ShowPanel();
    }

    private void ShowPanel()
    {
        Sequence s = DOTween.Sequence();

        s.AppendInterval(delayBeforeShow);

        s.Append(canvasGroup.DOFade(1f, animDuration * 0.5f).SetEase(Ease.OutQuad));
        s.Join(panel.DOScale(Vector3.one * 1.15f, animDuration * 0.6f).SetEase(Ease.OutQuad));

        // slight overshoot settle
        s.Append(panel.DOScale(Vector3.one, animDuration * 0.3f).SetEase(Ease.InQuad));

        s.OnComplete(() =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });
        AudioManager.Instance?.PlaySound(showPanelClip);
    }

    private void OnMenuButtonClicked()
    {
        AudioManager.Instance?.PlaySound(buttonClickClip);
        LevelLoader.LoadLevel(0);
    }

    private void OnDestroy()
    {
        panel.DOKill();
        canvasGroup.DOKill();
    }
}