using TMPro;
using UnityEngine;
using DG.Tweening;

public class GameplayUI : MonoBehaviour
{
    public static GameplayUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI chargesText;
    [SerializeField] private TextMeshProUGUI carriedResourceText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UpdateCharges(int current, int max)
    {
        chargesText.text = $"Teleports: {current} / {max}";

        chargesText.transform.DOKill();
        chargesText.transform.DOPunchScale(Vector3.one * 0.2f, 0.25f);

        if (current <= 2)
            chargesText.DOColor(Color.red, 0.2f); 
            else chargesText.DOColor(Color.white, 0.2f);
    }

    public void UpdateCarriedResource(ResourceData resource)
    {
        carriedResourceText.text = resource != null
            ? $"Carrying: {resource.resourceName}"
            : "Carrying: nothing";

        carriedResourceText.transform.DOKill();
        carriedResourceText.transform.DOPunchScale(Vector3.one * 0.15f, 0.2f);
    }
}