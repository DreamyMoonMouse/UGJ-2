using UnityEngine;
using UnityEngine.Events;

public class VictoryCondition : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Debt _debt;
    [SerializeField] private int _targetAmount;
    
    public UnityEvent<bool> OnConditionChecked;
    
    public void SetTarget(int amount) => _targetAmount = amount;
    
    public void CheckCondition()
    {
        int netBalance = _wallet.Balance - _debt.Amount;
        bool isVictory = netBalance >= _targetAmount;
        OnConditionChecked?.Invoke(isVictory);
    }
}