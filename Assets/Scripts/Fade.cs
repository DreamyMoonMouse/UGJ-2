using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fade : MonoBehaviour
{
    public static Fade Instance { get; private set; }
    
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeDuration = 1f;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void FadeIn() {
        StartCoroutine(Start(0, 1));
    }

    public void FadeOut() {
        StartCoroutine(Start(1, 0));
    }

    IEnumerator Start(float from, float to) {
        float elapsed = 0;
        while (elapsed < fadeDuration) {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}