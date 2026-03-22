using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private GameObject orePrefab;
    [SerializeField] private float minSpawnTime = 1f;
    [SerializeField] private float maxSpawnTime = 2f;
    [SerializeField] private int maxOresOnScreen = 5;

    [Header("Зона спавна")]
    [SerializeField] private float spawnAreaWidth = 10f;
    [SerializeField] private float spawnAreaHeight = 6f;
    [SerializeField] private Vector2 spawnAreaCenter = new Vector2(0, 0);

    private Mine mineController;
    private int currentOresOnScreen = 0;

    void Start()
    {
        mineController = FindObjectOfType<Mine>();
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (mineController != null && mineController.IsGameActive())
        {
            if (currentOresOnScreen < maxOresOnScreen)
            {
                Spawn();
            }
            
            float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    void Spawn()
    {
        float spawnX = spawnAreaCenter.x + Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
        float spawnY = spawnAreaCenter.y + Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2);
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);
        
        GameObject ore = Instantiate(orePrefab, spawnPos, Quaternion.identity);
        currentOresOnScreen++;
        
        Ore oreComponent = ore.GetComponent<Ore>();
        
        if (oreComponent != null)
        {
            oreComponent.SetSpawner(this);
        }
    }

    public void OnOreBroken()
    {
        currentOresOnScreen--;
    }

    public int GetCurrentOresCount()
    {
        return currentOresOnScreen;
    }
}