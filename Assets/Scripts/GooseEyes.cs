using UnityEngine;
using System.Collections;

public class GooseEyes : MonoBehaviour
{
    [SerializeField] private GameObject _eyesOpen;
    [SerializeField] private GameObject _eyesClosed;
    [SerializeField] private AudioClip _blinkSound;
    [SerializeField] private float _blinkInterval = 30f;
    [SerializeField] private float _blinkDuration = 0.1f;

    private void Start()
    {
        _eyesOpen.SetActive(true);
        _eyesClosed.SetActive(false);
        
        StartCoroutine(BlinkCycle());
    }

    private IEnumerator BlinkCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(_blinkInterval);
            
            yield return StartCoroutine(Blink());
            
            yield return new WaitForSeconds(_blinkInterval);
            
            yield return StartCoroutine(Blink());
            yield return new WaitForSeconds(_blinkDuration);
            yield return StartCoroutine(Blink());
        }
    }

    private IEnumerator Blink()
    {
        _eyesOpen.SetActive(false);
        _eyesClosed.SetActive(true);
        
        if (Audio.Instance != null && _blinkSound != null)
        {
            Audio.Instance.PlaySfx(_blinkSound);
        }
        
        yield return new WaitForSeconds(_blinkDuration);
        _eyesOpen.SetActive(true);
        _eyesClosed.SetActive(false);
    }
}