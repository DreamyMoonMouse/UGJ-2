using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LetterUI : MonoBehaviour
{
    [Header("Envelope")]
    [SerializeField] private Image _envelopeClosed;
    [SerializeField] private Image _envelopeOpen;

    [Header("Papers")]
    [SerializeField] private GameObject _panelLetter1;
    [SerializeField] private GameObject _panelLetter2;
    [SerializeField] private Image _paper1;
    [SerializeField] private Image _paper2;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _textTitle1;
    [SerializeField] private TextMeshProUGUI _textContent1;
    [SerializeField] private TextMeshProUGUI _textTitle2;
    [SerializeField] private TextMeshProUGUI _textContent2;

    [Header("Buttons")]
    [SerializeField] private Button _buttonAccept;
    [SerializeField] private Button _buttonStartWork;
    [SerializeField] private Button _buttonRefuse;

    [Header("Goose")]
    [SerializeField] private Image _gooseThinking;
    [SerializeField] private Image _gooseSurprised;

    [Header("Stamp")]
    [SerializeField] private Image _stampImage;

    private Vector3 _paper1InitialScale;
    private Vector3 _paper2InitialScale;
    private Vector3 _refuseButtonInitialScale;
    private int _refuseClickCount = 0;

    public Image EnvelopeClosed => _envelopeClosed;
    public Image EnvelopeOpen => _envelopeOpen;
    public Image Paper1 => _paper1;
    public Image Paper2 => _paper2;
    public GameObject PanelLetter1 => _panelLetter1;
    public GameObject PanelLetter2 => _panelLetter2;
    public TextMeshProUGUI TextTitle1 => _textTitle1;
    public TextMeshProUGUI TextContent1 => _textContent1;
    public TextMeshProUGUI TextTitle2 => _textTitle2;
    public TextMeshProUGUI TextContent2 => _textContent2;
    public Button ButtonAccept => _buttonAccept;
    public Button ButtonStartWork => _buttonStartWork;
    public Button ButtonRefuse => _buttonRefuse;
    public Image GooseThinking => _gooseThinking;
    public Image GooseSurprised => _gooseSurprised;
    public Image StampImage => _stampImage;
    public Vector3 Paper1InitialScale => _paper1InitialScale;
    public Vector3 Paper2InitialScale => _paper2InitialScale;
    public Vector3 RefuseButtonInitialScale => _refuseButtonInitialScale;

    public void Initialize()
    {
        _envelopeClosed.gameObject.SetActive(true);
        _envelopeOpen.gameObject.SetActive(false);
        _panelLetter1.SetActive(false);
        _panelLetter2.SetActive(false);
        _stampImage.gameObject.SetActive(false);
        _buttonAccept.gameObject.SetActive(false);
        _buttonStartWork.gameObject.SetActive(false);
        
        if (_buttonRefuse != null)
        {
            _buttonRefuse.gameObject.SetActive(false);
            _refuseClickCount = 0;
        }
        
        _envelopeOpen.color = new Color(1, 1, 1, 0);
        _gooseThinking.color = new Color(1, 1, 1, 0);
        _gooseSurprised.color = new Color(1, 1, 1, 0);

        if (_paper1 != null)
        {
            _paper1InitialScale = _paper1.rectTransform.localScale;
            _paper1.rectTransform.localScale = Vector3.zero;
            _paper1.color = new Color(1, 1, 1, 0);
        }

        if (_paper2 != null)
        {
            _paper2InitialScale = _paper2.rectTransform.localScale;
            _paper2.rectTransform.localScale = Vector3.zero;
            _paper2.color = new Color(1, 1, 1, 0);
        }

        if (_buttonRefuse != null)
        {
            _refuseButtonInitialScale = _buttonRefuse.transform.localScale;
        }

        if (_textTitle1 != null) _textTitle1.color = new Color(1, 1, 1, 0);
        if (_textContent1 != null) _textContent1.color = new Color(1, 1, 1, 0);
        if (_textTitle2 != null) _textTitle2.color = new Color(1, 1, 1, 0);
        if (_textContent2 != null) _textContent2.color = new Color(1, 1, 1, 0);
    }

    public void SetLetter1Texts(string title, string content)
    {
        _textTitle1.text = title;
        _textContent1.text = content;
    }

    public void SetLetter2Texts(string title, string content)
    {
        _textTitle2.text = title;
        _textContent2.text = content;
    }

    public void ShowRefuseButton(bool show)
    {
        if (_buttonRefuse != null)
        {
            _buttonRefuse.gameObject.SetActive(show);
            _refuseClickCount = 0;
        }
    }

    public void OnRefuseButtonClicked()
    {
        _refuseClickCount++;
        
        float scale = 1f - (_refuseClickCount * 0.25f);
        
        if (scale <= 0f)
        {
            _buttonRefuse.gameObject.SetActive(false);
            _refuseClickCount = 0;
        }
        else
        {
            _buttonRefuse.transform.localScale = Vector3.one * scale;
        }
    }

    public void ShowLetter2()
    {
        _panelLetter2.SetActive(true);
        _paper2.gameObject.SetActive(true);
    }

    public void ShowStamp()
    {
        _stampImage.gameObject.SetActive(true);
    }

    public void ShowStartWorkButton()
    {
        _buttonStartWork.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        _buttonAccept.onClick.RemoveAllListeners();
        _buttonStartWork.onClick.RemoveAllListeners();
        if (_buttonRefuse != null)
        {
            _buttonRefuse.onClick.RemoveAllListeners();
        }
    }
}