using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadMainMenu()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.FadeOutMusic(1f);
        }
        SceneManager.LoadScene("MainMenu");
    }

    public static void LoadLetterScene(int level = 1)
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.FadeOutMusic(1f);
        }
        SceneManager.LoadScene("Letter_Level_1");
    }

    public static void LoadLevel(int levelNumber)
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.FadeOutMusic(1f);
        }
        
        string sceneName = levelNumber switch
        {
            1 => "Level1_Mine",
            2 => "Level2_Factory",
            3 => "Level3_EggsPlant",
            _ => "MainMenu"
        };
        SceneManager.LoadScene(sceneName);
    }

    public static void LoadGameOver()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.FadeOutMusic(1f);
        }
        SceneManager.LoadScene("GameOver");
    }

    public static void LoadVictory()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.FadeOutMusic(1f);
        }
        SceneManager.LoadScene("Victory");
    }

    public static void LoadLoseVictory()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.FadeOutMusic(1f);
        }
        SceneManager.LoadScene("LoseVictory");
    }
}