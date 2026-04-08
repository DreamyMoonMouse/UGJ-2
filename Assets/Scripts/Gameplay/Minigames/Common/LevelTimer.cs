using UnityEngine;
using UnityEngine.Events;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private float _duration;
    
    public UnityEvent OnTimeEnded;
    public UnityEvent<float> OnProgressChanged;
    
    private float _remaining;
    private bool _isRunning;
    
    public float Remaining => _remaining;
    public float Progress => _remaining / _duration;
    
    public void SetDuration(float duration) => _duration = duration;
    
    public void StartTimer()
    {
        _remaining = _duration;
        _isRunning = true;
    }
    
    public void StopTimer() => _isRunning = false;
    
    private void Update()
    {
        if (!_isRunning) return;
        
        _remaining -= Time.deltaTime;
        OnProgressChanged?.Invoke(Progress);
        
        if (_remaining <= 0)
        {
            _remaining = 0;
            _isRunning = false;
            OnTimeEnded?.Invoke();
        }
    }
}