using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    private bool _isPaused;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    
    public void TogglePause()
    {
        _isPaused = !_isPaused;
        _pausePanel.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0f : 1f;
    }
    
    public void Resume() => TogglePause();
    
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneLoader.ReloadCurrentScene();
    }
    
    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        SceneLoader.LoadMainMenu();
    }
}