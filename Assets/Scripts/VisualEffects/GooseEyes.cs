using UnityEngine;

public class GooseEyes : MonoBehaviour
{
    [SerializeField] private Transform _leftEye;
    [SerializeField] private Transform _rightEye;
    [SerializeField] private float _followSpeed = 5f;
    
    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        
        LookAt(_leftEye, worldPos);
        LookAt(_rightEye, worldPos);
    }
    
    private void LookAt(Transform eye, Vector3 target)
    {
        Vector3 direction = (target - eye.position).normalized;
        Vector3 lookPos = eye.position + direction * 0.1f;
        eye.position = Vector3.Lerp(eye.position, lookPos, Time.deltaTime * _followSpeed);
    }
}