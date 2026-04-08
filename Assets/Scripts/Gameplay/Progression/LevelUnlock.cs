using UnityEngine;

public class LevelUnlock : MonoBehaviour
{
    [SerializeField] private GameStateSO _gameState;
    
    public void UnlockNextLevel()
    {
        if (_gameState.currentLevel < _gameState.maxUnlockedLevel) return;
        
        _gameState.maxUnlockedLevel = _gameState.currentLevel + 1;
        SaveProgress();
    }
    
    public bool IsLevelUnlocked(int level) => level <= _gameState.maxUnlockedLevel;
    
    private void SaveProgress()
    {
        PlayerPrefs.SetInt("MaxUnlockedLevel", _gameState.maxUnlockedLevel);
        PlayerPrefs.Save();
    }
    
    public void LoadProgress()
    {
        _gameState.maxUnlockedLevel = PlayerPrefs.GetInt("MaxUnlockedLevel", 1);
    }
}