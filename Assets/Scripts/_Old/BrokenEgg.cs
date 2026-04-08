using UnityEngine;

public class BrokenEgg : MonoBehaviour
{
    [SerializeField] private float _destroyTime = 2f;
    [SerializeField] private ParticleSystem _breakParticles;

    private void Start()
    {
        if (_breakParticles != null)
        {
            _breakParticles.Play();
        }
        
        Destroy(gameObject, _destroyTime);
    }
}