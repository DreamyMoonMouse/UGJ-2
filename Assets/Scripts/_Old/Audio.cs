using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour
{
    public static Audio Instance { get; private set; }

    [SerializeField] private GameSettingsSO _settings;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource[] _sfxSources;
    [SerializeField] private AudioSource _ambientSource;
    [SerializeField] private AudioSource _loopSfxSource;
    [SerializeField] private int _maxSimultaneousSFX = 8;
    [SerializeField] private float _fadeDuration = 1f;

    private int _currentSFXIndex = 0;
    private bool _isMusicPaused = false;
    private float _musicPauseTime = 0f;
    private float _targetMusicVolume = 0.5f;
    private Coroutine _currentFadeCoroutine = null;

    private void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            if (_musicSource == null)
            {
                _musicSource = gameObject.AddComponent<AudioSource>();
                _musicSource.playOnAwake = false;
                _musicSource.loop = true;
                _musicSource.spatialBlend = 0f;
            }
            
            if (_ambientSource == null)
            {
                _ambientSource = gameObject.AddComponent<AudioSource>();
                _ambientSource.playOnAwake = false;
                _ambientSource.loop = true;
                _ambientSource.spatialBlend = 0f;
            }
            
            if (_loopSfxSource == null)
            {
                _loopSfxSource = gameObject.AddComponent<AudioSource>();
                _loopSfxSource.playOnAwake = false;
                _loopSfxSource.loop = true;
                _loopSfxSource.spatialBlend = 0f;
            }
            
            if (_sfxSources == null || _sfxSources.Length == 0)
            {
                _sfxSources = new AudioSource[_maxSimultaneousSFX];
                for (int i = 0; i < _maxSimultaneousSFX; i++)
                {
                    AudioSource source = gameObject.AddComponent<AudioSource>();
                    source.playOnAwake = false;
                    source.spatialBlend = 0f;
                    _sfxSources[i] = source;
                }
            }
        } 
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Start() 
    {
        _targetMusicVolume = _settings.musicVolume;
        SetSfxVolume(_settings.sfxVolume);
        PlayMusic(_settings.menuMusic);
    }

    public void SetMusicVolume(float volume) 
    {
        _targetMusicVolume = volume;
        if (_musicSource != null && !_isMusicPaused) 
        {
            _musicSource.volume = volume;
        }
    }

    public void SetSfxVolume(float volume) 
    {
        if (_sfxSources != null)
        {
            foreach (AudioSource source in _sfxSources)
            {
                if (source != null)
                {
                    source.volume = volume;
                }
            }
        }
        
        if (_loopSfxSource != null)
        {
            _loopSfxSource.volume = volume;
        }
        
        if (_ambientSource != null)
        {
            _ambientSource.volume = volume * 0.5f;
        }
    }

    public void PlayClick() 
    {
        if (_settings.clickSound != null) 
        {
            PlaySfx(_settings.clickSound);
        }
    }

    public void PlaySfx(AudioClip clip) 
    {
        if (clip == null || _sfxSources == null || _sfxSources.Length == 0) return;
        
        AudioSource source = GetAvailableSFXSource();
        if (source != null)
        {
            source.PlayOneShot(clip);
        }
    }

    public void PlayAmbient(AudioClip clip)
    {
        if (_ambientSource != null && clip != null)
        {
            _ambientSource.Stop();
            _ambientSource.clip = clip;
            _ambientSource.volume = _settings.sfxVolume * 0.5f;
            _ambientSource.Play();
        }
    }

    public void StopAmbient()
    {
        if (_ambientSource != null)
        {
            _ambientSource.Stop();
        }
    }

    public void PlaySfxLoop(AudioClip clip)
    {
        if (_loopSfxSource != null && clip != null)
        {
            _loopSfxSource.Stop();
            _loopSfxSource.clip = clip;
            _loopSfxSource.volume = _settings.sfxVolume;
            _loopSfxSource.Play();
        }
    }

    public void StopSfxLoop()
    {
        if (_loopSfxSource != null)
        {
            _loopSfxSource.Stop();
        }
    }

    private AudioSource GetAvailableSFXSource()
    {
        for (int i = 0; i < _sfxSources.Length; i++)
        {
            int index = (_currentSFXIndex + i) % _sfxSources.Length;
            if (!_sfxSources[index].isPlaying)
            {
                _currentSFXIndex = (index + 1) % _sfxSources.Length;
                return _sfxSources[index];
            }
        }
        
        _currentSFXIndex = (_currentSFXIndex + 1) % _sfxSources.Length;
        return _sfxSources[_currentSFXIndex];
    }

    public void PlayMusic(AudioClip clip) 
    {
        if (_musicSource != null && clip != null) 
        {
            _musicSource.Stop();
            _musicSource.clip = clip;
            _musicSource.time = 0f;
            _musicSource.Play();
            _isMusicPaused = false;
            _musicSource.volume = 0f;
            
            if (_currentFadeCoroutine != null)
            {
                StopCoroutine(_currentFadeCoroutine);
            }
            _currentFadeCoroutine = StartCoroutine(FadeMusicVolume(_targetMusicVolume));
        }
    }

    public void PauseMusic() 
    {
        if (_musicSource != null && !_isMusicPaused) 
        {
            _musicPauseTime = _musicSource.time;
            
            if (_currentFadeCoroutine != null)
            {
                StopCoroutine(_currentFadeCoroutine);
            }
            _currentFadeCoroutine = StartCoroutine(FadeMusicVolume(0f, _fadeDuration, () => 
            {
                _musicSource.Pause();
                _isMusicPaused = true;
            }));
        }
    }

    public void ResumeMusic() 
    {
        if (_musicSource != null && _isMusicPaused) 
        {
            _musicSource.time = _musicPauseTime;
            _musicSource.UnPause();
            _isMusicPaused = false;
            
            if (_currentFadeCoroutine != null)
            {
                StopCoroutine(_currentFadeCoroutine);
            }
            _currentFadeCoroutine = StartCoroutine(FadeMusicVolume(_targetMusicVolume));
        }
    }

    public void FadeOutMusic(float duration = 1f) 
    {
        if (_musicSource != null) 
        {
            if (_currentFadeCoroutine != null)
            {
                StopCoroutine(_currentFadeCoroutine);
            }
            _currentFadeCoroutine = StartCoroutine(FadeMusicVolume(0f, duration, () => 
            {
                _musicSource.Stop();
                _isMusicPaused = false;
            }));
        }
    }

    public void StopMusic() 
    {
        if (_musicSource != null) 
        {
            _musicSource.Stop();
            _musicSource.volume = 0f;
            _isMusicPaused = false;
        }
    }
    
    public void StopAllSfx() 
    {
        if (_sfxSources != null)
        {
            foreach (AudioSource source in _sfxSources)
            {
                if (source != null && source.isPlaying)
                {
                    source.Stop();
                }
            }
        }
        
        StopAmbient();
        StopSfxLoop();
    }

    private void OnApplicationQuit()
    {
        Time.timeScale = 1f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    private IEnumerator FadeMusicVolume(float targetVolume, float duration = -1f, System.Action onComplete = null)
    {
        if (duration < 0) duration = _fadeDuration;
        
        float startVolume = _musicSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            t = Mathf.SmoothStep(0, 1, t);
            
            _musicSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        _musicSource.volume = targetVolume;
        
        if (onComplete != null)
        {
            onComplete.Invoke();
        }
    }
}