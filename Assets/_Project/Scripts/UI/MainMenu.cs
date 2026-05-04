using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    
    void Start()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }
    
    private void OnPlayClicked()
    {
        // Load the main game scene
    }
    
    private void OnQuitClicked()
    {
        // Quit the application
        Application.Quit();
    }
    
    private void OnMusicVolumeChanged(float value)
    {
        // Adjust music volume
        // AudioManager.Instance.SetMusicVolume(value);
    }
    
    private void OnSFXVolumeChanged(float value)
    {
        // Adjust SFX volume
        // AudioManager.Instance.SetSFXVolume(value);
    }
}
