using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Toggle _fullscreenToggle;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private GameSettingsSO _settings;

    private bool isPaused = false;
    private Button _currentPauseButton;

    private void Start()
    {
        _pausePanel.SetActive(false);
        FindPauseButtonInScene();
        SetupUI();
    }

    private void FindPauseButtonInScene()
    {
        GameObject pauseBtnObj = GameObject.FindGameObjectWithTag("PauseButton");
    
        if (pauseBtnObj != null)
        {
            Button pauseButton = pauseBtnObj.GetComponent<Button>();
            if (pauseButton != null)
            {
                _currentPauseButton = pauseButton;
                pauseButton.onClick.AddListener(Pause);
            }
        }
    }

    private void SetupUI()
    {
        if (_settings != null)
        {
            _sfxSlider.value = _settings.sfxVolume;
            _musicSlider.value = _settings.musicVolume;
        }
        
        _fullscreenToggle.isOn = Screen.fullScreen;

        _continueButton.onClick.AddListener(OnContinueClicked);
        _exitButton.onClick.AddListener(OnExitClicked);
        _fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        _sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        _pausePanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        if (Audio.Instance != null)
        {
            Audio.Instance.PauseMusic();
        }
    }

    private void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        _pausePanel.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        if (Audio.Instance != null)
        {
            Audio.Instance.ResumeMusic();
        }
    }

    private void OnContinueClicked()
    {
        Audio.Instance.PlayClick();
        Resume();
    }

    private void OnExitClicked()
    {
        Audio.Instance.PlayClick();
        Time.timeScale = 1f;
        _pausePanel.SetActive(false);
        isPaused = false;
        
        if (Audio.Instance != null)
        {
            Audio.Instance.ResumeMusic();
        }
        
        if (Fade.Instance != null)
        {
            Fade.Instance.FadeIn();
            Invoke(nameof(LoadMainMenu), 1f);
        }
        else
        {
            LoadMainMenu();
        }
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnFullscreenChanged(bool isFullscreen)
    {
        Audio.Instance.PlayClick();
        Screen.fullScreen = isFullscreen;
        
        if (!isFullscreen)
        {
            Screen.SetResolution(1920, 1080, false);
        }
        else
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
    }

    private void OnSfxVolumeChanged(float volume)
    {
        if (_settings != null)
        {
            _settings.sfxVolume = volume;
        }
        Audio.Instance.SetSfxVolume(volume);
    }

    private void OnMusicVolumeChanged(float volume)
    {
        if (_settings != null)
        {
            _settings.musicVolume = volume;
        }
        Audio.Instance.SetMusicVolume(volume);
    }

    private void OnDestroy()
    {
        if (_currentPauseButton != null)
        {
            _currentPauseButton.onClick.RemoveListener(Pause);
        }
        
        _continueButton.onClick.RemoveListener(OnContinueClicked);
        _exitButton.onClick.RemoveListener(OnExitClicked);
        _fullscreenToggle.onValueChanged.RemoveListener(OnFullscreenChanged);
        _sfxSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
        _musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        
        Time.timeScale = 1f;
    }

    private void OnApplicationQuit()
    {
        Time.timeScale = 1f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}