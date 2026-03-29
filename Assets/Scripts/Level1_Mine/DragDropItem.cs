using UnityEngine;
using UnityEngine.InputSystem;

public class DragDropItem : MonoBehaviour
{
    [Header("Физика предмета")]
    [SerializeField] private Rigidbody2D itemRigidbody;
    [SerializeField] private float dragSmoothness = 10f;

    [Header("Floating Text")]
    [SerializeField] private FloatingText _floatingTextPrefab;

    [Header("Типы предметов (для Ore)")]
    [SerializeField] private ItemData[] _itemTypes;

    [Header("Данные предмета")]
    private ItemData _itemData;
    private float _itemValue;
    private float _itemWeight;

    private Mine _mineController;
    private Camera _mainCamera;
    private bool _isDragging = false;
    private bool _isCollected = false;
    private SpriteRenderer _spriteRenderer;
    private bool _isMouseOver = false;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (itemRigidbody == null)
        {
            itemRigidbody = GetComponent<Rigidbody2D>();
        }
        
        if (itemRigidbody != null)
        {
            itemRigidbody.gravityScale = 0.5f;
            itemRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    private void Start()
    {
        SetupCollider();
        
        if (_itemData == null && _itemTypes != null && _itemTypes.Length > 0)
        {
            RandomizeItemType();
        }
    }

    private void SetupCollider()
    {
        if (_spriteRenderer != null && _spriteRenderer.sprite != null)
        {
            CircleCollider2D collider = GetComponent<CircleCollider2D>();
            if (collider == null)
            {
                collider = gameObject.AddComponent<CircleCollider2D>();
            }
            
            float spriteSize = Mathf.Max(_spriteRenderer.sprite.bounds.size.x, _spriteRenderer.sprite.bounds.size.y);
            collider.radius = spriteSize / 2f;
            collider.offset = _spriteRenderer.sprite.bounds.center;
        }
    }

    public void Initialize(Mine mineController, ItemData data)
    {
        _mineController = mineController;
        _itemData = data;
        
        if (_itemData != null)
        {
            _itemValue = _itemData.value;
            _itemWeight = _itemData.weight;
            
            if (_spriteRenderer != null && _itemData.itemSprite != null)
            {
                _spriteRenderer.sprite = _itemData.itemSprite;
            }
            
            if (itemRigidbody != null)
            {
                itemRigidbody.mass = _itemData.weight;
            }
        }
    }

    private void RandomizeItemType()
    {
        if (_itemTypes == null || _itemTypes.Length == 0)
        {
            _itemValue = 100;
            _itemWeight = 1f;
            return;
        }

        float random = Random.value;
        float cumulativeChance = 0;

        foreach (ItemData item in _itemTypes)
        {
            cumulativeChance += item.spawnChance;
            
            if (random <= cumulativeChance)
            {
                ApplyItemData(item);
                return;
            }
        }

        ApplyItemData(_itemTypes[0]);
    }

    private void ApplyItemData(ItemData data)
    {
        _itemValue = data.value;
        _itemWeight = data.weight;
        
        if (_spriteRenderer != null && data.itemSprite != null)
        {
            _spriteRenderer.sprite = data.itemSprite;
        }
        
        if (itemRigidbody != null)
        {
            itemRigidbody.mass = data.weight;
        }
    }

    private void Update()
    {
        Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (!_isMouseOver)
            {
                _isMouseOver = true;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame && !_isDragging)
            {
                StartDrag();
            }
        }
        else
        {
            _isMouseOver = false;
        }

        if (_isDragging && itemRigidbody != null)
        {
            Vector2 targetPos = mousePos;
            itemRigidbody.linearVelocity = (targetPos - (Vector2)transform.position) * dragSmoothness;
        }

        if (_isDragging && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            StopDrag();
        }
    }

    private void StartDrag()
    {
        _isDragging = true;
        
        if (itemRigidbody != null)
        {
            itemRigidbody.gravityScale = 0f;
            itemRigidbody.linearVelocity = Vector2.zero;
            itemRigidbody.angularVelocity = 0f;
        }
    }

    private void StopDrag()
    {
        _isDragging = false;
        
        if (itemRigidbody != null)
        {
            itemRigidbody.gravityScale = 0.5f;
        }
    }

    public void Collect()
    {
        if (_isCollected) return;
        _isCollected = true;
        
        int reward = Mathf.FloorToInt(_itemValue);
        
        if (_mineController != null)
        {
            _mineController.AddMoney(reward);
        }
        
        if (_floatingTextPrefab != null)
        {
            ShowFloatingText(reward);
        }
        
        Destroy(gameObject);
    }

    private void ShowFloatingText(int amount)
    {
        FloatingText text = Instantiate(_floatingTextPrefab, transform.position, Quaternion.identity);
        
        string textString = amount >= 0 ? $"+{amount:N0} ₽" : $"{amount:N0} ₽";
        bool isPositive = amount >= 0;
        
        text.Initialize(textString, isPositive, transform.position);
    }

    private void OnDestroy()
    {
        if (!_isCollected && _mineController != null && !_isDragging)
        {
            _mineController.AddMoney(Mathf.FloorToInt(_itemValue));
        }
    }
}