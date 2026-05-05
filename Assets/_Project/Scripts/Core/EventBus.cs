using UnityEngine;
using UnityEngine.Events;

public static class EventBus
{
    public static readonly GameEvents GameE = new GameEvents();
    
    public class GameEvents
    {
        public UnityAction OnRestartRequested;
        public UnityAction OnWinLevel;
        public UnityAction <ResourceData> OnResourceDelivered;
        public UnityAction TeleportExausted;
    }
}