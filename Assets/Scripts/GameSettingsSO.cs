using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/Settings")]
public class GameSettingsSO : ScriptableObject
{
    [Header("Громкость")]
    [Range(0f, 1f)] public float sfxVolume = 0.7f;
    [Range(0f, 1f)] public float musicVolume = 0.5f;

    [Header("Разрешение")]
    public int defaultWidth = 1920;
    public int defaultHeight = 1080;

    [Header("Аудио клипы")]
    public AudioClip menuMusic;
    public AudioClip letterMusic;
    public AudioClip levelMusic; 
    public AudioClip clickSound;
    
    [Header("Звуки письма")]
    public AudioClip envelopeOpenSound;
    public AudioClip stampSound;
}