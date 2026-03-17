using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Ссылки на ScriptableObject")]
    [SerializeField] GameStateSO gameState;
    [SerializeField] GameSettingsSO settings;

    [Header("Кнопки выбора уровня")]
    [SerializeField] Button level1Button;
    [SerializeField] Button level2Button;
    [SerializeField] Button level3Button;

    [Header("Основные кнопки")]
    [SerializeField] Button startButton;
    [SerializeField] Button closeButton;

    [Header("Ползунки громкости")]
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;

    [Header("Настройки экрана")]
    [SerializeField] Toggle fullscreenToggle;

    [Header("Панель подтверждения")]
    [SerializeField] ConfirmPanel confirmPanel;

    [Header("Визуал кнопок уровней")]
    [SerializeField] Image[] levelButtonImages;
    [SerializeField] Color selectedColor = Color.green;
    [SerializeField] Color lockedColor = new Color(0.5f, 0.5f, 0.5f, 0.7f);
    [SerializeField] Color availableColor = Color.white;

    private int selectedLevel = 1;

    void Awake() {
        // Инициализация настроек из SO
        sfxSlider.value = settings.sfxVolume;
        musicSlider.value = settings.musicVolume;
        fullscreenToggle.isOn = Screen.fullScreen;
        
        if (!Screen.fullScreen) {
            Screen.SetResolution(1920, 1080, false);
        }

        // Подписка на события кнопок (со звуком)
        startButton.onClick.AddListener(() => PlayClickAndAction(OnStartClicked));
        closeButton.onClick.AddListener(OnCloseClicked); // 🔴 Теперь открывает панель
        
        sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);

        // Настройка кнопок уровней
        level1Button.onClick.AddListener(() => OnLevelSelected(1));
        level2Button.onClick.AddListener(() => OnLevelSelected(2));
        level3Button.onClick.AddListener(() => OnLevelSelected(3));

        // Обновление визуала кнопок уровней
        UpdateLevelButtonsVisual();
    }

    void OnLevelSelected(int level) {
        if (level > gameState.maxUnlockedLevel) {
            return;
        }

        selectedLevel = level;
        gameState.selectedLevel = selectedLevel;
        
        Audio.Instance.PlayClick();
        UpdateLevelButtonsVisual();
    }

    void UpdateLevelButtonsVisual() {
        for (int i = 0; i < levelButtonImages.Length; i++) {
            int levelNum = i + 1;
            Button btn = levelButtonImages[i].gameObject.GetComponent<Button>();
            
            if (levelNum > gameState.maxUnlockedLevel) {
                levelButtonImages[i].color = lockedColor;
                btn.interactable = false;
            } else if (levelNum == selectedLevel) {
                levelButtonImages[i].color = selectedColor;
                btn.interactable = true;
            } else {
                levelButtonImages[i].color = availableColor;
                btn.interactable = true;
            }
        }
    }

    void OnStartClicked() {
        SceneLoader.LoadLetterScene();
    }

    void OnCloseClicked() {
        // Открываем панель подтверждения вместо закрытия
        confirmPanel.Show();
    }

    void OnSfxVolumeChanged(float volume) {
        settings.sfxVolume = volume;
        Audio.Instance.SetSfxVolume(volume);
    }

    void OnMusicVolumeChanged(float volume) {
        settings.musicVolume = volume;
        Audio.Instance.SetMusicVolume(volume);
    }

    void OnFullscreenChanged(bool isFullscreen) {
        Audio.Instance.PlayClick();
        
        Screen.fullScreen = isFullscreen;
        
        if (!isFullscreen) {
            Screen.SetResolution(1920, 1080, false);
        } else {
            Screen.SetResolution(Screen.currentResolution.width, 
                                 Screen.currentResolution.height, 
                                 true);
        }
    }

    void PlayClickAndAction(System.Action action) {
        Audio.Instance.PlayClick();
        action?.Invoke();
    }

    void OnDestroy() {
        startButton.onClick.RemoveListener(() => PlayClickAndAction(OnStartClicked));
        closeButton.onClick.RemoveListener(OnCloseClicked);
        
        sfxSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
        musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        fullscreenToggle.onValueChanged.RemoveListener(OnFullscreenChanged);
    }
}