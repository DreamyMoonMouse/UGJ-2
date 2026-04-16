using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _orePrefabs;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private int _maxOres = 10;
    
    private bool _isSpawning;
    private int _currentOreCount;
    
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
            if (_currentOreCount < _maxOres)
            {
                SpawnOre();
            }
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
    
    private void SpawnOre()
    {
        if (_orePrefabs.Length == 0 || _spawnPoints.Length == 0) return;
        
        int randomOre = Random.Range(0, _orePrefabs.Length);
        int randomPoint = Random.Range(0, _spawnPoints.Length);
        
        Instantiate(_orePrefabs[randomOre], _spawnPoints[randomPoint].position, Quaternion.identity);
        _currentOreCount++;
    }
    
    public void OnOreCollected()
    {
        _currentOreCount--;
    }
}