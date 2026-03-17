using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Letter : MonoBehaviour
{
    [Header("Ссылки на SO")]
    [SerializeField] GameStateSO gameState;
    [SerializeField] GameSettingsSO settings;

    [Header("Конверты")]
    [SerializeField] Image envelopeClosed;
    [SerializeField] Image envelopeOpen;

    [Header("Письма")]
    [SerializeField] GameObject panelLetter1;
    [SerializeField] GameObject panelLetter2;
    [SerializeField] Image paper1;
    [SerializeField] Image paper2;

    [Header("Тексты (TMP)")]
    [SerializeField] TextMeshProUGUI textTitle1;
    [SerializeField] TextMeshProUGUI textContent1;
    [SerializeField] TextMeshProUGUI textTitle2;
    [SerializeField] TextMeshProUGUI textContent2;

    [Header("Кнопки")]
    [SerializeField] Button buttonAccept;
    [SerializeField] Button buttonStartWork;

    [Header("Гусь")]
    [SerializeField] GameObject gooseThinking;
    [SerializeField] GameObject gooseSurprised;

    [Header("Штамп")]
    [SerializeField] Image stampImage;

    [Header("Тексты писем (по уровням)")]
    [TextArea(5, 10)] public string[] letterText1;
    [TextArea(5, 10)] public string[] letterText2;

    [Header("Звуки")]
    [SerializeField] AudioClip envelopeOpenSound;
    [SerializeField] AudioClip paperAppearSound;
    [SerializeField] AudioClip stampSound;
    [SerializeField] AudioClip gooseSurprisedSound;

    [Header("Настройки анимации")]
    [SerializeField] float envelopeOpenDuration = 0.5f;
    [SerializeField] float fadeDuration = 0.5f;
    [SerializeField] float scaleDuration = 0.5f;
    [SerializeField] float textTypingSpeed = 0.06f;

    private bool isTyping = false;
    private Vector3 paper1InitialScale;
    private Vector3 paper2InitialScale;
    private Image _gooseThinkingImg;
    private Image _gooseSurprisedImg;

    void Awake()
    {
        envelopeClosed.gameObject.SetActive(true);
        envelopeOpen.gameObject.SetActive(false);

        panelLetter1.SetActive(false);
        panelLetter2.SetActive(false);
        
        stampImage.gameObject.SetActive(false);

        buttonAccept.gameObject.SetActive(false);
        buttonStartWork.gameObject.SetActive(false);

        envelopeClosed.color = new Color(1, 1, 1, 1);
        envelopeOpen.color = new Color(1, 1, 1, 0);
        _gooseSurprisedImg = gooseThinking.GetComponent<Image>();
        _gooseSurprisedImg.color = new Color(1, 1, 1, 0);
        
        gooseThinking.gameObject.SetActive(false);
        gooseSurprised.gameObject.SetActive(false);

        if (paper1 != null)
        {
            paper1InitialScale = paper1.rectTransform.localScale;
            paper1.rectTransform.localScale = Vector3.zero;
            paper1.color = new Color(1, 1, 1, 0);
        }

        if (paper2 != null)
        {
            paper2InitialScale = paper2.rectTransform.localScale;
            paper2.rectTransform.localScale = Vector3.zero;
            paper2.color = new Color(1, 1, 1, 0);
        }

        buttonAccept.onClick.AddListener(OnAcceptClicked);
        buttonStartWork.onClick.AddListener(OnStartWorkClicked);
    }

    void Start()
    {
        Audio.Instance.PlayMusic(settings.letterMusic);
    }

    void SetupLetterTexts()
    {
        int levelIndex = gameState.selectedLevel - 1;

        if (levelIndex >= 0 && levelIndex < letterText1.Length)
        {
            textTitle1.text = "УВЕДОМЛЕНИЕ О НАСЛЕДСТВЕ";
            textContent1.text = letterText1[levelIndex];

            textTitle2.text = "ДОПОЛНИТЕЛЬНОЕ ИЗВЕЩЕНИЕ";
            textContent2.text = letterText2[levelIndex];
        }
    }

    public void OnEnvelopeClicked()
    {
        if (envelopeClosed.gameObject.activeSelf && !isTyping)
        {
            SetupLetterTexts();
            Audio.Instance.PlaySfx(envelopeOpenSound);
            StartCoroutine(OpenEnvelopeSequence());
        }
    }

    IEnumerator OpenEnvelopeSequence()
    {
        StartCoroutine(FadeAlpha(envelopeClosed, 1, 0, fadeDuration));
        envelopeClosed.gameObject.SetActive(false);

        envelopeOpen.gameObject.SetActive(true);
        StartCoroutine(FadeAlpha(envelopeOpen, 0, 1, fadeDuration));
        yield return new WaitForSeconds(envelopeOpenDuration);
        StartCoroutine(FadeAlpha(envelopeOpen, 1, 0, fadeDuration));
        envelopeOpen.gameObject.SetActive(false);
        
        panelLetter1.SetActive(true);
        Audio.Instance.PlaySfx(paperAppearSound);
        StartCoroutine(ScaleAnimation(paper1.rectTransform, paper1InitialScale, scaleDuration));
        StartCoroutine(FadeAlpha(paper1, 0, 1, fadeDuration));

        gooseThinking.gameObject.SetActive(true);
        StartCoroutine(FadeAlpha(_gooseThinkingImg, 0, 1, fadeDuration));

        //textTitle1.color = new Color(textTitle1.color.r, textTitle1.color.g, textTitle1.color.b, 0);
        //textContent1.color = new Color(textContent1.color.r, textContent1.color.g, textContent1.color.b, 0);
        //yield return StartCoroutine(FadeAlphaUI(textTitle1, 0, 1, fadeDuration));
        //yield return StartCoroutine(FadeAlphaUI(textContent1, 0, 1, fadeDuration));

        yield return StartCoroutine(TypeText(textContent1, textContent1.text));

        buttonAccept.gameObject.SetActive(true);
        Image acceptBtnImage = buttonAccept.GetComponent<Image>();

        if (acceptBtnImage != null)
        {
            acceptBtnImage.color = new Color(acceptBtnImage.color.r, acceptBtnImage.color.g, acceptBtnImage.color.b, 0);
            yield return StartCoroutine(FadeAlpha(acceptBtnImage, 0, 1, fadeDuration));
        }

        Audio.Instance.PlayClick();
    }

    void OnAcceptClicked()
    {
        Audio.Instance.PlayClick();
        StartCoroutine(ShowSecondLetterSequence());
    }

    IEnumerator ShowSecondLetterSequence()
    {
        panelLetter2.SetActive(true);
        paper2.gameObject.SetActive(true);

        StartCoroutine(ScaleAnimation(paper2.rectTransform, paper2InitialScale, scaleDuration));
        StartCoroutine(FadeAlpha(paper2, 0, 1, fadeDuration));

        textTitle2.color = new Color(textTitle2.color.r, textTitle2.color.g, textTitle2.color.b, 0);
        textContent2.color = new Color(textContent2.color.r, textContent2.color.g, textContent2.color.b, 0);
        yield return StartCoroutine(FadeAlphaUI(textTitle2, 0, 1, fadeDuration));
        yield return StartCoroutine(FadeAlphaUI(textContent2, 0, 1, fadeDuration));

        textContent2.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.3f);
        stampImage.gameObject.SetActive(true);
        Audio.Instance.PlaySfx(stampSound);

        yield return new WaitForSeconds(0.2f);
        gooseThinking.gameObject.SetActive(false);
        gooseSurprised.gameObject.SetActive(true);
        Audio.Instance.PlaySfx(gooseSurprisedSound);

        yield return new WaitForSeconds(0.5f);
        buttonStartWork.gameObject.SetActive(true);
        Image workBtnImage = buttonStartWork.GetComponent<Image>();
        if (workBtnImage != null)
        {
            workBtnImage.color = new Color(workBtnImage.color.r, workBtnImage.color.g, workBtnImage.color.b, 0);
            yield return StartCoroutine(FadeAlpha(workBtnImage, 0, 1, fadeDuration));
        }
        Audio.Instance.PlayClick();
    }

    void OnStartWorkClicked()
    {
        Audio.Instance.PlayClick();
        gameState.UnlockLevel(gameState.selectedLevel);
        StartCoroutine(FadeToLevel());
    }

    IEnumerator FadeToLevel()
    {
        yield return new WaitForSeconds(0.5f);
        SceneLoader.LoadLevel(gameState.selectedLevel);
    }

    IEnumerator FadeAlpha(Image target, float from, float to, float duration)
    {
        float elapsed = 0;
        Color originalColor = target.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = Mathf.SmoothStep(0, 1, t);

            float alpha = Mathf.Lerp(from, to, t);
            target.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        target.color = new Color(originalColor.r, originalColor.g, originalColor.b, to);
    }

    IEnumerator FadeAlphaUI(TextMeshProUGUI target, float from, float to, float duration)
    {
        float elapsed = 0;
        Color originalColor = target.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = Mathf.SmoothStep(0, 1, t);

            float alpha = Mathf.Lerp(from, to, t);
            target.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        target.color = new Color(originalColor.r, originalColor.g, originalColor.b, to);
    }

    IEnumerator ScaleAnimation(RectTransform targetRect, Vector3 targetScale, float duration)
    {
        float elapsed = 0;
        Vector3 startScale = targetRect.localScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = 1 - Mathf.Pow(1 - t, 3);

            targetRect.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        targetRect.localScale = targetScale;
    }

    IEnumerator TypeText(TextMeshProUGUI textField, string fullText)
    {
        isTyping = true;
        textField.text = "";

        foreach (char c in fullText)
        {
            textField.text += c;
            yield return new WaitForSeconds(textTypingSpeed);
        }

        isTyping = false;
    }

    void OnDestroy()
    {
        buttonAccept.onClick.RemoveListener(OnAcceptClicked);
        buttonStartWork.onClick.RemoveListener(OnStartWorkClicked);
    }
}