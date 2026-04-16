using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public static class SceneLoader
{
    public static void LoadMainMenu() => SceneManager.LoadScene("MainMenu");
    public static void LoadLetterScene(int level) => SceneManager.LoadScene("Letter");
    public static void LoadLevel(int level)
    {
        string sceneName = level switch
        {
            1 => "Level1_Mine",
            2 => "Level2_Factory",
            3 => "Level3_EggsPlant",
            _ => "MainMenu"
        };
        SceneManager.LoadScene(sceneName);
    }
    
    public static void LoadVictory() => SceneManager.LoadScene("Victory");
    public static void LoadLoseVictory() => SceneManager.LoadScene("LoseVictory");
    public static void ReloadCurrentScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    
    public static IEnumerator LoadWithFade(SceneTransition transition, string sceneName)
    {
        yield return transition.FadeIn();
        SceneManager.LoadScene(sceneName);
    }
}