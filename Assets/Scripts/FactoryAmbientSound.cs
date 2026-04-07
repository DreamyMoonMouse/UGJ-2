using UnityEngine;

public class FactoryAmbientSound : MonoBehaviour
{
    [SerializeField] private AudioClip _factoryAmbient;
    [SerializeField] private float _volume = 0.5f;

    private AudioSource _audioSource;
    private bool _isPlaying = false;

    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = true;
        _audioSource.spatialBlend = 0f;
        _audioSource.volume = _volume;
    }

    public void StartAmbient()
    {
        if (_factoryAmbient != null && !_isPlaying)
        {
            _audioSource.clip = _factoryAmbient;
            _audioSource.Play();
            _isPlaying = true;
        }
    }

    public void StopAmbient()
    {
        if (_isPlaying)
        {
            _audioSource.Stop();
            _isPlaying = false;
        }
    }

    private void OnDestroy()
    {
        StopAmbient();
    }
}
