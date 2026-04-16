using UnityEngine;

public class GooseEasterEgg : MonoBehaviour
{
    [SerializeField] private GameObject _goose;
    [SerializeField] private float _moveSpeed = 200f;
    [SerializeField] private RectTransform _gooseRect;
    
    private Vector2 _targetPosition;
    private bool _isMoving;
    
    private void Start()
    {
        _targetPosition = _gooseRect.anchoredPosition;
    }
    
    public void Activate()
    {
        _isMoving = true;
        _goose.SetActive(true);
    }
    
    private void Update()
    {
        if (!_isMoving) return;
        
        _gooseRect.anchoredPosition = Vector2.MoveTowards(
            _gooseRect.anchoredPosition,
            _targetPosition,
            _moveSpeed * Time.deltaTime
        );
        
        if (Vector2.Distance(_gooseRect.anchoredPosition, _targetPosition) < 1f)
        {
            _isMoving = false;
        }
    }
}