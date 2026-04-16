using UnityEngine;
using UnityEngine.UI;

public class LampBlink : MonoBehaviour
{
    [SerializeField] private Image _lampImage;
    [SerializeField] private float _blinkInterval = 0.5f;
    [SerializeField] private Color _onColor = Color.yellow;
    [SerializeField] private Color _offColor = Color.gray;
    
    private float _timer;
    
    private void Update()
    {
        _timer += Time.deltaTime;
        
        if (_timer >= _blinkInterval)
        {
            _timer = 0f;
            _lampImage.color = _lampImage.color == _onColor ? _offColor : _onColor;
        }
    }
}