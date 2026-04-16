using UnityEngine;
using UnityEngine.EventSystems;

public class RunawayButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private RectTransform _buttonRect;
    [SerializeField] private float _moveDistance = 100f;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        _buttonRect.anchoredPosition += randomDirection * _moveDistance;
    }
}