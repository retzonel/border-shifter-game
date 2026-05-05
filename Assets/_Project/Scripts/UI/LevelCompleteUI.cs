using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class LevelCompleteUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    [SerializeField] private Button nextLevelButton, mainMenuButton;

    void Start()
    {
        nextLevelButton.onClick.AddListener(() =>
        {
            LevelLoader.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            LevelLoader.LoadLevel(0);
        });

        
        EventBus.GameE.OnWinLevel += Show;
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventBus.GameE.OnWinLevel -= Show;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}