using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class LetterAnimator : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 1.0f;
    [SerializeField] private float _scaleDuration = 1.0f;
    [SerializeField] private float _textTypingSpeed = 0.03f;

    private bool _isTyping = false;
    public bool IsTyping => _isTyping;

    public IEnumerator FadeAlpha(Image target, float from, float to, float duration = -1f)
    {
        if (duration < 0) duration = _fadeDuration;
        
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

    public IEnumerator FadeAlphaUI(TextMeshProUGUI target, float from, float to, float duration = -1f)
    {
        if (duration < 0) duration = _fadeDuration;
        
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

    public IEnumerator ScaleAnimation(RectTransform targetRect, Vector3 targetScale, float duration = -1f)
    {
        if (duration < 0) duration = _scaleDuration;
        
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

    public IEnumerator TypeText(TextMeshProUGUI textField, string fullText)
    {
        _isTyping = true;
        textField.text = "";

        foreach (char c in fullText)
        {
            textField.text += c;
            yield return new WaitForSeconds(_textTypingSpeed);
        }

        _isTyping = false;
    }

    public IEnumerator RunParallel(params IEnumerator[] coroutines)
    {
        var running = new List<bool>();
        
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

    private IEnumerator RunAndMark(IEnumerator coroutine, List<bool> running, int index)
    {
        yield return StartCoroutine(coroutine);
        running[index] = false;
    }

    private bool ContainsTrue(List<bool> list)
    {
        foreach (var value in list)
        {
            if (value) return true;
        }
        
        return false;
    }
}