using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _sfxSource;
    
    public void Play(AudioClip clip) => _sfxSource.PlayOneShot(clip);
    public void SetVolume(float volume) => _sfxSource.volume = volume;
}