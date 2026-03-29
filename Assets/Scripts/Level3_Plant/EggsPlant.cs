using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EggsPlant : MonoBehaviour
{
    [SerializeField] private GameStateSO _gameState;
    [SerializeField] private GameSettingsSO _settings;
    [SerializeField] private TextMeshProUGUI _textBalance;
    [SerializeField] private TextMeshProUGUI _textTimer;
    [SerializeField] private Slider _sliderTimerProgress;
    [SerializeField] private GameObject _panelIntro;
    [SerializeField] private GameObject _panelEndWin;
    [SerializeField] private GameObject _panelEndLose;
    [SerializeField] private float _levelDuration = 180f;
    [SerializeField] private int _debtAmount = 100000;
    [SerializeField] private AudioClip _levelBGM;
    [SerializeField] private AudioClip _introMusic;
    [SerializeField] private FactoryAmbientSound _ambientSound;
    [SerializeField] private Color _positiveColor = Color.green;
    [SerializeField] private Color _negativeColor = Color.red;

    [Header("Level End")]
    [SerializeField] private LevelEndHandler _levelEndHandler;

    [Header("Spawners")]
    [SerializeField] private EggSpawner[] _spawners;
    
    [Header("Tutorial Hints")]
    [SerializeField] private TutorialHint[] _tutorialHints;

    private float _timeRemaining;
    private int _startBalance;
    private int _levelEarnings = 0;
    private int _finalBalance = 0;
    private bool _isGameActive = false;
    private bool _isPaused = false;
    private bool _isLevelEnded = false;
    private bool _isIntroActive = true;

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
        _panelEndWin.SetActive(false);
        _panelEndLose.SetActive(false);
        
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
        if (_isIntroActive) return; 
        
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
        _isIntroActive = false;
        _isGameActive = true;
    
        foreach (EggSpawner spawner in _spawners)
        {
            if (spawner != null)
            {
                spawner.StartSpawning();
            }
        }
        
        foreach (TutorialHint hint in _tutorialHints)
        {
            if (hint != null)
            {
                hint.StartHint();
            }
        }
    
        if (_ambientSound != null)
        {
            _ambientSound.StartAmbient();
        }
    
        if (Audio.Instance != null && _levelBGM != null)
        {
            Audio.Instance.PlayMusic(_levelBGM);
        }
        
        GooseGun[] gooseGuns = FindObjectsOfType<GooseGun>();
        
        foreach (GooseGun gun in gooseGuns)
        {
            gun.StartGame();
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
        
        foreach (EggSpawner spawner in _spawners)
        {
            if (spawner != null)
            {
                spawner.StopSpawning();
            }
        }
        
        foreach (TutorialHint hint in _tutorialHints)
        {
            if (hint != null)
            {
                hint.StopHint();
            }
        }
        
        if (_ambientSound != null)
        {
            _ambientSound.StopAmbient();
        }
        
        GooseGun gooseGun = FindObjectOfType<GooseGun>();
        
        if (gooseGun != null)
        {
            gooseGun.StopGame();
        }
        
        _finalBalance = _startBalance + _levelEarnings;
        bool isVictory = _finalBalance >= 0;
        
        if (_levelEndHandler != null)
        {
            _levelEndHandler.EndLevel(isVictory, _levelEarnings, _finalBalance);
        }
        
        if (isVictory && Economy.Instance != null)
        {
            Economy.Instance.UnlockLevel(1);
            _gameState.currentLevel = 1;
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
    
        StartCoroutine(LoadEndSceneAfterDelay(1f));
    }

    public void OnRetryClicked()
    {
        Time.timeScale = 1f;
        
        if (_ambientSound != null)
        {
            _ambientSound.StopAmbient();
        }
        
        int balanceWithoutDebt = _startBalance + _debtAmount;
        int balanceWithEarnings = balanceWithoutDebt + _levelEarnings;
        
        if (Economy.Instance != null)
        {
            Economy.Instance.SetBalance(balanceWithEarnings);
        }
        
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

    private IEnumerator LoadEndSceneAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        
        if (_finalBalance > 0)
        {
            SceneLoader.LoadVictory();
        }
        else
        {
            SceneLoader.LoadLoseVictory();
        }
    }

    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneLoader.LoadLevel(3);
    }

    private IEnumerator LoadLetterAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneLoader.LoadLetterScene(1);
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