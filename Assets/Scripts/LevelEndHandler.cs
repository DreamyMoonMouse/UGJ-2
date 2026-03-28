using UnityEngine;
using TMPro;

public class LevelEndHandler : MonoBehaviour
{
    [SerializeField] private GameObject _panelEndWin;
    [SerializeField] private GameObject _panelEndLose;
    [SerializeField] private TextMeshProUGUI _textSummary;
    [SerializeField] private TextMeshProUGUI _textExpenses;
    [SerializeField] private TextMeshProUGUI _textFinalBalance;
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private AudioClip _loseSound;

    private bool _isLevelEnded = false;

    private void Awake()
    {
        _panelEndWin.SetActive(false);
        _panelEndLose.SetActive(false);
    }

    public void EndLevel(bool isVictory, int balance, int finalBalance)
    {
        if (_isLevelEnded) return;
        _isLevelEnded = true;
        
        Time.timeScale = 0f;
        
        if (isVictory)
        {
            HandleVictory(balance, finalBalance);
        }
        else
        {
            HandleDefeat(finalBalance);
        }
    }

    private void HandleVictory(int balance, int finalBalance)
    {
        _panelEndWin.SetActive(true);
        _panelEndLose.SetActive(false);
        
        if (Audio.Instance != null)
        {
            Audio.Instance.StopMusic();
            
            if (_winSound != null)
            {
                Audio.Instance.PlaySfx(_winSound);
            }
        }
        
        int expenses = 0;
        
        if (Economy.Instance != null)
        {
            finalBalance = Economy.Instance.CalculateFinalBalance(finalBalance);
            Economy.Instance.SetBalance(finalBalance);
        }
        else
        {
            Debug.Log("Economy instance не найден. Смотри сцену мэйнменю.");
        }
        
        int result = Mathf.Max(5000, Mathf.FloorToInt(balance * 0.3f));
        expenses = Economy.Instance.CalculateExpenses(balance);
        
        _textSummary.text = $"Ура! Долг погашен!\nЗаработано за уровень: \n{balance:N0} ₽";
        _textExpenses.text = $"Ежемесячные платежи:\nЕда: -{(result):N0} ₽" +
                             $"\nАренда: -{(result):N0} ₽" +
                             $"\nКоммуналка: -{(result):N0} ₽" +
                             $"\nИтого: -{expenses:N0} ₽";
        _textFinalBalance.text = $"Итоговый баланс: {finalBalance:N0} ₽";
    }

    private void HandleDefeat(int finalBalance)
    {
        _panelEndWin.SetActive(false);
        _panelEndLose.SetActive(true);
        
        if (Audio.Instance != null)
        {
            Audio.Instance.StopMusic();
            
            if (_loseSound != null)
            {
                Audio.Instance.PlaySfx(_loseSound);
            }
        }
    }
}