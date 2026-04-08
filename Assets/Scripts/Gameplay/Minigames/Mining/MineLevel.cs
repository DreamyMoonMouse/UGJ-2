using UnityEngine;

public class MineLevel : MonoBehaviour
{
    [SerializeField] private LevelConfigSO _config;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Debt _debt;
    [SerializeField] private Expenses _expenses;
    [SerializeField] private LevelTimer _timer;
    [SerializeField] private VictoryCondition _victory;
    [SerializeField] private LevelResultUI _resultUI;
    [SerializeField] private LevelUnlock _levelUnlock;
    [SerializeField] private GameStateSO _gameState;
    
    private void Start()
    {
        InitializeLevel();
        _timer.OnTimeEnded.AddListener(OnTimeEnded);
        _victory.OnConditionChecked.AddListener(OnConditionChecked);
        _timer.StartTimer();
    }
    
    private void InitializeLevel()
    {
        _debt.Set(_config.debtAmount);
        _timer.SetDuration(_config.duration);
        _victory.SetTarget(0);
        _wallet.Set(_gameState.currentMoney - _config.debtAmount);
    }
    
    private void OnTimeEnded()
    {
        _victory.CheckCondition();
    }
    
    private void OnConditionChecked(bool isVictory)
    {
        if (isVictory)
            HandleVictory();
        else
            HandleDefeat();
    }
    
    private void HandleVictory()
    {
        int finalBalance = _wallet.Balance;
        int expenses = _expenses.Calculate(finalBalance);
        int netBalance = finalBalance - expenses;
        
        _wallet.Set(netBalance);
        _gameState.currentMoney = netBalance;
        
        _levelUnlock.UnlockNextLevel();
        _resultUI.ShowVictory(finalBalance, expenses, netBalance);
    }
    
    private void HandleDefeat()
    {
        _resultUI.ShowDefeat();
    }
    
    public void OnRetry()
    {
        _wallet.Set(_gameState.currentMoney);
        InitializeLevel();
        _timer.StartTimer();
    }
    
    public void OnContinue()
    {
        SceneLoader.LoadLetterScene(_gameState.currentLevel + 1);
    }
}