using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroScreen : MonoBehaviour
{
    private string storyLine;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private Button skipAction;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Settings")] [SerializeField] private float typeSpeed = 0.04f;
    [SerializeField] private float fadeOutDuration = 0.6f;

    private Coroutine _typingCoroutine;
    private bool _typingComplete = false;

    void Start()
    {
        storyLine =
            "OPERATION: BORDER SHIFT" + "\n\n" +
            "Objective:" + "\n" +
            "Restore cooperation between neighboring nations." + "\n\n" +
            "Deliver critical resources across restricted borders using limited teleportation charges." + "\n\n" +
            "Every move matters." + "\n" +
            "Every route counts." + "\n\n" +
            "Complete exchanges." + "\n" +
            "Break the borders." + "\n" +
            "Reunite the people. Play without borders!!!";

        storyText.text = "";
        canvasGroup.alpha = 1f;


        skipAction.onClick.AddListener(() => OnSkip(new InputAction.CallbackContext()));

        _typingCoroutine = StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        storyText.text = "";

        foreach (char c in storyLine)
        {
            storyText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        _typingComplete = true;
        Invoke("Dismiss", 2f);
    }

    private void OnSkip(InputAction.CallbackContext ctx)
    {
        if (!_typingComplete)
        {
            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);

            storyText.text = storyLine;
            _typingComplete = true;
        }
        else
        {
            Dismiss();
        }
    }

    private void Dismiss()
    {
        canvasGroup.DOFade(0f, fadeOutDuration)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                
                int _ = SceneManager.GetActiveScene().buildIndex + 1;
                LevelLoader.LoadLevel(_);
            });
    }
}