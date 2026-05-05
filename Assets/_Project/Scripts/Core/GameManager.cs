using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private int totalDeliveriesForLevel = 2;
    [SerializeField] private int completedDeliveries = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        EventBus.GameE.OnRestartRequested += RestartLevel;
        EventBus.GameE.OnResourceDelivered += OnResourceDelivered;
    }

    private void OnDestroy()
    {
        EventBus.GameE.OnRestartRequested -= RestartLevel;
        EventBus.GameE.OnResourceDelivered -= OnResourceDelivered;
    }

    public void RestartLevel()
    {
        completedDeliveries = 0;
    }

    void OnResourceDelivered(ResourceData resource)
    {
        completedDeliveries++;

        if (completedDeliveries >= totalDeliveriesForLevel)
            WinGame();
    }

    void WinGame()
    {
        BorderManager.Instance?.CollapseBorders();
        EventBus.GameE.OnWinLevel?.Invoke();
    }
}