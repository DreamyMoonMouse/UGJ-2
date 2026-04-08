using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelResultUI : MonoBehaviour
{
    [SerializeField] private GameObject _victoryPanel;
    [SerializeField] private GameObject _defeatPanel;
    [SerializeField] private TextMeshProUGUI _finalBalanceText;
    [SerializeField] private TextMeshProUGUI _expensesText;
    [SerializeField] private TextMeshProUGUI _netBalanceText;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _retryButton;
    
    public void ShowVictory(int finalBalance, int expenses, int netBalance)
    {
        _victoryPanel.SetActive(true);
        _finalBalanceText.text = finalBalance.ToString();
        _expensesText.text = expenses.ToString();
        _netBalanceText.text = netBalance.ToString();
    }
    
    public void ShowDefeat()
    {
        _defeatPanel.SetActive(true);
    }
    
    public void HideAll()
    {
        _victoryPanel.SetActive(false);
        _defeatPanel.SetActive(false);
    }
}