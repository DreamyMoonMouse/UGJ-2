using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;  

public class Fade : MonoBehaviour
{
    public static Fade Instance { get; private set; }
    
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private GameSettingsSO _settings; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        if (_fadeImage != null)
        {
            _fadeImage.color = new Color(0, 0, 0, 1);
        }
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("SettingsInitialized"))
        {
            Screen.fullScreen = _settings.defaultFullscreen;
            Screen.SetResolution(
                _settings.defaultWidth, 
                _settings.defaultHeight, 
                _settings.defaultFullscreen
            );
            PlayerPrefs.SetInt("SettingsInitialized", 1);
            PlayerPrefs.Save();
        }
    
        gameObject.SetActive(true);
        StartCoroutine(FadeOutCoroutine());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameObject.SetActive(true);
        
        if (_fadeImage != null)
        {
            _fadeImage.color = new Color(0, 0, 0, 1);
        }
        StartCoroutine(FadeOutCoroutine());
    }

    public void FadeIn()
    {
        if (_fadeImage != null)
        {
            gameObject.SetActive(true);
            StartCoroutine(FadeInCoroutine());
        }
    }

    public void FadeOut()
    {
        if (_fadeImage != null)
        {
            StartCoroutine(FadeOutCoroutine());
        }
    }

    private IEnumerator FadeInCoroutine()
    {
        if (_fadeImage == null) yield break;

        float elapsed = 0;
        while (elapsed < _fadeDuration)
        {
            if (_fadeImage == null) yield break;
            
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsed / _fadeDuration);
            _fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        if (_fadeImage != null)
        {
            _fadeImage.color = new Color(0, 0, 0, 1);
        }
    }

    private IEnumerator FadeOutCoroutine()
    {
        if (_fadeImage == null) yield break;

        float elapsed = 0;
        while (elapsed < _fadeDuration)
        {
            if (_fadeImage == null) yield break;
            
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsed / _fadeDuration);
            _fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        if (_fadeImage != null)
        {
            _fadeImage.color = new Color(0, 0, 0, 0);
        }
        
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}