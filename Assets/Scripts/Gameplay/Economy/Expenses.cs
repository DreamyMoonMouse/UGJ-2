using UnityEngine;

public class Expenses : MonoBehaviour
{
    [SerializeField] private int _minExpensePerCategory = 50000;
    [SerializeField] private float _expenseRate = 0.3f;
    [SerializeField] private int _expenseCategories = 3;
    
    public int Calculate(int income)
    {
        int expensePerCategory = Mathf.Max(_minExpensePerCategory, Mathf.FloorToInt(income * _expenseRate));
        return expensePerCategory * _expenseCategories;
    }
}