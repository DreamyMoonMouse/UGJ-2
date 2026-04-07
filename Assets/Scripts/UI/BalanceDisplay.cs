using System;
using TMPro;
using UnityEngine;

public class BalanceDisplay : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TextMeshProUGUI _text;
    
    private void OnEnable() => _wallet.OnChanged += UpdateDisplay;
    private void OnDisable() => _wallet.OnChanged -= UpdateDisplay;
    
    private void UpdateDisplay(int balance) => _text.text = $"{balance:N0} ₽";
}