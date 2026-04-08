using UnityEngine;
using UnityEngine.Events;

public class Score : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    
    public UnityEvent<int> OnScoreChanged;
    
    private int _levelEarnings;
    
    public int LevelEarnings => _levelEarnings;
    
    public void Add(int amount)
    {
        _levelEarnings += amount;
        _wallet.Add(amount);
        OnScoreChanged?.Invoke(_levelEarnings);
    }
    
    public void Reset() => _levelEarnings = 0;
}