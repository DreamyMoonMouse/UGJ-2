using UnityEngine;
using UnityEngine.InputSystem;

public class DragDropItem : MonoBehaviour
{
    [Header("Типы предметов")]
    [SerializeField] private ItemData[] itemTypes;

    [Header("Физика предмета")]
    [SerializeField] private Rigidbody2D itemRigidbody;
    [SerializeField] private float dragSmoothness = 10f;

    private Mine mineController;
    private Camera mainCamera;
    private bool isDragging = false;
    private bool isCollected = false;
    private float itemValue = 100;
    private SpriteRenderer spriteRenderer;
    private bool isMouseOver = false;

    [System.Serializable]
    public class ItemData
    {
        public string itemName;
        public Sprite itemSprite;
        public float value;
        public float spawnChance;
    }

    void Awake()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
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

    void Start()
    {
        RandomizeItemType();
        SetupCollider();
    }

    void SetupCollider()
    {
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            CircleCollider2D collider = GetComponent<CircleCollider2D>();
            if (collider == null)
            {
                collider = gameObject.AddComponent<CircleCollider2D>();
            }
            
            float spriteSize = Mathf.Max(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
            collider.radius = spriteSize / 2f;
            collider.offset = spriteRenderer.sprite.bounds.center;
        }
    }

    void Update()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (!isMouseOver)
            {
                isMouseOver = true;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame && !isDragging)
            {
                StartDrag();
            }
        }
        else
        {
            isMouseOver = false;
        }

        if (isDragging && itemRigidbody != null)
        {
            Vector2 targetPos = mousePos;
            itemRigidbody.linearVelocity = (targetPos - (Vector2)transform.position) * dragSmoothness;
        }

        if (isDragging && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            StopDrag();
        }
    }

    public void SetMineController(Mine controller)
    {
        mineController = controller;
    }

    public void RandomizeItemType()
    {
        if (itemTypes == null || itemTypes.Length == 0)
        {
            itemValue = 100;
            return;
        }

        float random = Random.value;
        float cumulativeChance = 0;

        foreach (ItemData item in itemTypes)
        {
            cumulativeChance += item.spawnChance;
            
            if (random <= cumulativeChance)
            {
                ApplyItemData(item);
                return;
            }
        }

        ApplyItemData(itemTypes[0]);
    }

    void ApplyItemData(ItemData data)
    {
        itemValue = data.value;
        
        if (spriteRenderer != null && data.itemSprite != null)
        {
            spriteRenderer.sprite = data.itemSprite;
        }
    }

    void StartDrag()
    {
        isDragging = true;
        
        if (itemRigidbody != null)
        {
            itemRigidbody.gravityScale = 0f;
            itemRigidbody.linearVelocity = Vector2.zero;
            itemRigidbody.angularVelocity = 0f;
        }
    }

    void StopDrag()
    {
        isDragging = false;
        
        if (itemRigidbody != null)
        {
            itemRigidbody.gravityScale = 0.5f;
        }
    }

    public void Collect()
    {
        if (isCollected) return;
        isCollected = true;
        
        if (mineController != null)
        {
            mineController.AddMoney(Mathf.FloorToInt(itemValue));
        }
        
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (!isCollected && mineController != null && !isDragging)
        {
            mineController.AddMoney(Mathf.FloorToInt(itemValue));
        }
    }
}