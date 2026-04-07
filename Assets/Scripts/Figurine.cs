using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Figurine : MonoBehaviour
{
    [SerializeField] private FigurineData _data;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _pushForce = 5f;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private AudioClip _pushSound;
    [SerializeField] private AudioClip _endZoneSound;
    [SerializeField] private AudioClip _shredderSound;
    [SerializeField] private AudioClip _gooseEatSound;
    [SerializeField] private FloatingText _floatingTextPrefab;

    private Factory _factory;
    private bool _isPushed = false;
    private bool _isProcessed = false;
    private bool _isDragging = false;
    private bool _isBadItem = false;
    private bool _isEdible = false;
    private float _value;
    private Rigidbody2D _rb;
    private Vector3 _moveDirection;
    private Collider2D _collider;
    private Camera _mainCamera;

    public bool IsBadItem => _isBadItem;
    public bool IsEdible => _isEdible;
    public float Value => _value;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _mainCamera = Camera.main;
        _moveDirection = Vector3.left;
        
        if (_data != null)
        {
            _value = _data.baseValue;
            _isBadItem = _data.isBadItem;
            _isEdible = _data.isEdible;
        }
        
        if (_rb != null)
        {
            _rb.gravityScale = 0f;
            _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    public void Initialize(Factory factory, FigurineData data, bool forceBadVariant = false) 
    {
        _factory = factory;
        _data = data; 
    
        if (_data != null)
        {
            _value = _data.baseValue;
            _isBadItem = _data.isBadItem || forceBadVariant;
            _isEdible = _data.isEdible;
        }
    
        if (_spriteRenderer != null && _data != null)
        {
            if (_isBadItem)
            {
                if (_data.badSpriteVariants != null && _data.badSpriteVariants.Length > 0)
                {
                    _spriteRenderer.sprite = _data.badSpriteVariants[Random.Range(0, _data.badSpriteVariants.Length)];
                }
                else if (_data.itemSprite != null)
                {
                    _spriteRenderer.sprite = _data.itemSprite;
                }
            }
            else
            {
                if (_data.itemSprite != null)
                {
                    _spriteRenderer.sprite = _data.itemSprite;
                }
            }
        
            if (_data.canHaveColor)
            {
                Color randomColor = new Color(
                    Random.value,
                    Random.value,
                    Random.value,
                    Random.Range(_data.minAlpha, _data.maxAlpha)
                );
                _spriteRenderer.color = randomColor;
            }
        }
    }

    private void Update()
    {
        if (_isPushed || _isProcessed || _isDragging) return;
        
        transform.position += _moveDirection * _moveSpeed * Time.deltaTime;
    }

    private void OnMouseDown()
    {
        if (_isPushed || _isProcessed) return;
        
        StartDrag();
    }

    private void OnMouseDrag()
    {
        if (!_isDragging) return;
        
        Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }

    private void OnMouseUp()
    {
        if (!_isDragging) return;
        
        StopDrag();
    }

    private void StartDrag()
    {
        _isDragging = true;
        
        if (_collider != null)
        {
            _collider.enabled = false;
        }
        
        if (_rb != null)
        {
            _rb.gravityScale = 0f;
            _rb.linearVelocity = Vector2.zero;
        }
    }

    private void StopDrag()
    {
        _isDragging = false;
        
        if (_collider != null)
        {
            _collider.enabled = true;
        }
        
        if (_rb != null)
        {
            _rb.gravityScale = 1f;
        }
        
        CheckIfInBox();
    }

    private void CheckIfInBox()
    {
        Collider2D[] boxHits = Physics2D.OverlapCircleAll(transform.position, 1.5f);
        
        foreach (Collider2D hit in boxHits)
        {
            if (hit.CompareTag("Ground"))
            {
                PushToShredder();
                return;
            }
        }
    }

    public void PushToShredder()
    {
        if (_isPushed || _isProcessed) return;
        
        _isPushed = true;
        _isProcessed = true;
        
        if (_rb != null)
        {
            _rb.gravityScale = 1f;
            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(Vector2.down * _pushForce, ForceMode2D.Impulse);
        }
        
        if (Audio.Instance != null && _shredderSound != null)
        {
            Audio.Instance.PlaySfx(_shredderSound);
        }
        
        int reward = 0;
        
        if (_isBadItem)
        {
            reward = Mathf.FloorToInt(_value * 0.25f);
        }
        else if (_isEdible)
        {
            reward = -Mathf.FloorToInt(_value * 0.25f);
        }
        else
        {
            reward = -Mathf.FloorToInt(_value * 0.5f);
        }
        
        if (_factory != null)
        {
            _factory.AddMoney(reward);
        }
        
        if (_floatingTextPrefab != null)
        {
            ShowFloatingText(reward);
        }
        
        StartCoroutine(FadeAndDestroy());
    }

    public void ProcessAtEnd()
    {
        if (_isProcessed) return;
        _isProcessed = true;
        
        if (Audio.Instance != null && _endZoneSound != null)
        {
            Audio.Instance.PlaySfx(_endZoneSound);
        }
        
        int reward = 0;
        
        if (_isBadItem)
        {
            reward = -Mathf.FloorToInt(_value * 0.5f);
        }
        else if (_isEdible)
        {
            reward = -Mathf.FloorToInt(_value);
        }
        else
        {
            reward = Mathf.FloorToInt(_value);
        }
        
        if (_factory != null)
        {
            _factory.AddMoney(reward);
        }
        
        if (_floatingTextPrefab != null)
        {
            ShowFloatingText(reward);
        }
        
        Destroy(gameObject);
    }

    public void FeedToGoose()
    {
        if (_isProcessed || !_isEdible) return;
        
        _isProcessed = true;
        
        int reward = Mathf.FloorToInt(_value * 5f);
        
        if (Audio.Instance != null && _gooseEatSound != null)
        {
            Audio.Instance.PlaySfx(_gooseEatSound);
        }
        
        if (_factory != null)
        {
            _factory.AddMoney(reward);
        }
        
        if (_floatingTextPrefab != null)
        {
            ShowFloatingText(reward);
        }
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isDragging || _isProcessed) return;

        if (other.CompareTag("Killzone"))
        {
            ProcessAtEnd();
        }
    
        if (other.CompareTag("Ground"))
        {
            if (_collider != null)
            {
                _collider.enabled = false;
            }
        
            if (_rb != null)
            {
                _rb.gravityScale = 1f;
            }
        
            PushToShredder();
        }

        if (other.CompareTag("GooseFeedingZone"))
        {
            if (_isEdible)
            {
                FeedToGoose();
            }
        }
    }

    private void ShowFloatingText(int amount)
    {
        FloatingText text = Instantiate(_floatingTextPrefab, transform.position, Quaternion.identity);
        
        string textString = amount >= 0 ? $"+{amount:N0} ₽" : $"{amount:N0} ₽";
        bool isPositive = amount >= 0;
        
        text.Initialize(textString, isPositive, transform.position);
    }

    private IEnumerator FadeAndDestroy()
    {
        yield return new WaitForSeconds(2.5f);
        
        if (_spriteRenderer != null)
        {
            float elapsed = 0f;
            Color startColor = _spriteRenderer.color;
            
            while (elapsed < _fadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / _fadeDuration);
                _spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                yield return null;
            }
            
            _spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        }
        
        Destroy(gameObject);
    }
}