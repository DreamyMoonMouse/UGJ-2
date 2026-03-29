using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Victory : MonoBehaviour
{
    [Header("Аудио")]
    [SerializeField] private AudioClip _bgmMusic;
    [SerializeField] private AudioClip _ambientSound;
    [SerializeField] private AudioClip _typingSound;

    [Header("Текст благодарностей")]
    [SerializeField] private TextMeshProUGUI _creditsText;
    [SerializeField] private float _typingSpeed = 0.03f;
    [SerializeField] private string[] _creditsLines;

    [Header("Кнопки")]
    [SerializeField] private Button _closeButton;

    [Header("Настройки")]
    [SerializeField] private float _startDelay = 1f;

    private bool _isTyping = false;
    private string _fullText = "";

    private void Awake()
    {
        Time.timeScale = 1f;
        
        if (_closeButton != null)
        {
            _closeButton.onClick.AddListener(OnCloseClicked);
        }
        
        BuildFullText();
    }

    private void Start()
    {
        if (Audio.Instance != null)
        {
            if (_bgmMusic != null)
            {
                Audio.Instance.PlayMusic(_bgmMusic);
            }
            
            if (_ambientSound != null)
            {
                Audio.Instance.PlayAmbient(_ambientSound);
            }
        }
        
        Invoke(nameof(StartCredits), _startDelay);
    }

    private void BuildFullText()
    {
        foreach (string line in _creditsLines)
        {
            _fullText += line + "\n";
        }
    }

    private void StartCredits()
    {
        if (_creditsText != null)
        {
            StartCoroutine(TypeCredits());
        }
    }

    private IEnumerator TypeCredits()
    {
        _isTyping = true;
        _creditsText.text = "";

        if (Audio.Instance != null && _typingSound != null)
        {
            Audio.Instance.PlaySfxLoop(_typingSound);
        }

        foreach (char c in _fullText)
        {
            _creditsText.text += c;
            yield return new WaitForSeconds(_typingSpeed);
        }

        if (Audio.Instance != null)
        {
            Audio.Instance.StopSfxLoop();
        }

        _isTyping = false;
    }

    private void OnCloseClicked()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.FadeOutMusic(1f);
            Audio.Instance.StopAmbient();
        }

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
        
        if (_closeButton != null)
        {
            _closeButton.onClick.RemoveListener(OnCloseClicked);
        }
    }
}