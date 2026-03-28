using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GooseEasterEgg : MonoBehaviour
{
    [SerializeField] private AudioClip _easterEggSound;
    [SerializeField] private GameObject _beakClosed;
    [SerializeField] private GameObject _beakOpen;
    [SerializeField] private Transform _gooseTransform;
    [SerializeField] private int _clicksNeeded = 7;
    [SerializeField] private float _resetTime = 3f;
    [SerializeField] private float _beakOpenDuration = 0.2f;
    [SerializeField] private float _shakeDuration = 0.5f;
    [SerializeField] private float _shakeIntensity = 0.3f;

    private int _clickCount = 0;
    private float _lastClickTime = 0f;
    private bool _isAnimating = false;
    private Vector3 _originalPosition;

    void Start()
    {
        if (_gooseTransform == null)
        {
            _gooseTransform = transform;
        }
        
        _originalPosition = _gooseTransform.position;
        
        if (_beakClosed != null) _beakClosed.SetActive(true);
        if (_beakOpen != null) _beakOpen.SetActive(false);
    }

    void Update()
    {
        if (_isAnimating) return;

        if (Time.time - _lastClickTime > _resetTime)
        {
            _clickCount = 0;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                _clickCount++;
                _lastClickTime = Time.time;
                
                StartCoroutine(ShakeOnTick());

                if (_clickCount >= _clicksNeeded)
                {
                    TriggerEasterEgg();
                    _clickCount = 0;
                }
            }
        }
    }

    private IEnumerator ShakeOnTick()
    {
        float elapsed = 0f;
        Vector3 startPos = _gooseTransform.position;
        
        while (elapsed < 0.1f)
        {
            elapsed += Time.deltaTime;
            float offsetX = Random.Range(-_shakeIntensity, _shakeIntensity);
            float offsetY = Random.Range(-_shakeIntensity, _shakeIntensity);
            _gooseTransform.position = startPos + new Vector3(offsetX, offsetY, 0);
            yield return null;
        }
        
        _gooseTransform.position = startPos;
    }

    private void TriggerEasterEgg()
    {
        _isAnimating = true;
        StartCoroutine(PlayEasterEgg());
    }

    private IEnumerator PlayEasterEgg()
    {
        if (Audio.Instance != null && _easterEggSound != null)
        {
            Audio.Instance.PlaySfx(_easterEggSound);
        }
        
        StartCoroutine(ShakeGoose());

        for (int i = 0; i < 2; i++)
        {
            _beakClosed.SetActive(false);
            _beakOpen.SetActive(true);
            
            yield return new WaitForSeconds(_beakOpenDuration);
            
            _beakClosed.SetActive(true);
            _beakOpen.SetActive(false);
            
            yield return new WaitForSeconds(_beakOpenDuration);
        }

        _isAnimating = false;
    }

    private IEnumerator ShakeGoose()
    {
        float elapsed = 0f;
        Vector3 startPos = _gooseTransform.position;
        
        while (elapsed < _shakeDuration)
        {
            elapsed += Time.deltaTime;
            float offsetX = Random.Range(-_shakeIntensity, _shakeIntensity);
            float offsetY = Random.Range(-_shakeIntensity, _shakeIntensity);
            _gooseTransform.position = startPos + new Vector3(offsetX, offsetY, 0);
            yield return null;
        }
        
        _gooseTransform.position = startPos;
    }
}