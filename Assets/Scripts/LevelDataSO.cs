using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelDataSO : ScriptableObject
{
    [Header("Информация об уровне")]
    public string levelName;
    public int levelNumber;

    [Header("Родственник")]
    public string relativeName;      // "двоюродный дядя Борис"
    public string deathReason;       // "упал в шахту"

    [Header("Долг")]
    public int debtAmount;           // 50000
    public string workType;          // "шахта", "фабрика", "склад"

    [Header("Настройки мини-игры")]
    public float timeLimit;          // 60 секунд
    public int goalAmount;           // Сколько нужно собрать
    public int clicksPerStone;       // Для уровня 1

    [Header("Визуал")]
    public Sprite background;
}