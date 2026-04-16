using UnityEngine;
using System.Collections;

public class SplashScene : MonoBehaviour
{
    [SerializeField] private SplashVideoPlayer _videoPlayer;
    [SerializeField] private SplashDataLoader _dataLoader;
    [SerializeField] private SceneTransition _sceneTransition;
    [SerializeField] private float _minSplashDuration = 3f;
    
    private void Start()
    {
        StartCoroutine(SplashSequence());
    }
    
    private IEnumerator SplashSequence()
    {
        yield return _sceneTransition.FadeOut();
        
        _videoPlayer.Initialize();
        _dataLoader.LoadAllData();
        _videoPlayer.Play();
        
        yield return _videoPlayer.WaitForCompletion(_minSplashDuration);
        
        _videoPlayer.Stop();
        
        yield return _sceneTransition.FadeIn();

        SceneLoader.LoadMainMenu();
    }
}