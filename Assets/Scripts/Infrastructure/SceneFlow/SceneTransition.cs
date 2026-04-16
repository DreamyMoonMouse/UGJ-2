using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private Image _fadePanel;
    [SerializeField] private float _fadeDuration = 1f;
    
    private void Awake()
    {
        if (_fadePanel != null)
        {
            Color color = _fadePanel.color;
            color.a = 0f;
            _fadePanel.color = color;
            _fadePanel.gameObject.SetActive(false);
        }
    }
    
    public IEnumerator FadeIn()
    {
        _fadePanel.gameObject.SetActive(true);
        float elapsed = 0f;
        Color originalColor = _fadePanel.color;
        
        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = elapsed / _fadeDuration;
            _fadePanel.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        
        _fadePanel.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
    }
    
    public IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color originalColor = _fadePanel.color;
        
        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - (elapsed / _fadeDuration);
            _fadePanel.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        
        _fadePanel.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        _fadePanel.gameObject.SetActive(false);
    }
}