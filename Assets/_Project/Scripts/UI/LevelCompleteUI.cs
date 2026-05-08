using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class LevelCompleteUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [SerializeField] private Button nextLevelButton, mainMenuButton;
    [SerializeField] private float delayBeforeShow = 1f;
    [SerializeField] private float animDuration = 0.5f;

    [SerializeField] private RectTransform panelRect;

    [Space] [SerializeField] AudioClip buttonClickSound, showPanelSound;
    

    void Start()
    {
        nextLevelButton.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlaySound(buttonClickSound);
            LevelLoader.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            AudioManager.Instance?.PlaySound(buttonClickSound);
            LevelLoader.LoadLevel(0);
        });

        EventBus.GameE.OnWinLevel += Show;
        canvasGroup = GetComponent<CanvasGroup>();
        panelRect = panelRect != null ? panelRect : GetComponent<RectTransform>();
        Hide();
    }

    private void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventBus.GameE.OnWinLevel -= Show;
    }

    private void Show()
    {
        gameObject.SetActive(true);

        canvasGroup.DOKill();
        panelRect?.DOKill();

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if (panelRect != null)
            panelRect.localScale = Vector3.one * 0.85f;

        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(delayBeforeShow);

        seq.Append(canvasGroup.DOFade(1f, animDuration).SetEase(Ease.OutCubic));

        if (panelRect != null)
        {
            seq.Join(panelRect.DOScale(Vector3.one, animDuration).SetEase(Ease.OutBack));
        }

        seq.OnComplete(() =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });
        AudioManager.Instance?.PlaySound(showPanelSound);
    }
}