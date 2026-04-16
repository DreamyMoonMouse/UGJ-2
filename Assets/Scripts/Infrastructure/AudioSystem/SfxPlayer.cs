using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _sfxSource;
    
    public void Play(AudioClip clip) => _sfxSource.PlayOneShot(clip);
    public void Play(AudioClip clip, float volume) => _sfxSource.PlayOneShot(clip, volume);
    public void SetVolume(float volume) => _sfxSource.volume = volume;
    
    // Для loop звуков (опционально)
    public void PlayLoop(AudioClip clip)
    {
        _sfxSource.clip = clip;
        _sfxSource.loop = true;
        _sfxSource.Play();
    }
    
    public void StopLoop()
    {
        _sfxSource.loop = false;
        _sfxSource.Stop();
    }
}