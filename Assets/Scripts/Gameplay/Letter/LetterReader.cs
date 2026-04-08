using UnityEngine;
using System.Collections;

public class LetterReader : MonoBehaviour
{
    [SerializeField] private LetterContent _content;
    [SerializeField] private LetterAnimator _animator;
    [SerializeField] private LetterUI _ui;
    [SerializeField] private GameStateSO _gameState;
    
    private LetterTextSO _currentLetter;
    
    public void StartReading()
    {
        _currentLetter = _content.GetCurrentLetter();
        if (_currentLetter == null) return;
        
        StartCoroutine(ReadSequence());
    }
    
    private IEnumerator ReadSequence()
    {
        _ui.Initialize();
        
        yield return ShowEnvelope();
        yield return ShowLetter1();
        
        if (_currentLetter.hasRefuseButton)
        {
            _ui.ShowRefuseButton(true);
        }
        
        yield return WaitForAccept();
        
        if (_gameState.currentStage >= 2)
        {
            yield return ShowLetter2();
        }
        
        yield return ShowStartWorkButton();
    }
    
    private IEnumerator ShowEnvelope()
    {
        yield return _animator.FadeAlpha(_ui.EnvelopeClosed, 0, 1);
    }
    
    private IEnumerator ShowLetter1()
    {
        _ui.EnvelopeClosed.gameObject.SetActive(false);
        _ui.EnvelopeOpen.gameObject.SetActive(true);
        
        yield return _animator.FadeAlpha(_ui.EnvelopeOpen, 0, 1);
        
        _ui.PanelLetter1.SetActive(true);
        yield return _animator.ScaleAnimation(_ui.Paper1.rectTransform, _ui.Paper1InitialScale);
        yield return _animator.FadeAlpha(_ui.Paper1, 0, 1);
        
        yield return _animator.FadeAlphaUI(_ui.TextTitle1, 0, 1);
        yield return _animator.TypeText(_ui.TextContent1, _currentLetter.content1);
    }
    
    private IEnumerator ShowLetter2()
    {
        _ui.ShowLetter2();
        yield return _animator.ScaleAnimation(_ui.Paper2.rectTransform, _ui.Paper2InitialScale);
        yield return _animator.FadeAlpha(_ui.Paper2, 0, 1);
        
        yield return _animator.FadeAlphaUI(_ui.TextTitle2, 0, 1);
        yield return _animator.TypeText(_ui.TextContent2, _currentLetter.content2);
        
        _ui.ShowStamp();
    }
    
    private IEnumerator WaitForAccept()
    {
        _ui.ButtonAccept.gameObject.SetActive(true);
        
        bool accepted = false;
        _ui.ButtonAccept.onClick.AddListener(() => accepted = true);
        
        yield return new WaitUntil(() => accepted);
        
        _ui.ButtonAccept.onClick.RemoveAllListeners();
    }
    
    private IEnumerator ShowStartWorkButton()
    {
        _ui.ShowStartWorkButton();
        
        bool started = false;
        _ui.ButtonStartWork.onClick.AddListener(() => started = true);
        
        yield return new WaitUntil(() => started);
        
        _ui.ButtonStartWork.onClick.RemoveAllListeners();
        
        SceneLoader.LoadLevel(_currentLetter.targetLevel);
    }
}