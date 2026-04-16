using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "Game/Game State")]
public class GameStateSO : ScriptableObject
{
    public int currentLevel = 1;
    public int currentMoney = 0;
    public int maxUnlockedLevel = 1;
    public int currentStage = 1;
}