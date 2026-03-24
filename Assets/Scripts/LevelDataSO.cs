using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelDataSO : ScriptableObject
{
    [Header("Информация об уровне")]
    public string levelName;
    public int levelNumber;

    [Header("Родственник")]
    public string relativeName;      
    public string deathReason;      

    [Header("Долг")]
    public int debtAmount;          
    public string workType;        

    [Header("Настройки мини-игры")]
    public float timeLimit;        
    public int goalAmount;           
    public int clicksPerStone;    

    [Header("Визуал")]
    public Sprite background;
}