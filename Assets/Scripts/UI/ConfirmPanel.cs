using UnityEngine;
using UnityEngine.UI;

public class ConfirmPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;
    [SerializeField] private SfxPlayer _sfxPlayer;
    [SerializeField] private AudioClip _clickSound;
    
    private System.Action _onYes;
    private System.Action _onNo;
    
    private void Awake()
    {
        _yesButton.onClick.AddListener(OnYesClicked);
        _noButton.onClick.AddListener(OnNoClicked);
    }
    
    public void Show(System.Action onYes, System.Action onNo)
    {
        _onYes = onYes;
        _onNo = onNo;
        _panel.SetActive(true);
    }
    
    private void OnYesClicked()
    {
        _sfxPlayer?.Play(_clickSound);
        _panel.SetActive(false);
        _onYes?.Invoke();
    }
    
    private void OnNoClicked()
    {
        _sfxPlayer?.Play(_clickSound);
        _panel.SetActive(false);
        _onNo?.Invoke();
    }
}