using UnityEngine;

public class SplashDataLoader : MonoBehaviour
{
    [SerializeField] private PlayerPrefsStorage _storage;
    [SerializeField] private GameStateSO _gameState;
    
    public void LoadAllData()
    {
        LoadProgression();
        LoadEconomy();
        LoadSettings();
    }
    
    private void LoadProgression()
    {
        _gameState.maxUnlockedLevel = _storage.LoadInt("MaxUnlockedLevel", 1);
        _gameState.currentLevel = _storage.LoadInt("CurrentLevel", 1);
    }
    
    private void LoadEconomy()
    {
        _gameState.currentMoney = _storage.LoadInt("CurrentMoney", 0);
    }
    
    private void LoadSettings()
    {
        AudioListener.volume = _storage.LoadFloat("MasterVolume", 1f);
        Screen.fullScreen = _storage.LoadInt("Fullscreen", 1) == 1;
    }
}