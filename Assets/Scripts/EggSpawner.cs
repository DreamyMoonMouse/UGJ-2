using UnityEngine;
using System.Collections;

public class EggSpawner : MonoBehaviour
{
    [SerializeField] private EggData[] _eggTypes;
    [SerializeField] private Egg _eggPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _minSpawnInterval = 0.5f;
    [SerializeField] private float _maxSpawnInterval = 1.2f;

    private EggsPlant _eggsPlant;
    private bool _isSpawning = false;

    private void Start()
    {
        _eggsPlant = FindObjectOfType<EggsPlant>();
        
        if (_spawnPoints == null || _spawnPoints.Length == 0)
        {
            Debug.LogError("EggSpawner: Нет точек спавна!");
        }
    }

    public void StartSpawning()
    {
        if (_isSpawning) return;
        
        _isSpawning = true;
        StartCoroutine(SpawnCycle());
    }

    public void StopSpawning()
    {
        _isSpawning = false;
    }

    private IEnumerator SpawnCycle()
    {
        while (_isSpawning)
        {
            SpawnEgg();
            
            float interval = Random.Range(_minSpawnInterval, _maxSpawnInterval);
            yield return new WaitForSeconds(interval);
        }
    }

    private void SpawnEgg()
    {
        if (_eggPrefab == null || !_isSpawning) return;
        if (_spawnPoints == null || _spawnPoints.Length == 0) return;
        if (_eggTypes == null || _eggTypes.Length == 0) return;
        
        EggData data = GetRandomEggData();
        if (data == null) return;
        
        int spawnIndex = Random.Range(0, _spawnPoints.Length);
        Transform spawnPoint = _spawnPoints[spawnIndex];
        
        Egg egg = Instantiate(_eggPrefab, spawnPoint.position, spawnPoint.rotation);
        
        if (egg != null)
        {
            egg.Initialize(_eggsPlant, data);
        }
    }

    private EggData GetRandomEggData()
    {
        float random = Random.value;
        float cumulativeChance = 0;
        
        foreach (EggData data in _eggTypes)
        {
            if (data == null) continue;
            
            cumulativeChance += data.spawnChance;
            
            if (random <= cumulativeChance)
            {
                return data;
            }
        }
        
        return _eggTypes[0];
    }
}