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
    private float _totalEffectiveChance = 0f;

    private void Start()
    {
        _factory = FindObjectOfType<Factory>();
        CalculateTotalChance();
    }

    private void CalculateTotalChance()
    {
        _totalEffectiveChance = 0f;
        
        if (_figurineTypes != null)
        {
            foreach (FigurineData data in _figurineTypes)
            {
                if (data != null)
                {
                    _totalEffectiveChance += data.GetEffectiveSpawnChance();
                }
            }
        }
        
        Debug.Log($"Total effective spawn chance: {_totalEffectiveChance}");
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
        
        bool forceBadVariant = false;
        
        if (!data.isBadItem && data.canBeBadVariant && data.badSpriteVariants != null && data.badSpriteVariants.Length > 0)
        {
            forceBadVariant = Random.value < _badVariantChance;
        }
    
        Figurine figurine = Instantiate(_figurinePrefab, _spawnPoint.position, Quaternion.identity);
    
        if (figurine != null)
        {
            figurine.Initialize(_factory, data, forceBadVariant);
        }
    }

    private FigurineData GetRandomFigurineData()
    {
        if (_figurineTypes == null || _figurineTypes.Length == 0 || _totalEffectiveChance <= 0f)
        {
            return null;
        }
    
        float random = Random.value * _totalEffectiveChance;
        float cumulativeChance = 0f;
    
        foreach (FigurineData data in _figurineTypes)
        {
            if (data == null) continue;
        
            float effectiveChance = data.GetEffectiveSpawnChance();
            cumulativeChance += effectiveChance;
        
            if (random <= cumulativeChance)
            {
                Debug.Log($"Selected: {data.figurineName} (effectiveChance={effectiveChance:F2}, total={_totalEffectiveChance:F2})");
                return data;
            }
        }
    
        return _figurineTypes[_figurineTypes.Length - 1];
    }
}