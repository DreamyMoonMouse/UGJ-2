using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private ItemData[] _itemTypes;
    [SerializeField] private float _minSpawnTime = 1f;
    [SerializeField] private float _maxSpawnTime = 2f;
    [SerializeField] private int _maxItemsOnScreen = 5;

    [Header("Зона спавна")]
    [SerializeField] private float _spawnAreaWidth = 10f;
    [SerializeField] private float _spawnAreaHeight = 6f;
    [SerializeField] private Vector2 _spawnAreaCenter = new Vector2(0, 0);

    private Mine _mineController;
    private int _currentItemsOnScreen = 0;

    private void Start()
    {
        _mineController = FindObjectOfType<Mine>();
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (_mineController != null && _mineController.IsGameActive())
        {
            if (_currentItemsOnScreen < _maxItemsOnScreen)
            {
                Spawn();
            }
            
            float spawnTime = Random.Range(_minSpawnTime, _maxSpawnTime);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    private void Spawn()
    {
        float spawnX = _spawnAreaCenter.x + Random.Range(-_spawnAreaWidth / 2, _spawnAreaWidth / 2);
        float spawnY = _spawnAreaCenter.y + Random.Range(-_spawnAreaHeight / 2, _spawnAreaHeight / 2);
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);
        
        GameObject item = Instantiate(_itemPrefab, spawnPos, Quaternion.identity);
        _currentItemsOnScreen++;
        
        DragDropItem itemComponent = item.GetComponent<DragDropItem>();
        
        if (itemComponent != null)
        {
            ItemData data = GetRandomItemData();
            itemComponent.Initialize(_mineController, data);
        }
    }

    private ItemData GetRandomItemData()
    {
        if (_itemTypes == null || _itemTypes.Length == 0)
        {
            return null;
        }

        float random = Random.value;
        float cumulativeChance = 0;

        foreach (ItemData data in _itemTypes)
        {
            if (data == null) continue;

            cumulativeChance += data.spawnChance;

            if (random <= cumulativeChance)
            {
                return data;
            }
        }

        return _itemTypes[0];
    }

    public void OnItemBroken()
    {
        _currentItemsOnScreen--;
    }

    public int GetCurrentItemsCount()
    {
        return _currentItemsOnScreen;
    }
}