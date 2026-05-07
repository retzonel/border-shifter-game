using System;
using UnityEngine;
using UnityEngine.UI;

public class CompleteScreen : MonoBehaviour
{
    [SerializeField] private Button menuButton;

    private void Start()
    {
        menuButton.onClick.AddListener(OnMenuButtonClicked);
    }

    private void OnMenuButtonClicked()
    {
        LevelLoader.LoadLevel(0);
    }
}
