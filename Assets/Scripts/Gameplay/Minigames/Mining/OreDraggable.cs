using UnityEngine;
using UnityEngine.EventSystems;

public class OreDraggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private ItemDataSO _itemData;
    [SerializeField] private Score _score;
    
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private Vector2 _startPosition;
    private bool _isDragging;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
        _startPosition = _rectTransform.anchoredPosition;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
        
        if (IsOverCart(eventData))
        {
            Collect();
        }
        else
        {
            _rectTransform.anchoredPosition = _startPosition;
        }
    }
    
    private bool IsOverCart(PointerEventData eventData)
    {
        return eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("Cart");
    }
    
    private void Collect()
    {
        _score.Add(_itemData.value);
        Destroy(gameObject);
    }
}