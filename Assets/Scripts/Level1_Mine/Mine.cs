using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Mine : MonoBehaviour
{
    [Header("Ссылки на SO")]
    [SerializeField] private GameStateSO _gameState;
    [SerializeField] private GameSettingsSO _settings;
    [SerializeField] private LevelDataSO _levelData;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _textMoney;
    [SerializeField] private TextMeshProUGUI _textDebt;
    [SerializeField] private TextMeshProUGUI _textTimer;
    [SerializeField] private Slider _sliderTimerProgress;
    
    [Header("Панели")]
    [SerializeField] private GameObject _panelIntro;
    [SerializeField] private GameObject _panelEndWin;
    [SerializeField] private GameObject _panelEndLose;
    
    [Header("Тексты концовки")]
    [SerializeField] private TextMeshProUGUI _textSummary;
    [SerializeField] private TextMeshProUGUI _textExpenses;
    [SerializeField] private TextMeshProUGUI _textFinalBalance;

    [Header("Настройки")]
    [SerializeField] private float _levelDuration = 150f;
    [SerializeField] private int _debtAmount = 50000;
    
    [Header("Звуки")]
    [SerializeField] private AudioClip _levelBGM;
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private AudioClip _loseSound;

    private float _timeRemaining;
    private int _currentMoney;
    private bool _isGameActive = false;
    private bool _isPaused = false;

    private void Awake()
    {
        _timeRemaining = _levelDuration;
        _currentMoney = _gameState.currentMoney;
        
        _panelIntro.SetActive(true);
        _panelEndWin.SetActive(false);
        _panelEndLose.SetActive(false);
        
        Time.timeScale = 1f;
    }

    private void Start()
    {
        UpdateUI();
        Fade.Instance.FadeOut();
        
        if (Audio.Instance != null)
        {
            Audio.Instance.StopMusic();
            Audio.Instance.ResumeMusic();
        }
    }

    private void Update()
    {
        if (_isGameActive && !_isPaused)
        {
            _timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
            
            if (_timeRemaining <= 0)
            {
                EndLevel();
            }
        }
    }

    public void StartGame()
    {
        _panelIntro.SetActive(false);
        _isGameActive = true;
        
        if (Audio.Instance != null && _levelBGM != null)
        {
            Audio.Instance.PlayMusic(_levelBGM);
        }
    }
    
    public bool IsGameActive()
    {
        return _isGameActive && !_isPaused;
    }

    public float GetTimeRemaining()
    {
        return _timeRemaining;
    }

    public void AddMoney(int amount)
    {
        _currentMoney += amount;
        UpdateUI();
    }

    public int GetCurrentMoney()
    {
        return _currentMoney;
    }

    private void UpdateUI()
    {
        _textMoney.text = $"{_currentMoney:N0} ₽";
        _textDebt.text = $"{_debtAmount:N0} ₽";
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(_timeRemaining / 60);
        int seconds = Mathf.FloorToInt(_timeRemaining % 60);
        _textTimer.text = $"{minutes:00}:{seconds:00}";
        
        _sliderTimerProgress.value = _timeRemaining / _levelDuration;
    }

    private void EndLevel()
    {
        _isGameActive = false;
        
        if (_currentMoney >= _debtAmount)
        {
            WinLevel();
        }
        else
        {
            LoseLevel();
        }
    }

    private void WinLevel()
    {
        _panelEndWin.SetActive(true);
    
        if (Audio.Instance != null)
        {
            Audio.Instance.StopMusic();
            if (_winSound != null)
            {
                Audio.Instance.PlaySfx(_winSound);
            }
        }
    
        int foodExpense = Mathf.Max(5000, Mathf.FloorToInt(_currentMoney * 0.3f));
        int rentExpense = Mathf.Max(5000, Mathf.FloorToInt(_currentMoney * 0.3f));
        int utilityExpense = Mathf.Max(5000, Mathf.FloorToInt(_currentMoney * 0.3f));
        int totalExpenses = foodExpense + rentExpense + utilityExpense;
        int finalBalance = Mathf.Max(0, _currentMoney - totalExpenses);
    
        _textSummary.text = $"Ура! Долг погашен!\nВаш заработок: {_currentMoney:N0} ₽ \nОсталось оплатить ежемесячные платежи:";
        _textExpenses.text = $"Расходы:\nЕда: -{foodExpense:N0} ₽\nАренда: -{rentExpense:N0} ₽\nКоммуналка: -{utilityExpense:N0} ₽\nИтого: -{totalExpenses:N0} ₽";
        _textFinalBalance.text = $"Ваш баланс: {finalBalance:N0} ₽";
    
        _gameState.currentMoney = finalBalance;
        _gameState.UnlockLevel(2);
        _gameState.currentLevel = 2;
    }

    public void OnNextLevelClicked()
    {
        _gameState.selectedLevel = _gameState.currentLevel;
    
        if (Audio.Instance != null)
        {
            Audio.Instance.FadeOutMusic(1f);
        }
    
        if (Fade.Instance != null)
        {
            Fade.Instance.FadeIn();
        }
    
        StartCoroutine(LoadNextSceneAfterDelay(1f));
    }

    private void LoseLevel()
    {
        _panelEndLose.SetActive(true);
        Time.timeScale = 0f;
        
        if (Audio.Instance != null)
        {
            Audio.Instance.StopMusic();
            if (_loseSound != null)
            {
                Audio.Instance.PlaySfx(_loseSound);
            }
        }
    }

    public void OnRetryClicked()
    {
        Time.timeScale = 1f;
        
        if (Audio.Instance != null)
        {
            Audio.Instance.StopAllSfx();
            Audio.Instance.FadeOutMusic(1f);
        }
    
        if (Fade.Instance != null)
        {
            Fade.Instance.FadeIn();
        }
    
        StartCoroutine(ReloadSceneAfterDelay(1f));
    }
    

    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneLoader.LoadLevel(1);
    }

    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneLoader.LoadLetterScene(2);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}