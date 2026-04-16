using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource;
    
    public void Play(AudioClip clip)
    {
        if (_musicSource.isPlaying) _musicSource.Stop();
        _musicSource.clip = clip;
        _musicSource.Play();
    }
    
    public void Stop() => _musicSource.Stop();
    public void SetVolume(float volume) => _musicSource.volume = volume;
}