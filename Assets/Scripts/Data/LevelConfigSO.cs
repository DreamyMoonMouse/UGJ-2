using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
public class LevelConfigSO : ScriptableObject
{
    public float duration;
    public int debtAmount;
    public int targetBalance;
    public AudioClip music;
    public LevelType type;
}

public enum LevelType
{
    Mining,
    Sorting,
    Catching
}