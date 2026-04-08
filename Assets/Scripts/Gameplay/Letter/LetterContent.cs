using UnityEngine;

public class LetterContent : MonoBehaviour
{
    [SerializeField] private LetterTextSO[] _letterTexts;
    [SerializeField] private GameStateSO _gameState;
    
    public LetterTextSO GetCurrentLetter()
    {
        int index = _gameState.currentLevel - 1;
        if (index < 0 || index >= _letterTexts.Length) return null;
        return _letterTexts[index];
    }
}