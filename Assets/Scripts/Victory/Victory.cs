using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Victory : MonoBehaviour
{
    [Header("Аудио")]
    [SerializeField] private AudioClip _bgmMusic;
    [SerializeField] private AudioClip _ambientSound;

    [Header("Конверт")]
    [SerializeField] private VictoryEnvelope _envelopeAnimator;

    [Header("Кнопки")]
    [SerializeField] private Button _closeButton;

    private void Awake()
    {
        Time.timeScale = 1f;
        
        if (_closeButton != null)
        {
            _closeButton.onClick.AddListener(OnCloseClicked);
        }
    }

    private void Start()
    {
        if (Audio.Instance != null)
        {
            if (_bgmMusic != null)
            {
                Audio.Instance.PlayMusic(_bgmMusic);
            }
            
            if (_ambientSound != null)
            {
                Audio.Instance.PlayAmbient(_ambientSound);
            }
        }
    }

    private void OnCloseClicked()
    {
        if (Audio.Instance != null)
        {
            Audio.Instance.FadeOutMusic(1f);
            Audio.Instance.StopAmbient();
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
        
        if (_closeButton != null)
        {
            _closeButton.onClick.RemoveListener(OnCloseClicked);
        }
    }
}