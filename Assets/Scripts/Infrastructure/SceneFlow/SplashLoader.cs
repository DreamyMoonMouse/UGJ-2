using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashLoader : MonoBehaviour
{
    [SerializeField] private float _splashDuration = 3f;
    [SerializeField] private string _nextScene = "MainMenu";
    
    private void Start()
    {
        Invoke(nameof(LoadNextScene), _splashDuration);
    }
    
    private void LoadNextScene()
    {
        SceneManager.LoadScene(_nextScene);
    }
}