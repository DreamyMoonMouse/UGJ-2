using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private LevelConfigSO config;
    public event Action OnTimeEnded;
    
    private float _remaining;
    
    public void StartTimer() => _remaining = config.duration;
    public float Remaining => _remaining;
    public float Progress => _remaining / config.duration;
    
    private void Update()
    {
        if (_remaining > 0)
        {
            _remaining -= Time.deltaTime;
            if (_remaining <= 0) OnTimeEnded?.Invoke();
        }
    }
}