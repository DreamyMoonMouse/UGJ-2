using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameStateSO _gameState;
    [SerializeField] private Button[] _levelButtons;
    [SerializeField] private TextMeshProUGUI _balanceText;
    [SerializeField] private Slider _volumeSlider;
    
    private void Start()
    {
        UpdateUI();
        _volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }
    
    private void UpdateUI()
    {
        _balanceText.text = $"{_gameState.currentMoney:N0} ₽";
        
        for (int i = 0; i < _levelButtons.Length; i++)
        {
            _levelButtons[i].interactable = i < _gameState.maxUnlockedLevel;
        }
    }
    
    public void StartLevel(int level)
    {
        _gameState.currentLevel = level;
        SceneLoader.LoadLetterScene(level);
    }
    
    public void ResetProgress()
    {
        _gameState.currentMoney = 0;
        _gameState.maxUnlockedLevel = 1;
        UpdateUI();
    }
    
    private void OnVolumeChanged(float value) => AudioListener.volume = value;
}