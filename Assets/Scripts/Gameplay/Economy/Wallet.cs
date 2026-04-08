using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private int _balance;
    
    public event Action<int> OnChanged;
    public int Balance => _balance;
    public bool CanAfford(int amount) => _balance >= amount;
    
    public void Add(int amount) 
    { 
        _balance += amount; 
        OnChanged?.Invoke(_balance);
    }
    
    public void Set(int amount)
    {
        _balance = amount;
        OnChanged?.Invoke(_balance);
    }
}