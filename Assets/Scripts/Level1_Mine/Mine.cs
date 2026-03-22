using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Mine : MonoBehaviour
{
    [Header("Ссылки на SO")]
    [SerializeField] GameStateSO gameState;
    [SerializeField] GameSettingsSO settings;
    [SerializeField] LevelDataSO levelData;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI textMoney;
    [SerializeField] TextMeshProUGUI textDebt;
    [SerializeField] TextMeshProUGUI textTimer;
    [SerializeField] Slider sliderTimerProgress;

    [Header("Панели")]
    [SerializeField] GameObject panelIntro;
    [SerializeField] GameObject panelEndWin;
    [SerializeField] GameObject panelEndLose;

    [Header("Тексты концовки")]
    [SerializeField] TextMeshProUGUI textSummary;
    [SerializeField] TextMeshProUGUI textExpenses;
    [SerializeField] TextMeshProUGUI textFinalBalance;

    [Header("Настройки")]
    [SerializeField] float levelDuration = 150f;
    [SerializeField] int debtAmount = 50000;

    [Header("Звуки")]
    [SerializeField] AudioClip levelBGM;

    private float timeRemaining;
    private int currentMoney;
    private bool isGameActive = false;
    private bool isPaused = false;

    void Awake()
    {
        timeRemaining = levelDuration;
        currentMoney = gameState.currentMoney;
        
        panelIntro.SetActive(true);
        panelEndWin.SetActive(false);
        panelEndLose.SetActive(false);
        
        Time.timeScale = 1f;
    }

    void Start()
    {
        UpdateUI();
        Fade.Instance.FadeOut();
    }

    void Update()
    {
        if (isGameActive && !isPaused)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
            
            if (timeRemaining <= 0)
            {
                EndLevel();
            }
        }
    }

    public void StartGame()
    {
        panelIntro.SetActive(false);
        isGameActive = true;
        Audio.Instance.PlayMusic(levelBGM);
    }
    
    public bool IsGameActive()
    {
        return isGameActive && !isPaused;
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateUI();
    }

    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    void UpdateUI()
    {
        textMoney.text = $"{currentMoney:N0} ₽";
        textDebt.text = $"{debtAmount:N0} ₽";
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        textTimer.text = $"{minutes:00}:{seconds:00}";
        
        sliderTimerProgress.value = timeRemaining / levelDuration;
    }

    void EndLevel()
    {
        isGameActive = false;
        
        if (currentMoney >= debtAmount)
        {
            WinLevel();
        }
        else
        {
            LoseLevel();
        }
    }

    void WinLevel()
    {
        panelEndWin.SetActive(true);
        
        int foodExpense = Mathf.Max(5000, Mathf.FloorToInt(currentMoney * 0.3f));
        int rentExpense = Mathf.Max(5000, Mathf.FloorToInt(currentMoney * 0.3f));
        int utilityExpense = Mathf.Max(5000, Mathf.FloorToInt(currentMoney * 0.3f));
        int totalExpenses = foodExpense + rentExpense + utilityExpense;
        int finalBalance = Mathf.Max(0, currentMoney - totalExpenses);
        
        textSummary.text = $"Ура! Долг погашен!\nВаш заработок: {currentMoney:N0} ₽";
        textExpenses.text = $"Расходы:\nЕда: -{foodExpense:N0} ₽\nАренда: -{rentExpense:N0} ₽\nКоммуналка: -{utilityExpense:N0} ₽\nИтого: -{totalExpenses:N0} ₽";
        textFinalBalance.text = $"Ваш баланс: {finalBalance:N0} ₽";
        
        gameState.currentMoney = finalBalance;
        gameState.UnlockLevel(2);
    }

    void LoseLevel()
    {
        panelEndLose.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnRetryClicked()
    {
        Time.timeScale = 1f;
        if (Audio.Instance != null)
        {
            Audio.Instance.FadeOutMusic(1f);
        }
        Fade.Instance.FadeIn();
        Invoke(nameof(ReloadScene), 1f);
    }

    public void OnNextLevelClicked()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.FadeOutMusic(1f);
        }
        Fade.Instance.FadeIn();
        Invoke(nameof(LoadNextScene), 1f);
    }

    void ReloadScene()
    {
        SceneLoader.LoadLevel(1);
    }

    void LoadNextScene()
    {
        SceneLoader.LoadLetterScene(2);
    }

    void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
