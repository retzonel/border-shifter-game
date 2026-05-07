using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private int totalDeliveriesForLevel = 2;
    [SerializeField] private int completedDeliveries = 0;

    public enum GameState
    {
        Playing,
        GameOver
    }

    public GameState CurrentGameState { get; private set; } = GameState.Playing;

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

        GameState initialState = GameState.Playing;
        SetState(initialState);
    }

    private void Update()
    {
    }

    private void OnDestroy()
    {
        EventBus.GameE.OnRestartRequested -= RestartLevel;
        EventBus.GameE.OnResourceDelivered -= OnResourceDelivered;
    }

    public void RestartLevel()
    {
        completedDeliveries = 0;
        SetState(GameState.Playing);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene()
            .buildIndex);
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

        SetState(GameState.GameOver);
        EventBus.GameE.OnWinLevel?.Invoke();
    }

    public void SetState(GameState newState)
    {
        CurrentGameState = newState;
    }

    public GameState GetState()
    {
        return CurrentGameState;
    }
}