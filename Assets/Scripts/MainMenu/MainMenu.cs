using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Ссылки на ScriptableObject")]
    [SerializeField] private GameStateSO gameState;
    [SerializeField] private GameSettingsSO settings;

    [Header("Кнопки выбора уровня")]
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private Button level3Button;

    [Header("Основные кнопки")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button closeButton;

    [Header("Ползунки громкости")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    [Header("Настройки экрана")]
    [SerializeField] private Toggle fullscreenToggle;

    [Header("Панель подтверждения")]
    [SerializeField] private ConfirmPanel confirmPanel;

    [Header("Визуал кнопок уровней")]
    [SerializeField] private Image[] levelButtonImages;
    [SerializeField] private Color selectedColor = Color.green;
    [SerializeField] private Color lockedColor = new Color(0.5f, 0.5f, 0.5f, 0.7f);
    [SerializeField] private Color availableColor = Color.white;

    private int selectedLevel = 1;
    private const int TARGET_WIDTH = 1920;
    private const int TARGET_HEIGHT = 1080;

    void Awake()
    {
        Screen.SetResolution(TARGET_WIDTH, TARGET_HEIGHT, true);
        fullscreenToggle.isOn = true;
        
        sfxSlider.value = settings.sfxVolume;
        musicSlider.value = settings.musicVolume;
        
        startButton.onClick.AddListener(() => PlayClickAndAction(OnStartClicked));
        closeButton.onClick.AddListener(OnCloseClicked);
        
        sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        
        level1Button.onClick.AddListener(() => OnLevelSelected(1));
        level2Button.onClick.AddListener(() => OnLevelSelected(2));
        level3Button.onClick.AddListener(() => OnLevelSelected(3));
        
        UpdateLevelButtonsVisual();
    }

    void Start()
    {
        Audio.Instance.PlayMusic(settings.menuMusic);
    }

    void OnLevelSelected(int level)
    {
        if (level > gameState.maxUnlockedLevel)
        {
            return;
        }

        selectedLevel = level;
        gameState.selectedLevel = selectedLevel;
        
        Audio.Instance.PlayClick();
        UpdateLevelButtonsVisual();
    }

    void UpdateLevelButtonsVisual()
    {
        for (int i = 0; i < levelButtonImages.Length; i++)
        {
            int levelNum = i + 1;
            Button btn = levelButtonImages[i].gameObject.GetComponent<Button>();
            
            if (levelNum > gameState.maxUnlockedLevel)
            {
                levelButtonImages[i].color = lockedColor;
                btn.interactable = false;
            } else if (levelNum == selectedLevel)
            {
                levelButtonImages[i].color = selectedColor;
                btn.interactable = true;
            }
            else
            {
                levelButtonImages[i].color = availableColor;
                btn.interactable = true;
            }
        }
    }

    void OnStartClicked()
    {
        Audio.Instance.PlayClick();
    
        if (Fade.Instance != null)
        {
            Fade.Instance.FadeIn();
            Invoke(nameof(LoadLetterAfterFade), 1f);
        }
        else
        {
            LoadLetterAfterFade();
        }
    }
    
    void LoadLetterAfterFade()
    {
        SceneLoader.LoadLetterScene(gameState.selectedLevel);
    }

    void OnCloseClicked()
    {
        confirmPanel.Show();
    }

    void OnSfxVolumeChanged(float volume)
    {
        settings.sfxVolume = volume;
        Audio.Instance.SetSfxVolume(volume);
    }

    void OnMusicVolumeChanged(float volume)
    {
        settings.musicVolume = volume;
        Audio.Instance.SetMusicVolume(volume);
    }

    void OnFullscreenChanged(bool isFullscreen)
    {
        Audio.Instance.PlayClick();
        Screen.SetResolution(TARGET_WIDTH, TARGET_HEIGHT, isFullscreen);
    }

    void PlayClickAndAction(System.Action action)
    {
        Audio.Instance.PlayClick();
        action?.Invoke();
    }

    void OnDestroy()
    {
        startButton.onClick.RemoveListener(() => PlayClickAndAction(OnStartClicked));
        closeButton.onClick.RemoveListener(OnCloseClicked);
        
        sfxSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
        musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        fullscreenToggle.onValueChanged.RemoveListener(OnFullscreenChanged);
    }
}