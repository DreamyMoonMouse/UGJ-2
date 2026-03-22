using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Serialization;

public class Letter : MonoBehaviour
{
    [Header("Ссылки на SO")]
    [SerializeField] private GameStateSO _gameState;
    [SerializeField] private GameSettingsSO _settings;
    
    [Header("Конверты")]
    [SerializeField] private Image _envelopeClosed;
    [SerializeField] private Image _envelopeOpen;
    
    [Header("Письма")]
    [SerializeField] private GameObject _panelLetter1;
    [SerializeField] private GameObject _panelLetter2;
    [SerializeField] private Image _paper1;
    [SerializeField] private Image _paper2;
    
    [Header("Тексты (TMP)")]
    [SerializeField] private TextMeshProUGUI _textTitle1;
    [SerializeField] private TextMeshProUGUI _textContent1;
    [SerializeField] private TextMeshProUGUI _textTitle2;
    [SerializeField] private TextMeshProUGUI _textContent2;
    
    [Header("Кнопки")]
    [SerializeField] private Button _buttonAccept;
    [SerializeField] private Button _buttonStartWork;
    
    [Header("Гусь")]
    [SerializeField] private GameObject _gooseThinking;
    [SerializeField] private GameObject _gooseSurprised;
    
    [Header("Штамп")]
    [SerializeField] private Image _stampImage;
    
    [Header("Тексты писем (по уровням)")]
    [TextArea(5, 10)] public string[] _letterText1;
    [TextArea(5, 10)] public string[] _letterText2;
    
    [Header("Звуки")]
    [SerializeField] private AudioClip _envelopeOpenSound;
    [SerializeField] private AudioClip _paperAppearSound;
    [SerializeField] private AudioClip _stampSound;
    [SerializeField] private AudioClip _gooseSurprisedSound;
    
    [Header("Настройки анимации")]
    [SerializeField] float _envelopeOpenDuration = 2.5f;
    [SerializeField] float _fadeDuration = 2.5f;
    [SerializeField] float _scaleDuration = 2.5f;
    [SerializeField] float _textTypingSpeed = 0.016f;

    private bool _isTyping = false;
    private Vector3 _paper1InitialScale;
    private Vector3 _paper2InitialScale;
    private Image _gooseThinkingImg;
    private Image _gooseSurprisedImg;

    void Awake()
    {
        _envelopeClosed.gameObject.SetActive(true);
        _envelopeOpen.gameObject.SetActive(false);

        _panelLetter1.SetActive(false);
        _panelLetter2.SetActive(false);
        
        _stampImage.gameObject.SetActive(false);

        _buttonAccept.gameObject.SetActive(false);
        _buttonStartWork.gameObject.SetActive(false);

        _envelopeClosed.color = new Color(1, 1, 1, 1);
        _envelopeOpen.color = new Color(1, 1, 1, 0);
        _gooseThinkingImg = _gooseThinking.GetComponent<Image>();
        _gooseThinkingImg.color = new Color(1, 1, 1, 0);
        _textTitle1.color = new Color(_textTitle1.color.r, _textTitle1.color.g, _textTitle1.color.b, 0);
        _textContent1.color = new Color(_textTitle1.color.r, _textTitle1.color.g, _textTitle1.color.b, 0);
        
        _gooseThinking.gameObject.SetActive(false);
        _gooseSurprised.gameObject.SetActive(false);

        if (_paper1 != null)
        {
            _paper1InitialScale = _paper1.rectTransform.localScale;
            _paper1.rectTransform.localScale = Vector3.zero;
            _paper1.color = new Color(1, 1, 1, 0);
        }

        if (_paper2 != null)
        {
            _paper2InitialScale = _paper2.rectTransform.localScale;
            _paper2.gameObject.SetActive(false);
        }

        _buttonAccept.onClick.AddListener(OnAcceptClicked);
        _buttonStartWork.onClick.AddListener(OnStartWorkClicked);
    }

    void Start()
    {
        Audio.Instance.PlayMusic(_settings.letterMusic);
    }

    void SetupLetterTexts()
    {
        int levelIndex = _gameState.selectedLevel - 1;

        if (levelIndex >= 0 && levelIndex < _letterText1.Length)
        {
            _textTitle1.text = "СПОЙЛЕР";
            _textContent1.text = _letterText1[levelIndex];

            _textTitle2.text = "СПОЙЛЕР";
            _textContent2.text = _letterText2[levelIndex];
        }
    }

