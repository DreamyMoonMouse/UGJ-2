using UnityEngine;
using System.Collections;

public class LampBlink : MonoBehaviour
{
    [SerializeField] private GameObject _lightOn;
    [SerializeField] private GameObject _lightOff;
    [SerializeField] private AudioClip _blinkSound;
    [SerializeField] private float _blinkInterval = 25f;
    [SerializeField] private float _blinkDuration = 0.15f;

    private void Start()
    {
        if (_lightOn != null) _lightOn.SetActive(true);
        if (_lightOff != null) _lightOff.SetActive(false);
        
        StartCoroutine(BlinkCycle());
    }

    private IEnumerator BlinkCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(_blinkInterval);
            
            for (int i = 0; i < 3; i++)
            {
                yield return StartCoroutine(Blink());
                
                if (i < 2)
                {
                    yield return new WaitForSeconds(_blinkDuration);
                }
            }
        }
    }

    private IEnumerator Blink()
    {
        if (_lightOn != null) _lightOn.SetActive(false);
        if (_lightOff != null) _lightOff.SetActive(true);
        
        if (Audio.Instance != null && _blinkSound != null)
        {
            Audio.Instance.PlaySfx(_blinkSound);
        }
        
        yield return new WaitForSeconds(_blinkDuration);
        
        if (_lightOn != null) _lightOn.SetActive(true);
        if (_lightOff != null) _lightOff.SetActive(false);
    }
}