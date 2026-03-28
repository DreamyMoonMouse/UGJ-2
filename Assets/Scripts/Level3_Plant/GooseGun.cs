using UnityEngine;
using UnityEngine.InputSystem;

public class GooseGun : MonoBehaviour
{
    [Header("Ссылки на объекты")]
    [SerializeField] private Transform _handPivot;
    [SerializeField] private Transform _handGun;
    [SerializeField] private Transform _gooseSprite;
    [SerializeField] private Transform _catchZone;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _flipThreshold = 0.1f;
    [SerializeField] private float _spriteOffset = 90f;

    [Header("Зона притяжения")]
    [SerializeField] private Collider2D _attractionZone;
    [SerializeField] private float _attractionForce = 10f;

    private Camera _mainCamera;
    private bool _isFacingRight = true;
    private bool _isGameActive = false;

    private void Awake()
    {
        _mainCamera = Camera.main;
        
        if (_attractionZone == null)
        {
            _attractionZone = GetComponent<Collider2D>();
        }
    }

    public void StartGame()
    {
        _isGameActive = true;
    }

    public void StopGame()
    {
        _isGameActive = false;
    }

    private void Update()
    {
        if (!_isGameActive) return;
        
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldMousePos = _mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
        
        FlipObjects(worldMousePos);
        RotateHand(worldMousePos);
    }

    private void FlipObjects(Vector3 mousePos)
    {
        float direction = mousePos.x - transform.position.x;
        
        bool shouldBeRight = direction > _flipThreshold;
        bool shouldBeLeft = direction < -_flipThreshold;
        
        if (shouldBeRight && !_isFacingRight)
        {
            _isFacingRight = true;
            ApplyFlip(1f);
        }
        else if (shouldBeLeft && _isFacingRight)
        {
            _isFacingRight = false;
            ApplyFlip(-1f);
        }
    }

    private void ApplyFlip(float direction)
    {
        if (_gooseSprite != null)
        {
            _gooseSprite.localScale = new Vector3(
                direction * Mathf.Abs(_gooseSprite.localScale.x),
                _gooseSprite.localScale.y,
                _gooseSprite.localScale.z
            );
        }
        
        if (_handGun != null)
        {
            _handGun.localScale = new Vector3(
                direction * Mathf.Abs(_handGun.localScale.x),
                _handGun.localScale.y,
                _handGun.localScale.z
            );
        }
    }

    private void RotateHand(Vector3 mousePos)
    {
        if (_handPivot == null) return;
        
        Vector3 direction = mousePos - _handPivot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        if (!_isFacingRight)
        {
            angle = 180f - angle;
        }
        
        angle += _spriteOffset;
        
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        _handPivot.rotation = Quaternion.Lerp(_handPivot.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isGameActive) return;
        
        if (other.CompareTag("Egg"))
        {
            Egg egg = other.GetComponent<Egg>();
            if (egg != null && !egg.IsAttracted)
            {
                egg.StartAttraction(_catchZone.position, _attractionForce);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_isGameActive) return;
        
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