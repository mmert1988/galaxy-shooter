using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject _asteroidPrefab;

    [SerializeField]
    private GameObject[] _powerUps;

    [SerializeField]
    private bool _isSpawningEnabled = true;

    [SerializeField]
    private float _enemySpawnFrequency = 10.0f;

    [SerializeField]
    private float _asteroidSpawnFrequency = 1.0f;

    void Start()
    {
        StartCoroutine(SpawnEnemiesRoutine());
        //StartCoroutine(SpawnAsteroidsRoutine());
        StartCoroutine(SpawnPowerUpsRoutine());
    }

    public void StopSpawning()
    {
        _isSpawningEnabled = false;
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (_isSpawningEnabled)
        {
            float x = Random.Range(-9.0f, 9.0f);
            Vector3 newEnemyPosition = new Vector3(x, 8, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, newEnemyPosition, Quaternion.identity);
            newEnemy.transform.SetParent(_enemyContainer.transform);
            yield return new WaitForSeconds(_enemySpawnFrequency);
        }
    }

    IEnumerator SpawnAsteroidsRoutine()
    {
        while (_isSpawningEnabled)
        {
            float x = Random.Range(-9.0f, 9.0f);
            Vector3 newAsteroidPosition = new Vector3(x, 8, 0);
            Instantiate(_asteroidPrefab, newAsteroidPosition, Quaternion.identity);
            yield return new WaitForSeconds(_asteroidSpawnFrequency);
        }
    }

    IEnumerator SpawnPowerUpsRoutine()
    {
        while (_isSpawningEnabled)
        {
            yield return new WaitForSeconds(Random.Range(5.0f, 10.0f));
            int powerUpIndex = Random.Range(0, _powerUps.Length);
            Instantiate(_powerUps[powerUpIndex], new Vector3(Random.Range(-9.0f, 9.0f), 8, 0), Quaternion.identity);
        }
    }
}
