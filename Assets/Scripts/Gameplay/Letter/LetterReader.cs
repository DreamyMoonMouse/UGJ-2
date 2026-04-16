using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class VictoryUI : MonoBehaviour
{
    [Header("Envelope")]
    [SerializeField] private Image _envelopeClosed;
    [SerializeField] private Image _envelopeOpen;
    [SerializeField] private Image _paper;
    
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _creditsText;
    [SerializeField] private string[] _creditsLines;
    
    [Header("Animation")]
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _scaleDuration = 1f;
    [SerializeField] private float _typingSpeed = 0.03f;
    [SerializeField] private float _startDelay = 1f;
    [SerializeField] private float _envelopeOpenDelay = 0.5f;
    
    [Header("Audio")]
    [SerializeField] private AudioClip _envelopeOpenSound;
    [SerializeField] private AudioClip _paperAppearSound;
    [SerializeField] private AudioClip _typingSound;
    
    private void Start()
    {
        Initialize();
        Invoke(nameof(StartSequence), _startDelay);
    }
    
    private void Initialize()
    {
        _envelopeClosed.gameObject.SetActive(true);
        _envelopeClosed.color = new Color(1, 1, 1, 1);
        
        _envelopeOpen.gameObject.SetActive(true);
        _envelopeOpen.color = new Color(1, 1, 1, 0);
        
        _paper.gameObject.SetActive(false);
        _paper.color = new Color(1, 1, 1, 0);
        _paper.rectTransform.localScale = Vector3.zero;
        
        _creditsText.text = "";
        _creditsText.color = new Color(1, 1, 1, 0);
    }
    
    private void StartSequence() => StartCoroutine(PlayOpenEnvelopeSequence());
    
    private IEnumerator PlayOpenEnvelopeSequence()
    {
        _envelopeOpen.gameObject.SetActive(true);
        _envelopeOpen.color = new Color(1, 1, 1, 0);
        
        yield return RunParallel(
            FadeAlpha(_envelopeClosed, 1, 0, _fadeDuration),
            FadeAlpha(_envelopeOpen, 0, 1, _fadeDuration)
        );
        
        Audio.Instance?.PlaySfx(_envelopeOpenSound);
        yield return new WaitForSeconds(_envelopeOpenDelay);
        
        _envelopeOpen.gameObject.SetActive(false);
        _paper.gameObject.SetActive(true);
        
        Audio.Instance?.PlaySfx(_paperAppearSound);
        
        yield return RunParallel(
            FadeAlpha(_envelopeOpen, 1, 0, _fadeDuration),
            ScaleAnimation(_paper.rectTransform, Vector3.one, _scaleDuration),
            FadeAlpha(_paper, 0, 1, _fadeDuration)
        );
        
        _creditsText.color = new Color(1, 1, 1, 1);
        _creditsText.text = "";
        
        yield return TypeCredits();
    }
    
    private IEnumerator TypeCredits()
    {
        string fullText = string.Join("\n", _creditsLines);
        
        Audio.Instance?.PlaySfxLoop(_typingSound);
        
        foreach (char c in fullText)
        {
            _creditsText.text += c;
            yield return new WaitForSeconds(_typingSpeed);
        }
        
        Audio.Instance?.StopSfxLoop();
    }
    
    private IEnumerator FadeAlpha(Image target, float from, float to, float duration)
    {
        if (target == null) yield break;
        
        float elapsed = 0;
        Color originalColor = target.color;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            target.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(from, to, t));
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
            float t = 1 - Mathf.Pow(1 - (elapsed / duration), 3);
            targetRect.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }
        
        targetRect.localScale = targetScale;
    }
    
    private IEnumerator RunParallel(params IEnumerator[] coroutines)
    {
        var running = new System.Collections.Generic.List<bool>();
        
        foreach (var coro in coroutines)
        {
            running.Add(true);
            StartCoroutine(RunAndMark(coro, running, running.Count - 1));
        }
        
        while (running.Contains(true))
            yield return null;
    }
    
    private IEnumerator RunAndMark(IEnumerator coroutine, System.Collections.Generic.List<bool> running, int index)
    {
        yield return StartCoroutine(coroutine);
        running[index] = false;
    }
}