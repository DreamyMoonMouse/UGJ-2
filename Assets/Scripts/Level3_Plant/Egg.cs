using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioClip _catchSound;
    [SerializeField] private AudioClip _breakSound;
    [SerializeField] private AudioClip _attractSound;
    [SerializeField] private PhysicsMaterial2D _rollPhysics;

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
        
        if (Audio.Instance != null && _catchSound != null)
        {
            Audio.Instance.PlaySfx(_catchSound);
        }
        
        if (_eggsPlant != null)
        {
            _eggsPlant.AddMoney(Mathf.FloorToInt(_data.baseValue));
        }
        
        Destroy(gameObject);
    }

    public void Break()
    {
        if (_isCaught || _isProcessed) return;
        
        _isProcessed = true;
        
        if (Audio.Instance != null && _breakSound != null)
        {
            Audio.Instance.PlaySfx(_breakSound);
        }
        
        if (_eggsPlant != null)
        {
            _eggsPlant.AddMoney(-Mathf.FloorToInt(_data.baseValue / 2));
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
        
        Destroy(gameObject, 2f);
    }
}