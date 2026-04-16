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
    
    [Header("Audio")]
    [SerializeField] private SfxPlayer _sfxPlayer;
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
        _envelopeClosed.color = Color.white;
        
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
        
        yield return StartCoroutine(FadeAlpha(_envelopeClosed, 1, 0, _fadeDuration));
        yield return StartCoroutine(FadeAlpha(_envelopeOpen, 0, 1, _fadeDuration));
        
        _sfxPlayer?.Play(_envelopeOpenSound);
        yield return new WaitForSeconds(0.5f);
        
        _envelopeOpen.gameObject.SetActive(false);
        _paper.gameObject.SetActive(true);
        
        _sfxPlayer?.Play(_paperAppearSound);
        
        yield return StartCoroutine(FadeAlpha(_envelopeOpen, 1, 0, _fadeDuration));
        yield return StartCoroutine(ScaleAnimation(_paper.rectTransform, Vector3.one, _scaleDuration));
        yield return StartCoroutine(FadeAlpha(_paper, 0, 1, _fadeDuration));
        
        _creditsText.color = Color.white;
        _creditsText.text = "";
        
        yield return StartCoroutine(TypeCredits());
    }
    
    private IEnumerator TypeCredits()
    {
        string fullText = string.Join("\n", _creditsLines);
        
        // Звук печатания - запустить цикл
        // _sfxPlayer?.PlayLoop(_typingSound);
        
        foreach (char c in fullText)
        {
            _creditsText.text += c;
            yield return new WaitForSeconds(_typingSpeed);
        }
        
        // Остановить цикл
        // _sfxPlayer?.StopLoop();
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
}