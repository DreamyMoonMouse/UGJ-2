using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public static void LoadLetterScene() {
        SceneManager.LoadScene("Letter_Level_1");
    }

    public static void LoadLevel(int levelNumber) {
        string sceneName = levelNumber switch {
            1 => "Level1_Mine",
            2 => "Level2_Factory",
            3 => "Level3_Warehouse",
            _ => "MainMenu"
        };
        SceneManager.LoadScene(sceneName);
    }

    public static void LoadGameOver() {
        SceneManager.LoadScene("GameOver");
    }

    public static void LoadVictory() {
        SceneManager.LoadScene("Victory");
    }
}