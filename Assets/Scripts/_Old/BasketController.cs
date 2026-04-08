using UnityEngine;
using UnityEngine.InputSystem;

public class BasketController : MonoBehaviour
{
    [SerializeField] private Transform _pivot;
    [SerializeField] private Transform _hand;
    [SerializeField] private Transform _basket;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _minAngle = -60f;
    [SerializeField] private float _maxAngle = 60f;
    [SerializeField] private Camera _mainCamera;

    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldMousePos = _mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
        
        if (_pivot != null && _hand != null)
        {
            Vector3 direction = worldMousePos - _pivot.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            
            targetAngle = Mathf.Clamp(targetAngle, _minAngle, _maxAngle);
            
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            _hand.rotation = Quaternion.Lerp(_hand.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}