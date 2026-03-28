using UnityEngine;
using System.Collections;

public class TutorialHint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _appearDelay = 0f;
    [SerializeField] private float _visibleDuration = 15f;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private float _alphaMin = 0.4f;
    [SerializeField] private float _alphaMax = 1f;
    [SerializeField] private float _blinkSpeed = 2f;
    [SerializeField] private bool _scaleAnimation = false;
    [SerializeField] private Vector2 _scaleRange = new Vector2(0.9f, 1.1f);
    [SerializeField] private float _scaleSpeed = 2f;
    [SerializeField] private bool _moveAnimation = false;
    [SerializeField] private Vector2 _moveRange = new Vector2(0f, 0.3f);
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private Vector3 _originalPosition;

    private bool _isActive = false;
    private float _elapsedTime = 0f;
    private float _appearTimer = 0f;

    private void Awake()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        _originalPosition = transform.position;
        
        Color color = _spriteRenderer.color;
        color.a = 0f;
        _spriteRenderer.color = color;
        
        gameObject.SetActive(false);
    }

    public void StartHint()
    {
        gameObject.SetActive(true);
        _isActive = true;
        _elapsedTime = 0f;
        _appearTimer = _appearDelay;
        
        StartCoroutine(FadeIn());
    }

    public void StopHint()
    {
        _isActive = false;
        StartCoroutine(FadeOut());
    }

    private void Update()
    {
        if (!_isActive) return;
        
        if (_appearTimer > 0)
        {
            _appearTimer -= Time.deltaTime;
            return;
        }
        
        _elapsedTime += Time.deltaTime;
        
        if (_elapsedTime >= _visibleDuration)
        {
            StopHint();
            return;
        }
        
        float blinkAlpha = Mathf.Lerp(_alphaMin, _alphaMax, (Mathf.Sin(_elapsedTime * _blinkSpeed) + 1f) / 2f);
        
        Color color = _spriteRenderer.color;
        color.a = blinkAlpha;
        _spriteRenderer.color = color;
        
        if (_scaleAnimation)
        {
            float scale = Mathf.Lerp(_scaleRange.x, _scaleRange.y, (Mathf.Sin(_elapsedTime * _scaleSpeed) + 1f) / 2f);
            transform.localScale = Vector3.one * scale;
        }
        
        if (_moveAnimation)
        {
            float moveOffset = Mathf.Sin(_elapsedTime * _moveSpeed) * _moveRange.y;
            transform.position = _originalPosition + new Vector3(0, moveOffset, 0);
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color color = _spriteRenderer.color;
        color.a = 0f;
        
        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0f, _alphaMax, elapsed / _fadeDuration);
            _spriteRenderer.color = color;
            yield return null;
        }
        
        color.a = _alphaMax;
        _spriteRenderer.color = color;
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color color = _spriteRenderer.color;
        
        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(_alphaMax, 0f, elapsed / _fadeDuration);
            _spriteRenderer.color = color;
            yield return null;
        }
        
        color.a = 0f;
        _spriteRenderer.color = color;
        gameObject.SetActive(false);
    }
}