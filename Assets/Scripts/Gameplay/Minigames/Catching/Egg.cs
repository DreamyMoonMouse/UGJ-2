using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioClip _catchSound;
    [SerializeField] private AudioClip _breakSound;
    [SerializeField] private AudioClip _attractSound;
    [SerializeField] private PhysicsMaterial2D _rollPhysics;
    [SerializeField] private float _fadeDuration = 0.3f;
    [SerializeField] private FloatingText _floatingTextPrefab;

    private EggsPlant _eggsPlant;
    private bool _isCaught = false;
    private bool _isProcessed = false;
    private bool _isAttracted = false;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private EggData _data;
    private Vector3 _attractionTarget;
    private float _attractionForce;

    public float Value => _data.baseValue;
    public bool IsAttracted => _isAttracted;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    public void Initialize(EggsPlant eggsPlant, EggData data)
    {
        _eggsPlant = eggsPlant;
        _data = data;
        
        if (_spriteRenderer != null && _data != null)
        {
            _spriteRenderer.sprite = _data.spriteGood;
            float eggScale = _data.scale > 0 ? _data.scale : 1f;
            transform.localScale = Vector3.one * eggScale;
            
            Color color = _spriteRenderer.color;
            color.a = 1f;
            _spriteRenderer.color = color;
        }
        
        if (_rb != null && _data != null)
        {
            _rb.gravityScale = 1f;
            _rb.mass = _data.mass;
            
            if (_rollPhysics != null)
            {
                _rb.sharedMaterial = _rollPhysics;
            }
        }
    }

    private void Update()
    {
        if (_isCaught || _isProcessed) return;
        
        if (_isAttracted && _rb != null)
        {
            Vector3 direction = (_attractionTarget - transform.position).normalized;
            _rb.linearVelocity = direction * _attractionForce;
            
            float distance = Vector2.Distance(transform.position, _attractionTarget);
            if (distance < 0.5f)
            {
                Catch();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isCaught || _isProcessed) return;

        if (other.CompareTag("Basket"))
        {
            Catch();
        }
        
        if (other.CompareTag("Ground"))
        {
            Break();
        }
    }

    public void StartAttraction(Vector3 target, float force)
    {
        if (_isAttracted || _isCaught || _isProcessed) return;
        
        _isAttracted = true;
        _attractionTarget = target;
        _attractionForce = force;
        
        if (_rb != null)
        {
            _rb.gravityScale = 0f;
        }
        
        if (Audio.Instance != null && _attractSound != null)
        {
            Audio.Instance.PlaySfx(_attractSound);
        }
    }

    public void Catch()
    {
        if (_isCaught || _isProcessed) return;
        
        _isCaught = true;
        _isProcessed = true;
        
        int reward = Mathf.FloorToInt(_data.baseValue);
        
        if (Audio.Instance != null && _catchSound != null)
        {
            Audio.Instance.PlaySfx(_catchSound);
        }
        
        if (_eggsPlant != null)
        {
            _eggsPlant.AddMoney(reward);
        }
        
        if (_floatingTextPrefab != null)
        {
            ShowFloatingText(reward);
        }
        
        StartCoroutine(FadeAndDestroy());
    }

    public void Break()
    {
        if (_isCaught || _isProcessed) return;
        
        _isProcessed = true;
        
        int penalty = -Mathf.FloorToInt(_data.baseValue / 2);
        
        if (Audio.Instance != null && _breakSound != null)
        {
            Audio.Instance.PlaySfx(_breakSound);
        }
        
        if (_eggsPlant != null)
        {
            _eggsPlant.AddMoney(penalty);
        }
        
        if (_data != null && _data.spriteBroken != null && _spriteRenderer != null)
        {
            _spriteRenderer.sprite = _data.spriteBroken;
            transform.localScale = Vector3.one * (_data.scale > 0 ? _data.scale : 1f);
        }
        
        if (_collider != null)
        {
            _collider.enabled = false;
        }
        
        if (_rb != null)
        {
            _rb.simulated = false;
        }
        
        if (_floatingTextPrefab != null)
        {
            ShowFloatingText(penalty);
        }
        
        StartCoroutine(FadeAndDestroy());
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
        if (_spriteRenderer != null)
        {
            float elapsed = 0f;
            Color startColor = _spriteRenderer.color;
            startColor.a = 1f;
            
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