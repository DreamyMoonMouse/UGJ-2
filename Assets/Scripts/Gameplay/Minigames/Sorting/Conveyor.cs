using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Vector2 _direction = Vector2.right;
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = _direction * _speed;
        }
    }
}