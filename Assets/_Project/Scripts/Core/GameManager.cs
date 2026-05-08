using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private int totalDeliveriesForLevel = 2;
    [SerializeField] private int completedDeliveries = 0;



    public GameState CurrentGameState { get; private set; } = GameState.Playing;

    private void Awake()
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
        EventBus.GameE.OnResourceDelivered += OnResourceDelivered;

        GameState initialState = GameState.Playing;
        SetState(initialState);
        completedDeliveries = 0;


        GameplayUI.Instance?.UpdateDeliveries(completedDeliveries, totalDeliveriesForLevel);
    }

    private void OnDestroy()
    {
        EventBus.GameE.OnResourceDelivered -= OnResourceDelivered;


    }


    private void RestartLevel(InputAction.CallbackContext _)
    {
        if (CurrentGameState != GameState.GameOver)
            return;

        completedDeliveries = 0;
        SetState(GameState.Playing);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene()
            .buildIndex);
    }

    void OnResourceDelivered(ResourceData resource)
    {
        completedDeliveries++;
        GameplayUI.Instance?.UpdateDeliveries(completedDeliveries, totalDeliveriesForLevel);

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

public enum GameState
{
    Playing,
    GameOver
}
