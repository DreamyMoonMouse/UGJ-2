using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class VictoryEnvelope : MonoBehaviour
{
    [Header("Конверт")]
    [SerializeField] private Image _envelopeClosed;
    [SerializeField] private Image _envelopeOpen;
    [SerializeField] private Image _paper;

    [Header("Текст")]
    [SerializeField] private TextMeshProUGUI _creditsText;
    [SerializeField] private string[] _creditsLines;

    [Header("Анимация")]
    [SerializeField] private float _fadeDuration = 1.0f;
    [SerializeField] private float _scaleDuration = 1.0f;
    [SerializeField] private float _typingSpeed = 0.03f;
    [SerializeField] private Vector3 _paperInitialScale = Vector3.zero;

    [Header("Аудио")]
    [SerializeField] private AudioClip _envelopeOpenSound;
    [SerializeField] private AudioClip _paperAppearSound;
    [SerializeField] private AudioClip _typingSound;

    [Header("Настройки")]
    [SerializeField] private float _startDelay = 1f;
    [SerializeField] private float _envelopeOpenDelay = 0.5f;

    private bool _isAnimating = false;
    private string _fullText = "";

    private void Awake()
    {
        BuildFullText();
        Initialize();
    }

    private void Start()
    {
        Invoke(nameof(StartSequence), _startDelay);
    }

    private void Initialize()
    {
        if (_envelopeClosed != null)
        {
            _envelopeClosed.gameObject.SetActive(true);
            _envelopeClosed.color = new Color(1, 1, 1, 1);
        }

        if (_envelopeOpen != null)
        {
            _envelopeOpen.gameObject.SetActive(true);
            _envelopeOpen.color = new Color(1, 1, 1, 0);
        }

        if (_paper != null)
        {
            _paper.gameObject.SetActive(false);
            _paper.color = new Color(1, 1, 1, 0);
            if (_paper.rectTransform != null)
            {
                _paper.rectTransform.localScale = _paperInitialScale;
            }
        }

        if (_creditsText != null)
        {
            _creditsText.text = "";
            _creditsText.color = new Color(1, 1, 1, 0);
        }
    }

    private void BuildFullText()
    {
        foreach (string line in _creditsLines)
        {
            _fullText += line + "\n";
        }
    }

    private void StartSequence()
    {
        if (!_isAnimating)
        {
            StartCoroutine(PlayOpenEnvelopeSequence());
        }
    }

    public IEnumerator PlayOpenEnvelopeSequence()
    {
        _isAnimating = true;

        if (_envelopeOpen != null)
        {
            _envelopeOpen.gameObject.SetActive(true);
            _envelopeOpen.color = new Color(1, 1, 1, 0);
        }

        yield return StartCoroutine(RunParallel(
            FadeAlpha(_envelopeClosed, 1, 0, _fadeDuration),
            FadeAlpha(_envelopeOpen, 0, 1, _fadeDuration)
        ));

        if (Audio.Instance != null && _envelopeOpenSound != null)
        {
            Audio.Instance.PlaySfx(_envelopeOpenSound);
        }

        yield return new WaitForSeconds(_envelopeOpenDelay);

        if (_envelopeOpen != null)
        {
            _envelopeOpen.gameObject.SetActive(false);
        }

        if (_paper != null)
        {
            _paper.gameObject.SetActive(true);
        }

        if (Audio.Instance != null && _paperAppearSound != null)
        {
            Audio.Instance.PlaySfx(_paperAppearSound);
        }

        yield return StartCoroutine(RunParallel(
            FadeAlpha(_envelopeOpen, 1, 0, _fadeDuration),
            ScaleAnimation(_paper.rectTransform, Vector3.one, _scaleDuration),
            FadeAlpha(_paper, 0, 1, _fadeDuration)
        ));

        if (_creditsText != null)
        {
            _creditsText.color = new Color(1, 1, 1, 1);
            _creditsText.text = "";
        }

        yield return StartCoroutine(TypeText(_creditsText, _fullText));

        _isAnimating = false;
    }

    private IEnumerator FadeAlpha(Image target, float from, float to, float duration)
    {
        if (target == null) yield break;

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

    private IEnumerator ScaleAnimation(RectTransform targetRect, Vector3 targetScale, float duration)
    {
        if (targetRect == null) yield break;

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

    private IEnumerator TypeText(TextMeshProUGUI textField, string fullText)
    {
        if (textField == null) yield break;

        textField.text = "";

        if (Audio.Instance != null && _typingSound != null)
        {
            Audio.Instance.PlaySfxLoop(_typingSound);
        }

        foreach (char c in fullText)
        {
            textField.text += c;
            yield return new WaitForSeconds(_typingSpeed);
        }

        if (Audio.Instance != null)
        {
            Audio.Instance.StopSfxLoop();
        }
    }

    private IEnumerator RunParallel(params IEnumerator[] coroutines)
    {
        var running = new System.Collections.Generic.List<bool>();

        foreach (var coro in coroutines)
        {
            running.Add(true);
            StartCoroutine(RunAndMark(coro, running, running.Count - 1));
        }

        while (ContainsTrue(running))
        {
            yield return null;
        }
    }

    private IEnumerator RunAndMark(IEnumerator coroutine, System.Collections.Generic.List<bool> running, int index)
    {
        yield return StartCoroutine(coroutine);
        running[index] = false;
    }

    private bool ContainsTrue(System.Collections.Generic.List<bool> list)
    {
        foreach (var value in list)
        {
            if (value) return true;
        }
        return false;
    }
}