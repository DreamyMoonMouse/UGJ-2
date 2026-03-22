using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Ore : MonoBehaviour
{
    [Header("Настройки руды")]
    [SerializeField] private int _minClicks = 3;
    [SerializeField] private int _maxClicks = 7;
    [SerializeField] private float _shakeDuration = 0.2f;
    
    [Header("Ссылки на объекты")]
    [SerializeField] private GameObject _dropPrefab;
    [SerializeField] private AudioClip[] _breakSounds;
    
    [Header("Анимация гуся")]
    [SerializeField] private Animator _gooseAnimator;
    [SerializeField] private string _animationTriggerName = "Mine";

    private int _clicksRemaining;
    private Mine _mineController;
    private Spawner _spawner;
    private Vector3 _originalPosition;
    private bool _isShaking = false;
    private bool _isMouseOver = false;
    private Camera _mainCamera;

    void Awake()
    {
        _mainCamera = Camera.main;
        _originalPosition = transform.position;
        _mineController = FindObjectOfType<Mine>();
        _clicksRemaining = Random.Range(_minClicks, _maxClicks + 1);
    }

    void Update()
    {
        if (_isShaking) return;

        Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            if (!_isMouseOver)
            {
                _isMouseOver = true;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                OnClick();
            }
        }
        else
        {
            _isMouseOver = false;
        }
    }

    void OnClick()
    {
        _clicksRemaining--;
        _isShaking = true;

        if (Audio.Instance != null && _breakSounds != null && _breakSounds.Length > 0)
        {
            AudioClip randomSound = _breakSounds[Random.Range(0, _breakSounds.Length)];
            Audio.Instance.PlaySfx(randomSound);
        }

        if (_gooseAnimator != null)
        {
            _gooseAnimator.SetTrigger(_animationTriggerName);
        }

        StartCoroutine(ShakeAnimation());

        if (_clicksRemaining <= 0)
        {
            Invoke(nameof(SpawnDropItem), _shakeDuration);
            _clicksRemaining = Random.Range(_minClicks, _maxClicks + 1);
        }
    }

    IEnumerator ShakeAnimation()
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0;
        
        while (elapsed < _shakeDuration)
        {
            elapsed += Time.deltaTime;
            float offsetX = Random.Range(-0.10f, 0.10f);
            float offsetY = Random.Range(-0.10f, 0.10f);
            transform.position = startPosition + new Vector3(offsetX, offsetY, 0);
            yield return null;
        }
        
        transform.position = startPosition;
        _isShaking = false;
    }

    void SpawnDropItem()
    {
        if (_dropPrefab != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0.8f, 1.2f), 0);
            GameObject droppedItem = Instantiate(_dropPrefab, spawnPos, Quaternion.identity);
        
            DragDropItem dragDrop = droppedItem.GetComponent<DragDropItem>();
            
            if (dragDrop != null)
            {
                dragDrop.SetMineController(_mineController);
            }
        }
    }

    public void SetSpawner(Spawner spawner)
    {
        this._spawner = spawner;
    }

    void OnDestroy()
    {
        if (_spawner != null)
        {
            _spawner.OnOreBroken();
        }
    }
}