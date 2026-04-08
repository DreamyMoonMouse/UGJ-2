using UnityEngine;

public class GameCompletion : MonoBehaviour
{
    [SerializeField] private GameStateSO _gameState;
    
    public void CheckCompletion()
    {
        if (_gameState.currentLevel >= 3)
        {
            bool hasPositiveBalance = _gameState.currentMoney > 0;
            
            if (hasPositiveBalance)
                SceneLoader.LoadVictory();
            else
                SceneLoader.LoadLoseVictory();
        }
    }
}