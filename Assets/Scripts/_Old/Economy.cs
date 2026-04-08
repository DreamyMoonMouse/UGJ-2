using UnityEngine;

public class Economy : MonoBehaviour
{
    public static Economy Instance { get; private set; }

    [SerializeField] private GameStateSO _gameState;
    [SerializeField] private int _startBalance = 1000;

    private int _globalBalance = 0;

    public int GlobalBalance => _globalBalance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadBalance();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBalance(int balance)
    {
        _globalBalance = balance;
        SaveBalance();
    }

    public void AddToBalance(int amount)
    {
        _globalBalance += amount;
        SaveBalance();
    }

    public void ResetBalance()
    {
        _globalBalance = _startBalance;
        
        if (_gameState != null)
        {
            _gameState.maxUnlockedLevel = 1;
            _gameState.currentLevel = 1;
            _gameState.selectedLevel = 1;
        }
        
        SaveBalance();
    }

    public int GetStartingBalanceForLevel(int debt)
    {
        return _globalBalance - debt;
    }

    public int CalculateExpenses(int income)
    {
        int foodExpense = Mathf.Max(50000, Mathf.FloorToInt(income * 0.3f));
        int rentExpense = Mathf.Max(50000, Mathf.FloorToInt(income * 0.3f));
        int utilityExpense = Mathf.Max(50000, Mathf.FloorToInt(income * 0.3f));
        
        return foodExpense + rentExpense + utilityExpense;
    }

    public int CalculateFinalBalance(int finalBalance)
    {
        int expenses = CalculateExpenses(finalBalance);
        return finalBalance - expenses;
    }

    public void UnlockLevel(int level)
    {
        if (_gameState != null)
        {
            _gameState.UnlockLevel(level);
            _gameState.currentLevel = level;
        }
    }

    private void SaveBalance()
    {
        PlayerPrefs.SetInt("GlobalBalance", _globalBalance);
        PlayerPrefs.Save();
    }

    private void LoadBalance()
    {
        _globalBalance = PlayerPrefs.GetInt("GlobalBalance", 0);
    }
}