using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("Баланс")]
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private Button resetBalanceButton;
    [SerializeField] private GameObject resetConfirmPanel;
    [SerializeField] private Color positiveColor = Color.green;
    [SerializeField] private Color negativeColor = Color.red;

    private int selectedLevel = 1;
    private const int TARGET_WIDTH = 1920;
    private const int TARGET_HEIGHT = 1080;

    private void Awake()
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
        
        if (resetBalanceButton != null)
        {
            resetBalanceButton.onClick.AddListener(OnResetBalanceClicked);
        }
        
        selectedLevel = gameState.selectedLevel;
        
        if (resetConfirmPanel != null)
        {
            resetConfirmPanel.SetActive(false);
        }
        
        UpdateLevelButtonsVisual();
    }

    private void Start()
    {
        Audio.Instance.PlayMusic(settings.menuMusic);
        UpdateBalanceDisplay();
    }

    private void Update()
    {
        UpdateBalanceDisplay();
    }

    private void OnLevelSelected(int level)
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

    private void UpdateLevelButtonsVisual()
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

    private void UpdateBalanceDisplay()
    {
        if (Economy.Instance != null && balanceText != null)
        {
            int balance = Economy.Instance.GlobalBalance;
            balanceText.text = $"Баланс: \n{balance:N0} ₽";
            balanceText.color = balance >= 0 ? positiveColor : negativeColor;
        
            if (resetBalanceButton != null)
            {
                resetBalanceButton.gameObject.SetActive(balance < 0);
            }
        }
    }

    private void OnStartClicked()
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
    
    private void LoadLetterAfterFade()
    {
        SceneLoader.LoadLetterScene(gameState.selectedLevel);
    }

    private void OnCloseClicked()
    {
        confirmPanel.Show();
    }

    private void OnResetBalanceClicked()
    {
        Audio.Instance.PlayClick();
        
        if (resetConfirmPanel != null)
        {
            resetConfirmPanel.SetActive(true);
        }
    }

    public void ConfirmResetBalance()
    {
        Audio.Instance.PlayClick();
        
        if (Economy.Instance != null)
        {
            Economy.Instance.ResetBalance();
        }
        
        if (resetConfirmPanel != null)
        {
            resetConfirmPanel.SetActive(false);
        }
        
        UpdateLevelButtonsVisual();
    }

    public void CancelResetBalance()
    {
        Audio.Instance.PlayClick();
        
        if (resetConfirmPanel != null)
        {
            resetConfirmPanel.SetActive(false);
        }
    }

    private void OnSfxVolumeChanged(float volume)
    {
        settings.sfxVolume = volume;
        Audio.Instance.SetSfxVolume(volume);
    }

    private void OnMusicVolumeChanged(float volume)
    {
        settings.musicVolume = volume;
        Audio.Instance.SetMusicVolume(volume);
    }

    private void OnFullscreenChanged(bool isFullscreen)
    {
        Audio.Instance.PlayClick();
        Screen.SetResolution(TARGET_WIDTH, TARGET_HEIGHT, isFullscreen);
    }

    private void PlayClickAndAction(System.Action action)
    {
        Audio.Instance.PlayClick();
        action?.Invoke();
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveListener(() => PlayClickAndAction(OnStartClicked));
        closeButton.onClick.RemoveListener(OnCloseClicked);
        
        sfxSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
        musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        fullscreenToggle.onValueChanged.RemoveListener(OnFullscreenChanged);
        
        if (resetBalanceButton != null)
        {
            resetBalanceButton.onClick.RemoveListener(OnResetBalanceClicked);
        }
    }
}