using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private LevelTimer _timer;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Slider _slider;
    
    private void OnEnable()
    {
        _timer.OnProgressChanged.AddListener(UpdateDisplay);
    }
    
    private void OnDisable()
    {
        _timer.OnProgressChanged.RemoveListener(UpdateDisplay);
    }
    
    private void UpdateDisplay(float progress)
    {
        int seconds = Mathf.CeilToInt(_timer.Remaining);
        _text.text = $"{seconds / 60:00}:{seconds % 60:00}";
        
        if (_slider != null)
            _slider.value = progress;
    }
}