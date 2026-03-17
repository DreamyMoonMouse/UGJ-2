using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio Instance { get; private set; }

    [SerializeField] GameSettingsSO settings;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        SetMusicVolume(settings.musicVolume);
        SetSfxVolume(settings.sfxVolume);
        
        // Запускаем музыку из GameSettingsSO
        PlayMusic(settings.menuMusic);
    }

    public void SetMusicVolume(float volume) {
        if (musicSource != null) {
            musicSource.volume = volume;
        }
    }

    public void SetSfxVolume(float volume) {
        if (sfxSource != null) {
            sfxSource.volume = volume;
        }
    }

    public void PlayClick() {
        if (sfxSource != null && settings.clickSound != null) {
            sfxSource.PlayOneShot(settings.clickSound);
        }
    }

    public void PlaySfx(AudioClip clip) {
        if (sfxSource != null && clip != null) {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayMusic(AudioClip clip) {
        if (musicSource != null && clip != null) {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void StopMusic() {
        if (musicSource != null) {
            musicSource.Stop();
        }
    }
}