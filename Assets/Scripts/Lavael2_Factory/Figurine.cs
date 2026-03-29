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

    private Factory _factory;
    private bool _isPushed = false;
    private bool _isProcessed = false;
    private bool _isCracked = false;
    private bool _isDragging = false;
    private float _value;
    private Rigidbody2D _rb;
    private Vector3 _moveDirection;
    private Collider2D _collider;
    private Camera _mainCamera;

    public bool IsCracked => _isCracked;
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
        }
        
        if (_rb != null)
        {
            _rb.gravityScale = 0f;
            _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    public void Initialize(Factory factory, bool isCracked, FigurineData data) 
    {
        _factory = factory;
        _isCracked = isCracked;
        _data = data; 
    
        if (_data != null)
        {
            _value = _data.baseValue;
        }
    
        if (_spriteRenderer != null)
        {
            if (isCracked && _data != null && _data.spriteCrackedVariants != null && _data.spriteCrackedVariants.Length > 0)
            {
                _spriteRenderer.sprite = _data.spriteCrackedVariants[Random.Range(0, _data.spriteCrackedVariants.Length)];
            }
            else if (_data != null && _data.spriteGood != null)
            {
                _spriteRenderer.sprite = _data.spriteGood;
            }
        
            if (_data != null && _data.canHaveColor)
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
        
        if (Audio.Instance != null && _pushSound != null)
        {
            Audio.Instance.PlaySfx(_pushSound);
        }
        
        int reward = _isCracked ? 50 : -50;
        if (_factory != null)
        {
            _factory.AddMoney(reward);
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
        
        int reward = _isCracked ? -100 : 100;
        if (_factory != null)
        {
            _factory.AddMoney(reward);
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