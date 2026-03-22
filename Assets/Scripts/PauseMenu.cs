using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Toggle _fullscreenToggle;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private GameSettingsSO _settings;

    private bool isPaused = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _pausePanel.SetActive(false);

        _continueButton.onClick.AddListener(OnContinueClicked);
        _exitButton.onClick.AddListener(OnExitClicked);
        _fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        _sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }

    void Start()
    {
        if (_settings != null)
        {
            _sfxSlider.value = _settings.sfxVolume;
            _musicSlider.value = _settings.musicVolume;
        }
        
        _fullscreenToggle.isOn = Screen.fullScreen;
    }

    void Update()
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

    public void Resume()
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

    void OnContinueClicked()
    {
        Audio.Instance.PlayClick();
        Resume();
    }

    void OnExitClicked()
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

    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void OnFullscreenChanged(bool isFullscreen)
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

    void OnSfxVolumeChanged(float volume)
    {
        if (_settings != null)
        {
            _settings.sfxVolume = volume;
        }
        Audio.Instance.SetSfxVolume(volume);
    }

    void OnMusicVolumeChanged(float volume)
    {
        if (_settings != null)
        {
            _settings.musicVolume = volume;
        }
        Audio.Instance.SetMusicVolume(volume);
    }

    void OnDestroy()
    {
        _continueButton.onClick.RemoveListener(OnContinueClicked);
        _exitButton.onClick.RemoveListener(OnExitClicked);
        _fullscreenToggle.onValueChanged.RemoveListener(OnFullscreenChanged);
        _sfxSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
        _musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        
        Time.timeScale = 1f;
    }

    void OnApplicationQuit()
    {
        Time.timeScale = 1f;
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }
}