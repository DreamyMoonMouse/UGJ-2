using UnityEngine;
using UnityEngine.UI;

public class Letter : MonoBehaviour
{
    [SerializeField] private GameStateSO _gameState;
    [SerializeField] private GameSettingsSO _settings;
    [SerializeField] private LetterData _letterData;
    [SerializeField] private LetterUI _ui;
    [SerializeField] private LetterSequences _sequences;
    [SerializeField] private LetterAnimator _animator;

    private int _currentLevel;

    public int GetCurrentLevel() => _currentLevel;

    private void Awake()
    {
        _ui.Initialize();
        _ui.ButtonAccept.onClick.AddListener(OnAcceptClicked);
        _ui.ButtonStartWork.onClick.AddListener(OnStartWorkClicked);
        
        if (_ui.ButtonRefuse != null)
        {
            _ui.ButtonRefuse.onClick.AddListener(OnRefuseClicked);
        }
    }

    private void Start()
    {
        _currentLevel = _gameState.selectedLevel;
        if (_currentLevel < 1) _currentLevel = 1;
        Audio.Instance.PlayMusic(_settings.letterMusic);
    }

    public void OnEnvelopeClicked()
    {
        if (_ui.EnvelopeClosed.gameObject.activeSelf && !_animator.IsTyping)
        {
            SetupLetterTexts();
            Audio.Instance.PlaySfx(_sequences.EnvelopeOpenSound);
            StartCoroutine(_sequences.PlayOpenEnvelopeSequence());
        }
    }

    private void SetupLetterTexts()
    {
        int index = _currentLevel - 1;
        LetterData.LetterTexts texts = _letterData.GetLevelTexts(index);
        _ui.SetLetter1Texts(texts.title1, texts.content1);
        _ui.SetLetter2Texts(texts.title2, texts.content2);
    }

    private void OnAcceptClicked()
    {
        Audio.Instance.PlayClick();
        StartCoroutine(_sequences.PlaySecondLetterSequence());
    }

    private void OnRefuseClicked()
    {
        Audio.Instance.PlayClick();
        _ui.OnRefuseButtonClicked();
    }

    private void OnStartWorkClicked()
    {
        Audio.Instance.PlayClick();
        _gameState.UnlockLevel(_currentLevel);

        if (Audio.Instance != null)
        {
            Audio.Instance.FadeOutMusic(1f);
        }

        if (Fade.Instance != null)
        {
            Fade.Instance.FadeIn();
            Invoke(nameof(LoadLevel), 1f);
        }
        else
        {
            LoadLevel();
        }
    }

    private void LoadLevel()
    {
        SceneLoader.LoadLevel(_currentLevel);
    }

    private void OnDestroy()
    {
        _ui.ButtonAccept.onClick.RemoveListener(OnAcceptClicked);
        _ui.ButtonStartWork.onClick.RemoveListener(OnStartWorkClicked);
        if (_ui.ButtonRefuse != null)
        {
            _ui.ButtonRefuse.onClick.RemoveListener(OnRefuseClicked);
        }
    }
}