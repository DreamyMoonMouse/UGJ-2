using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/LevelConfig")]
public class LevelConfigSO : ScriptableObject
{
    public float duration;
    public int debtAmount;
    public AudioClip music;
    public LevelType type; 
}