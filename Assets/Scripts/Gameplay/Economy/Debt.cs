using System;
using UnityEngine;

public class Debt : MonoBehaviour
{
    [SerializeField] private int _amount;
    
    public int Amount => _amount;
    public void Set(int amount) => _amount = amount;
}
