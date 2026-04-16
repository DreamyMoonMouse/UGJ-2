using UnityEngine;
using System.Collections;

public class EggSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _eggPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _spawnInterval = 1.5f;
    [SerializeField] private int _maxEggs = 15;
    
    private bool _isSpawning;
    private int _currentEggCount;
    
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
            if (_currentEggCount < _maxEggs)
            {
                SpawnEgg();
            }
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
    
    private void SpawnEgg()
    {
        if (_eggPrefab == null || _spawnPoints.Length == 0) return;
        
        int randomPoint = Random.Range(0, _spawnPoints.Length);
        GameObject egg = Instantiate(_eggPrefab, _spawnPoints[randomPoint].position, Quaternion.identity);
        
        Egg eggComponent = egg.GetComponent<Egg>();
        if (eggComponent != null)
        {
            // eggComponent.OnDestroyed += OnEggDestroyed;
        }
        
        _currentEggCount++;
    }
    
    private void OnEggDestroyed()
    {
        _currentEggCount--;
    }
}