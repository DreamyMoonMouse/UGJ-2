using UnityEngine;

public class FlagAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform _flagTransform;
    [SerializeField] private float _waveSpeed = 2f;
    [SerializeField] private float _waveAmount = 10f;
    
    private void Update()
    {
        float wave = Mathf.Sin(Time.time * _waveSpeed) * _waveAmount;
        _flagTransform.rotation = Quaternion.Euler(0f, 0f, wave);
    }
}