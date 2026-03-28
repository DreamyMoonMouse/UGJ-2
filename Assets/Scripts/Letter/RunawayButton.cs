using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RunawayButton : MonoBehaviour
{
    [SerializeField] private RectTransform _buttonRect;
    [SerializeField] private float _runDistance = 150f;
    [SerializeField] private float _runSpeed = 10f;
    [SerializeField] private Vector2 _minAnchor = new Vector2(-800, -400);
    [SerializeField] private Vector2 _maxAnchor = new Vector2(800, 400);
    [SerializeField] private bool _isButtonActive = false;

    private Canvas _canvas;
    private Vector2 _buttonLocalPos;

    private void Awake()
    {
        if (_buttonRect == null)
        {
            _buttonRect = GetComponent<RectTransform>();
        }
        
        _canvas = GetComponentInParent<Canvas>();
    }

    private void OnEnable()
    {
        _isButtonActive = true;
    }

    private void OnDisable()
    {
        _isButtonActive = false;
    }

    private void Update()
    {
        if (!_isButtonActive) return;
        
        Vector2 mousePos = Mouse.current.position.ReadValue();
        
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _buttonRect,
            mousePos,
            _canvas.worldCamera,
            out Vector2 localMousePos))
        {
            float distance = Vector2.Distance(Vector2.zero, localMousePos);
            
            if (distance < _runDistance)
            {
                RunFromMouse(localMousePos);
            }
        }
    }

    private void RunFromMouse(Vector2 localMousePos)
    {
        Vector2 direction = -localMousePos.normalized;
        
        float newX = _buttonRect.anchoredPosition.x + direction.x * _runSpeed;
        float newY = _buttonRect.anchoredPosition.y + direction.y * _runSpeed;
        
        newX = Mathf.Clamp(newX, _minAnchor.x, _maxAnchor.x);
        newY = Mathf.Clamp(newY, _minAnchor.y, _maxAnchor.y);
        
        _buttonRect.anchoredPosition = new Vector2(newX, newY);
    }

    public void OnClick()
    {
        Debug.Log("Кнопка поймана!");
    }
}