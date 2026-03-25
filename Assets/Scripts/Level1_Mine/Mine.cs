using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Mine : MonoBehaviour
{
    [SerializeField] private GameStateSO _gameState;
    [SerializeField] private GameSettingsSO _settings;
    [SerializeField] private LevelDataSO _levelData;
    [SerializeField] private TextMeshProUGUI _textBalance;
    [SerializeField] private TextMeshProUGUI _textTimer;
    [SerializeField] private Slider _sliderTimerProgress;
    [SerializeField] private GameObject _panelIntro;
    [SerializeField] private float _levelDuration = 150f;
    [SerializeField] private AudioClip _levelBGM;
    [SerializeField] private AudioClip _introMusic;
    [SerializeField] private Color _positiveColor = Color.green;
    [SerializeField] private Color _negativeColor = Color.red;

    [Header("Level End")]
    [SerializeField] private LevelEndHandler _levelEndHandler;
    [SerializeField] private int _debtAmount = 50000;

    private float _timeRemaining;
    private int _startBalance;
    private int _levelEarnings = 0;
    private bool _isGameActive = false;
    private bool _isPaused = false;
    private bool _isLevelEnded = false;

    private void Awake()
    {
        _timeRemaining = _levelDuration;
        
        if (Economy.Instance != null)
        {
            _startBalance = Economy.Instance.GetStartingBalanceForLevel(_debtAmount);
        }
        else
        {
            _startBalance = -_debtAmount;
        }
        
        _panelIntro.SetActive(true);
        Time.timeScale = 1f;
    }

    private void Start()
    {
        UpdateUI();
        Fade.Instance.FadeOut();
        
        if (Audio.Instance != null && _introMusic != null)
        {
            Audio.Instance.PlayMusic(_introMusic);
        }
    }

    private void Update()
    {
        if (_isGameActive && !_isPaused && !_isLevelEnded)
        {
            _timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
            
            if (_timeRemaining <= 0)
            {
                _timeRemaining = 0;
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

    public void AddMoney(int amount)
    {
        _levelEarnings += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        int currentBalance = _startBalance + _levelEarnings;
        
        if (_textBalance != null)
        {
            _textBalance.text = $"{currentBalance:N0} ₽";
            _textBalance.color = currentBalance >= 0 ? _positiveColor : _negativeColor;
        }
    }

    private void UpdateTimerUI()
    {
        if (_timeRemaining <= 0)
        {
            _textTimer.text = "Время вышло";
            _sliderTimerProgress.value = 0;
        }
        else
        {
            int minutes = Mathf.FloorToInt(_timeRemaining / 60);
            int seconds = Mathf.FloorToInt(_timeRemaining % 60);
            _textTimer.text = $"{minutes:00}:{seconds:00}";
            _sliderTimerProgress.value = _timeRemaining / _levelDuration;
        }
    }

    private void EndLevel()
    {
        if (_isLevelEnded) return;
        _isLevelEnded = true;
        _isGameActive = false;
        
        bool isVictory = _levelEarnings >= _debtAmount;
        int finalBalance = _startBalance + _levelEarnings;
        
        if (_levelEndHandler != null)
        {
            _levelEndHandler.EndLevel(isVictory, finalBalance, finalBalance);
        }
        
        if (isVictory && Economy.Instance != null)
        {
            Economy.Instance.UnlockLevel(2);
            _gameState.currentLevel = 2;
        }
    }

    public void OnNextLevelClicked()
    {
        Time.timeScale = 1f;
        
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
    
    public void OnMainMenuClicked()
    {
        Time.timeScale = 1f;
        
        if (Audio.Instance != null)
        {
            Audio.Instance.FadeOutMusic(1f);
        }
    
        if (Fade.Instance != null)
        {
            Fade.Instance.FadeIn();
        }
    
        StartCoroutine(LoadMainMenuAfterDelay(1f));
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

    private IEnumerator LoadMainMenuAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneLoader.LoadMainMenu();
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}