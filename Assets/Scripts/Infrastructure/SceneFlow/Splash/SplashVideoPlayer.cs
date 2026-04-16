using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class SplashVideoPlayer : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private RawImage _videoDisplay;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private RenderTexture _renderTexture;
    
    public bool IsPlaying => _videoPlayer.isPlaying;
    
    public void Initialize()
    {
        _videoPlayer.targetTexture = _renderTexture;
        _videoDisplay.texture = _renderTexture;
        _videoPlayer.SetTargetAudioSource(0, _audioSource);
    }
    
    public void Play()
    {
        _videoPlayer.Play();
    }
    
    public void Stop()
    {
        _videoPlayer.Stop();
    }
    
    public IEnumerator WaitForCompletion(float minDuration)
    {
        float startTime = Time.time;
        
        while (true)
        {
            float elapsed = Time.time - startTime;
            bool videoFinished = !_videoPlayer.isPlaying && elapsed > 0.5f;
            bool minTimeReached = elapsed >= minDuration;
            
            if ((videoFinished && minTimeReached) || minTimeReached)
                break;
                
            yield return null;
        }
    }
}