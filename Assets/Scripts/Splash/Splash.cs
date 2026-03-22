using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class Splash : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] string nextSceneName = "MainMenu";
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] float initialBlackDuration = 1f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(StartSequence());
    }
    
    IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(initialBlackDuration);
        
        int waitCount = 0;
        while (Fade.Instance == null && waitCount < 60)
        {
            yield return null;
            waitCount++;
        }
        
        if (Fade.Instance != null)
        {
            Fade.Instance.FadeOut();
            yield return new WaitForSeconds(fadeDuration);
        }
        
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
            videoPlayer.Play();
        }
        else
        {
            Invoke(nameof(StartFadeSequence), 5f);
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        StartFadeSequence();
    }

    void StartFadeSequence()
    {
        videoPlayer.loopPointReached -= OnVideoFinished;
        StartCoroutine(FadeAndLoadSequence());
    }

    IEnumerator FadeAndLoadSequence()
    {
        yield return new WaitForEndOfFrame();
        
        if (Fade.Instance != null)
        {
            Fade.Instance.FadeIn();
            yield return new WaitForSeconds(fadeDuration);
        }
        
        SceneManager.LoadScene(nextSceneName);
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}