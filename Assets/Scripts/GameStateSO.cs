using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameState", menuName = "Game/State")]
public class GameStateSO : ScriptableObject
{
    [Header("Прогресс")]
    public int currentLevel = 1;
    public int currentMoney = 0;
    public int debtAmount = 0;
    
    [Header("Разблокировка уровней")]
    public int maxUnlockedLevel = 1; 
    public int selectedLevel = 1;   
    
    [Header("События")]
    public UnityEvent<int> OnMoneyChanged;
    public UnityEvent<int> OnLevelChanged;
    public UnityEvent OnPlayerDied;
    public UnityEvent OnLevelComplete;

    public void AddMoney(int amount) {
        currentMoney += amount;
        OnMoneyChanged?.Invoke(currentMoney);
    }

    public void SetLevel(int level) {
        currentLevel = level;
        OnLevelChanged?.Invoke(level);
    }

    public void UnlockLevel(int level) {
        if (level > maxUnlockedLevel) {
            maxUnlockedLevel = level;
        }
    }

    public void ResetState() {
        currentLevel = 1;
        currentMoney = 0;
        debtAmount = 0;
    }
}