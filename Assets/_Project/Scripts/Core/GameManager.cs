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
    
    public void OnTeleportsExhausted()
    {
        //if the player has no more teleports and hasn't completed the deliveries, they lose
        Debug.Log("LOSE — no teleports left");
    }
    
    public void OnResourceDelivered(ResourceData resource)
    {
        completedDeliveries++;

        if (completedDeliveries >= totalDeliveriesForLevel)
            WinGame();
    }

    void WinGame()
    {
        Debug.Log("WIN — borders dissolving!");
        BorderManager.Instance?.CollapseBorders();
    }
}
