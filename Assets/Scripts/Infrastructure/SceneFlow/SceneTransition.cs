using UnityEngine;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private CanvasGroup _fadePanel;
    [SerializeField] private float _fadeDuration = 1f;
    
    public IEnumerator FadeIn()
    {
        _fadePanel.gameObject.SetActive(true);
        float elapsed = 0f;
        
        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            _fadePanel.alpha = elapsed / _fadeDuration;
            yield return null;
        }
    }
    
    public IEnumerator FadeOut()
    {
        float elapsed = 0f;
        
        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            _fadePanel.alpha = 1f - (elapsed / _fadeDuration);
            yield return null;
        }
        
        _fadePanel.gameObject.SetActive(false);
    }
}