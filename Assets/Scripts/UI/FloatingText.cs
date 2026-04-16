using UnityEngine;
using TMPro;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TextMeshPro _worldText;
    [SerializeField] private float _lifetime = 1.5f;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private Color _positiveColor = Color.green;
    [SerializeField] private Color _negativeColor = Color.red;

    public void Initialize(string text, bool isPositive, Vector3 worldPosition)
    {
        transform.position = worldPosition;
        
        if (_worldText != null)
        {
            _worldText.text = text;
            _worldText.color = isPositive ? _positiveColor : _negativeColor;
        }
        
        StartCoroutine(FadeAndMove());
    }

    private IEnumerator FadeAndMove()
    {
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        
        while (elapsed < _lifetime)
        {
            elapsed += Time.deltaTime;
            
            transform.position = startPos + Vector3.up * _moveSpeed * elapsed;
            
            float alpha = Mathf.Lerp(1f, 0f, elapsed / _lifetime);
            
            if (_worldText != null)
            {
                Color color = _worldText.color;
                color.a = alpha;
                _worldText.color = color;
            }
            
            yield return null;
        }
        
        Destroy(gameObject);
    }
}