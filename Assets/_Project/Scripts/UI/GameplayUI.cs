using System;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public static GameplayUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI chargesText;
    [SerializeField] private TextMeshProUGUI deliveriesText;
    [SerializeField] private Image carriedResourceIcon;
    [SerializeField] private Sprite emptyIcon;
    [SerializeField] private GameObject teleportExhaustedRestartPrompt;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UpdateCarriedResource(null);
        EventBus.GameE.TeleportExausted += ShowRestartPrompt;
    }
    
    void ShowRestartPrompt()
    {
        if (teleportExhaustedRestartPrompt != null)
        {
            teleportExhaustedRestartPrompt.SetActive(true);
        }
    }

    public void UpdateCharges(int current, int max)
    {
        chargesText.text = $"{current}";

        chargesText.transform.DOKill();
        chargesText.transform.DOPunchScale(Vector3.one * 0.2f, 0.25f);

        chargesText.DOColor(current <= 2 ? Color.red : Color.white, 0.2f);
    }

    public void UpdateCarriedResource(ResourceData resource)
    {
        if (resource == null && emptyIcon != null)
        {
            carriedResourceIcon.sprite = emptyIcon;
            return;
        }

        carriedResourceIcon.sprite = resource.sprite;


        carriedResourceIcon.transform.DOKill();
        carriedResourceIcon.transform.DOPunchScale(Vector3.one * 0.15f, 0.2f);
    }
    
    public void UpdateDeliveries(int current, int total)
    {
        deliveriesText.text = $"{current}/{total}";

        deliveriesText.transform.DOKill();
        deliveriesText.transform.DOPunchScale(Vector3.one * 0.15f, 0.2f);
    }
}