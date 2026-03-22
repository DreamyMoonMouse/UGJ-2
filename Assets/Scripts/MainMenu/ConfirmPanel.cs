using UnityEngine;
using UnityEngine.UI;

public class ConfirmPanel : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    void Awake() {
        if (panel != null) {
            panel.SetActive(false);
        }
        
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    public void Show() {
        panel.SetActive(true);
        Time.timeScale = 0f; // Пауза игры
    }

    public void Hide() {
        panel.SetActive(false);
        Time.timeScale = 1f; // Продолжение игры
    }

    void OnYesClicked() {
        Audio.Instance.PlayClick();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnNoClicked() {
        Audio.Instance.PlayClick();
        Hide();
    }

    void OnDestroy() {
        yesButton.onClick.RemoveListener(OnYesClicked);
        noButton.onClick.RemoveListener(OnNoClicked);
    }
}