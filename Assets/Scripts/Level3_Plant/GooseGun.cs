using UnityEngine;
using UnityEngine.InputSystem;

public class GooseGun : MonoBehaviour
{
    [Header("Ссылки на объекты")]
    [SerializeField] private Transform _handPivot;
    [SerializeField] private Transform _handGun;
    [SerializeField] private SpriteRenderer _gooseSpriteRenderer;
    [SerializeField] private SpriteRenderer _handSpriteRenderer;
    [SerializeField] private Transform _catchZone;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private bool _isLeftGoose = false;
    
    [Header("Настройка углов")]
    [SerializeField] private float _angleOffset = 0f;
    [SerializeField] private bool _invertAngleForLeft = false;
    
    [Header("Зона притяжения")]
    [SerializeField] private Collider2D _attractionZone;
    [SerializeField] private float _attractionForce = 10f;

    private Camera _mainCamera;
    private bool _isGameActive = false;
    private bool _isCurrentlyActive = false;

    private void Awake()
    {
        _mainCamera = Camera.main;
        
        if (_attractionZone == null)
        {
            _attractionZone = GetComponent<Collider2D>();
        }
        
        // Скрываем гуся при старте
        SetVisualsActive(false);
    }

    public void StartGame()
    {
        _isGameActive = true;
    }

    public void StopGame()
    {
        _isGameActive = false;
        SetVisualsActive(false);
    }

    private void SetVisualsActive(bool active)
    {
        if (_gooseSpriteRenderer != null)
        {
            _gooseSpriteRenderer.enabled = active;
        }
        
        if (_handSpriteRenderer != null)
        {
            _handSpriteRenderer.enabled = active;
        }
    }

    private void Update()
    {
        if (!_isGameActive) return;
        
        Vector2 mousePos = Mouse.current.position.ReadValue();
        
        CheckIfShouldBeActive(mousePos);
        
        if (_isCurrentlyActive)
        {
            Vector3 worldMousePos = _mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
            RotateHand(worldMousePos);
        }
    }

    private void CheckIfShouldBeActive(Vector2 mousePos)
    {
        float screenCenterX = _mainCamera.pixelWidth / 2f;
        bool mouseIsLeft = mousePos.x < screenCenterX;
        
        bool shouldBeActive = _isLeftGoose ? mouseIsLeft : !mouseIsLeft;
        
        if (shouldBeActive && !_isCurrentlyActive)
        {
            _isCurrentlyActive = true;
            SetVisualsActive(true);
        }
        else if (!shouldBeActive && _isCurrentlyActive)
        {
            _isCurrentlyActive = false;
            SetVisualsActive(false);
        }
    }

    private void RotateHand(Vector3 mousePos)
    {
        if (_handPivot == null) return;
        
        Vector3 direction = mousePos - _handPivot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        if (_invertAngleForLeft)
        {
            angle = 180f - angle;
        }
        
        angle += _angleOffset;
        
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        _handPivot.rotation = Quaternion.Lerp(_handPivot.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"OnTriggerEnter: {other.gameObject.name}, Tag: {other.tag}");
        
        if (!_isGameActive || !_isCurrentlyActive) return;
        
        if (other.CompareTag("Egg"))
        {
            Debug.Log("Яйцо найдено! Начинаем притяжение...");
            
            Egg egg = other.GetComponent<Egg>();
            if (egg != null && !egg.IsAttracted)
            {
                Debug.Log($"StartAttraction к {_catchZone.position}");
                egg.StartAttraction(_catchZone.position, _attractionForce);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_isGameActive || !_isCurrentlyActive) return;
        
        if (other.CompareTag("Egg"))
        {
            Egg egg = other.GetComponent<Egg>();
            if (egg != null && !egg.IsAttracted)
            {
                float distance = Vector2.Distance(other.transform.position, (Vector2)_catchZone.position);
                
                if (distance <= 5f)
                {
                    egg.StartAttraction(_catchZone.position, _attractionForce);
                }
            }
        }
    }
}