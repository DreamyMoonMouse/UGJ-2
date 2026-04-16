using UnityEngine;
using System.Collections;

public class FigurineSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _figurinePrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _spawnInterval = 1f;
    [SerializeField] private int _maxFigurines = 20;
    
    private bool _isSpawning;
    private int _currentCount;
    
    public void StartSpawning()
    {
        _isSpawning = true;
        StartCoroutine(SpawnRoutine());
    }
    
    public void StopSpawning()
    {
        _isSpawning = false;
    }
    
    private IEnumerator SpawnRoutine()
    {
        while (_isSpawning)
        {
            if (_currentCount < _maxFigurines)
            {
                SpawnFigurine();
            }
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
    
    private void SpawnFigurine()
    {
        if (_figurinePrefab == null || _spawnPoints.Length == 0) return;
        
        int randomPoint = Random.Range(0, _spawnPoints.Length);
        Instantiate(_figurinePrefab, _spawnPoints[randomPoint].position, Quaternion.identity);
        
        _currentCount++;
    }
    
    public void OnFigurineDestroyed()
    {
        _currentCount--;
    }
}