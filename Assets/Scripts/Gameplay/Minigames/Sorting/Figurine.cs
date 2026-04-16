using UnityEngine;
using UnityEngine.UI;

public class Figurine : MonoBehaviour
{
    [SerializeField] private ItemDataSO _data;
    [SerializeField] private Image _image;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SfxPlayer _sfxPlayer;
    [SerializeField] private AudioClip _pushSound;
    
    private bool _isPushed;
    private bool _isProcessed;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _image = GetComponent<Image>();
        
        if (_data != null && _image != null)
        {
            _image.sprite = _data.icon;
            
            // Установить прозрачность
            Color color = _image.color;
            color.a = Random.Range(0.7f, 1f); // Явно float
            _image.color = color;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShredderZone") && !_isProcessed)
        {
            _isProcessed = true;
            PushToShredder();
        }
        else if (other.CompareTag("AcceptZone") && !_isProcessed)
        {
            _isProcessed = true;
            PushToAccept();
        }
    }
    
    private void PushToShredder()
    {
        if (_isPushed) return;
        _isPushed = true;
        
        _sfxPlayer?.Play(_pushSound);
        
        // Добавить деньги за брак
        // _wallet.Add(_data.value / 2);
        
        Destroy(gameObject, 1f);
    }
    
    private void PushToAccept()
    {
        if (_isPushed) return;
        _isPushed = true;
        
        _sfxPlayer?.Play(_pushSound);
        
        // Добавить полную стоимость
        // _wallet.Add(_data.value);
        
        Destroy(gameObject, 1f);
    }
}