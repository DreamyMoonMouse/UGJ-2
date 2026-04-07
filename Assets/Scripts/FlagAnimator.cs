using UnityEngine;

public class FlagAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _waveSpeed = 2f;
    [SerializeField] private float _waveAmount = 0.05f;

    private Vector3 _originalScale;
    private float _offset = 0f;

    void Start()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _originalScale = transform.localScale;
    }

    void Update()
    {
        _offset += Time.deltaTime * _waveSpeed;
        
        float wave = Mathf.Sin(_offset) * _waveAmount;
        
        transform.localScale = new Vector3(
            _originalScale.x + wave,
            _originalScale.y,
            _originalScale.z
        );
        
        transform.rotation = Quaternion.Euler(0, 0, wave * 5f);
    }
}
