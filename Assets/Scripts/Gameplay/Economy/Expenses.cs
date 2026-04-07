using System;
using UnityEngine;

public class ExpenseCalculator : MonoBehaviour
{
    public int Calculate(int income)
    {
        int expense = Mathf.Max(50000, Mathf.FloorToInt(income * 0.3f));
        return expense * 3; 
    }
}