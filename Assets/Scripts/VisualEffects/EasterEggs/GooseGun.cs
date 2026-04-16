using UnityEngine;

public class GooseGun : MonoBehaviour
{
    [SerializeField] private Transform _gunPivot;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _shootForce = 500f;
    
    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        
        Vector3 direction = worldPos - _gunPivot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _gunPivot.rotation = Quaternion.Euler(0f, 0f, angle);
        
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    
    private void Shoot()
    {
        GameObject bullet = Instantiate(_bulletPrefab, _gunPivot.position, _gunPivot.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(_gunPivot.right * _shootForce);
    }
}