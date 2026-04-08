using UnityEngine;
using TMPro;

public class BalanceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _balanceText;
    [SerializeField] private Color _positiveColor = Color.green;
    [SerializeField] private Color _negativeColor = Color.red;

    private void Update()
    {
        if (Economy.Instance != null)
        {
            UpdateBalanceDisplay(Economy.Instance.GlobalBalance);
        }
    }

    public void UpdateBalanceDisplay(int balance)
    {
        if (_balanceText != null)
        {
            _balanceText.text = $"{balance:N0} ₽";
            _balanceText.color = balance >= 0 ? _positiveColor : _negativeColor;
        }
    }
}