    public void OnEnvelopeClicked()
    {
        if (_envelopeClosed.gameObject.activeSelf && !_isTyping)
        {
            SetupLetterTexts();
            Audio.Instance.PlaySfx(_envelopeOpenSound);
            StartCoroutine(OpenEnvelopeSequence());
        }
    }

    IEnumerator OpenEnvelopeSequence()
    {
        StartCoroutine(FadeAlpha(_envelopeClosed, 1, 0, _fadeDuration));
        _envelopeClosed.gameObject.SetActive(false);

        _envelopeOpen.gameObject.SetActive(true);
        StartCoroutine(FadeAlpha(_envelopeOpen, 0, 1, _fadeDuration));
        yield return new WaitForSeconds(_envelopeOpenDuration);
        StartCoroutine(FadeAlpha(_envelopeOpen, 1, 0, _fadeDuration));
        _envelopeOpen.gameObject.SetActive(false);
        
        Audio.Instance.PlaySfx(_paperAppearSound);
        StartCoroutine(ScaleAnimation(_paper1.rectTransform, _paper1InitialScale, _scaleDuration));
        StartCoroutine(FadeAlpha(_paper1, 0, 1, _fadeDuration));
        yield return new WaitForSeconds(_fadeDuration);
        _panelLetter1.SetActive(true);
        _gooseThinking.gameObject.SetActive(true);
        StartCoroutine(FadeAlpha(_gooseThinkingImg, 0, 1, _fadeDuration));
        _textTitle1.gameObject.SetActive(true);
        yield return StartCoroutine(FadeAlphaUI(_textTitle1, 0, 1, _fadeDuration));
        _textContent1.gameObject.SetActive(true);
        yield return StartCoroutine(TypeText(_textContent1, _textContent1.text));
        
        yield return new WaitForSeconds(0.6f);
        _buttonAccept.gameObject.SetActive(true);
    }

    void OnAcceptClicked()
    {
        Audio.Instance.PlayClick();
        StartCoroutine(ShowSecondLetterSequence());
        _buttonAccept.gameObject.SetActive(false);
    }

    IEnumerator ShowSecondLetterSequence()
    {
        Audio.Instance.PlaySfx(_paperAppearSound);
        _panelLetter2.SetActive(true);
        _paper2.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.8f);
        _stampImage.gameObject.SetActive(true);
        Audio.Instance.PlaySfx(_stampSound);
        _gooseThinking.gameObject.SetActive(false);
        _gooseSurprised.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Audio.Instance.PlaySfx(_gooseSurprisedSound);

        yield return new WaitForSeconds(0.5f);
        _buttonStartWork.gameObject.SetActive(true);
    }

    void OnStartWorkClicked()
    {
        Audio.Instance.PlayClick();
        _gameState.UnlockLevel(_gameState.selectedLevel);
        StartCoroutine(FadeToLevel());
    }

    IEnumerator FadeToLevel()
    {
        yield return new WaitForSeconds(0.5f);
        SceneLoader.LoadLevel(_gameState.selectedLevel);
    }

    IEnumerator FadeAlpha(Image target, float from, float to, float duration)
    {
        float elapsed = 0;
        Color originalColor = target.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = Mathf.SmoothStep(0, 1, t);

            float alpha = Mathf.Lerp(from, to, t);
            target.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        target.color = new Color(originalColor.r, originalColor.g, originalColor.b, to);
    }

    IEnumerator FadeAlphaUI(TextMeshProUGUI target, float from, float to, float duration)
    {
        float elapsed = 0;
        Color originalColor = target.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = Mathf.SmoothStep(0, 1, t);

            float alpha = Mathf.Lerp(from, to, t);
            target.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        target.color = new Color(originalColor.r, originalColor.g, originalColor.b, to);
    }

    IEnumerator ScaleAnimation(RectTransform targetRect, Vector3 targetScale, float duration)
    {
        float elapsed = 0;
        Vector3 startScale = targetRect.localScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = 1 - Mathf.Pow(1 - t, 3);

            targetRect.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        targetRect.localScale = targetScale;
    }

    IEnumerator TypeText(TextMeshProUGUI textField, string fullText)
    {
        _textContent1.color = new Color(_textTitle1.color.r, _textTitle1.color.g, _textTitle1.color.b, 1);
        _isTyping = true;
        textField.text = "";

        foreach (char c in fullText)
        {
            textField.text += c;
            yield return new WaitForSeconds(_textTypingSpeed);
        }

        _isTyping = false;
    }

    void OnDestroy()
    {
        _buttonAccept.onClick.RemoveListener(OnAcceptClicked);
        _buttonStartWork.onClick.RemoveListener(OnStartWorkClicked);
    }
}