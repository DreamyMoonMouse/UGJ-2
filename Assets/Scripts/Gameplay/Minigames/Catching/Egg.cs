using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] private ItemDataSO _data;
    [SerializeField] private float _rollSpeed = 3f;
    [SerializeField] private float _fallSpeed = 2f;
    [SerializeField] private SfxPlayer _sfxPlayer;
    [SerializeField] private AudioClip _breakSound;
    
    private Rigidbody2D _rb;
    private bool _isBroken;
    
    public bool IsBroken => _isBroken;
    public float Value => _data?.value ?? 0;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        if (!_isBroken)
        {
            _rb.linearVelocity = new Vector2(_rollSpeed, -_fallSpeed);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isBroken && !collision.collider.CompareTag("Basket"))
        {
            Break();
        }
    }
    
    private void Break()
    {
        _isBroken = true;
        _sfxPlayer?.Play(_breakSound);
        
        // Создать эффект разбитого яйца
        // Instantiate(_brokenEggPrefab, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }
}