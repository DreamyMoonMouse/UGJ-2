using UnityEngine;
using System.Collections;

public class FigurineSpawner : MonoBehaviour
{
    [SerializeField] private FigurineData[] _figurineTypes;
    [SerializeField] private Figurine _figurinePrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _minSpawnInterval = 0.8f;
    [SerializeField] private float _maxSpawnInterval = 1.5f;
    [SerializeField] private float _badVariantChance = 0.3f;

    private Factory _factory;
    private bool _isSpawning = false;

    private void Start()
    {
        _factory = FindObjectOfType<Factory>();
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
            SpawnFigurine();
            
            float interval = Random.Range(_minSpawnInterval, _maxSpawnInterval);
            yield return new WaitForSeconds(interval);
        }
    }

    private void SpawnFigurine()
    {
        if (_figurinePrefab == null || !_isSpawning) return;
    
        FigurineData data = GetRandomFigurineData();
    
        if (data == null) return;
        
        bool isBadVariant = false;
        
        if (!data.isBadItem && data.spriteBadVariants != null && data.spriteBadVariants.Length > 0)
        {
            isBadVariant = Random.value < _badVariantChance;
        }
    
        Figurine figurine = Instantiate(_figurinePrefab, _spawnPoint.position, Quaternion.identity);
    
        if (figurine != null)
        {
            figurine.Initialize(_factory, data, isBadVariant);
        }
    }

    private FigurineData GetRandomFigurineData()
    {
        if (_figurineTypes == null || _figurineTypes.Length == 0)
        {
            return null;
        }
    
        float random = Random.value;
        float cumulativeChance = 0;
    
        foreach (FigurineData data in _figurineTypes)
        {
            if (data == null) continue;
        
            cumulativeChance += data.spawnChance;
        
            if (random <= cumulativeChance)
            {
                return data;
            }
        }
    
        return _figurineTypes[0];
    }
}