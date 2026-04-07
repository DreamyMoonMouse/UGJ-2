using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LetterSequences : MonoBehaviour
{
    [SerializeField] private LetterUI _ui;
    [SerializeField] private LetterAnimator _animator;
    [SerializeField] private Letter _letterController;
    [SerializeField] private GameObject _runawayButton;
    [SerializeField] private float _envelopeOpenDuration = 0.5f;
    [SerializeField] private float _fadeDuration = 1.0f;
    [SerializeField] private float _scaleDuration = 1.0f;

    [Header("Audio")]
    [SerializeField] private AudioClip _envelopeOpenSound;
    [SerializeField] private AudioClip _paperAppearSound;
    [SerializeField] private AudioClip _stampSound;
    [SerializeField] private AudioClip _gooseSurprisedSound;

    public AudioClip EnvelopeOpenSound => _envelopeOpenSound;

    public IEnumerator PlayOpenEnvelopeSequence()
    {
        _ui.EnvelopeOpen.gameObject.SetActive(true);
        _ui.EnvelopeOpen.color = new Color(1, 1, 1, 0);

        yield return _animator.RunParallel(
            _animator.FadeAlpha(_ui.EnvelopeClosed, 1, 0, _fadeDuration),
            _animator.FadeAlpha(_ui.EnvelopeOpen, 0, 1, _fadeDuration)
        );

        yield return new WaitForSeconds(_envelopeOpenDuration);

        _ui.EnvelopeOpen.gameObject.SetActive(false);
        _ui.Paper1.gameObject.SetActive(true);
        _ui.PanelLetter1.SetActive(true);

        Audio.Instance.PlaySfx(_paperAppearSound);

        yield return _animator.RunParallel(
            _animator.FadeAlpha(_ui.EnvelopeOpen, 1, 0, _fadeDuration),
            _animator.ScaleAnimation(_ui.Paper1.rectTransform, _ui.Paper1InitialScale, _scaleDuration),
            _animator.FadeAlpha(_ui.Paper1, 0, 1, _fadeDuration),
            _animator.FadeAlpha(_ui.GooseThinking, 0, 1, _fadeDuration)
        );

        _ui.EnvelopeClosed.gameObject.SetActive(false);
        _ui.TextTitle1.color = new Color(_ui.TextTitle1.color.r, _ui.TextTitle1.color.g, _ui.TextTitle1.color.b, 0);
        _ui.TextContent1.color = new Color(_ui.TextContent1.color.r, _ui.TextContent1.color.g, _ui.TextContent1.color.b, 0);
        _ui.TextContent1.gameObject.SetActive(false);

        yield return _animator.RunParallel(
            _animator.FadeAlphaUI(_ui.TextTitle1, 0, 1, _fadeDuration),
            _animator.FadeAlphaUI(_ui.TextContent1, 0, 1, _fadeDuration)
        );

        _ui.TextContent1.gameObject.SetActive(true);
        yield return StartCoroutine(_animator.TypeText(_ui.TextContent1, _ui.TextContent1.text));
        
        if (_letterController.GetCurrentLevel() == 2)
        {
            _ui.ShowRefuseButton(true);
        }
        
        if (_letterController.GetCurrentLevel() == 3 && _runawayButton != null)
        {
            _runawayButton.SetActive(true);
        }
        
        _ui.ButtonAccept.gameObject.SetActive(true);
        Image acceptBtnImage = _ui.ButtonAccept.GetComponent<Image>();
        
        if (acceptBtnImage != null)
        {
            acceptBtnImage.color = new Color(acceptBtnImage.color.r, acceptBtnImage.color.g, acceptBtnImage.color.b, 1);
        }

        Audio.Instance.PlayClick();
    }

    public IEnumerator PlaySecondLetterSequence()
    {
        if (_letterController.GetCurrentLevel() == 2)
        {
            _ui.ShowRefuseButton(false);
        }
        
        if (_letterController.GetCurrentLevel() == 3 && _runawayButton != null)
        {
            _runawayButton.SetActive(false);
        }
        
        _ui.ShowLetter2();
        Audio.Instance.PlaySfx(_paperAppearSound);

        yield return _animator.RunParallel(
            _animator.FadeAlpha(_ui.GooseThinking, 1, 0, _fadeDuration),
            _animator.ScaleAnimation(_ui.Paper2.rectTransform, _ui.Paper2InitialScale, _scaleDuration),
            _animator.FadeAlpha(_ui.Paper2, 0, 1, _fadeDuration)
        );

        _ui.TextTitle2.color = new Color(_ui.TextTitle2.color.r, _ui.TextTitle2.color.g, _ui.TextTitle2.color.b, 1);
        _ui.TextContent2.color = new Color(_ui.TextContent2.color.r, _ui.TextContent2.color.g, _ui.TextContent2.color.b, 1);
        _ui.ShowStamp();
        Audio.Instance.PlaySfx(_stampSound);
        _ui.GooseSurprised.color = new Color(_ui.GooseSurprised.color.r, _ui.GooseSurprised.color.g, _ui.GooseSurprised.color.b, 1);
        Audio.Instance.PlaySfx(_gooseSurprisedSound);
        _ui.ShowStartWorkButton();
        
        Image workBtnImage = _ui.ButtonStartWork.GetComponent<Image>();
        
        if (workBtnImage != null)
        {
            workBtnImage.color = new Color(workBtnImage.color.r, workBtnImage.color.g, workBtnImage.color.b, 1);
        }

        Audio.Instance.PlayClick();
    }
}