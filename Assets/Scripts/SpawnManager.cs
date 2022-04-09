using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int enemyCount;
    public float timeBetweenSpawns;
}
public class SpawnManager : MonoBehaviour
{
    public Wave[] waves;
    [SerializeField]
    private GameObject _enemyPrefab;

    public Wave currentWave;
    public int currentWaveNumber;

    public int enemiesRemainingToSpawn;
    public int enemiesRemainingAlive;
    public float nextSpawnTime;

    private bool _isDead = false;

    private bool _isAsteroidDestroyed;

    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _PowerupPrefabs;

    [SerializeField]
    private GameObject _medkitPrefab;
    [SerializeField]
    private GameObject _missilePowerupPrefab;
    private UIManager _uIManager;

    private void Start()
    {
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (_isAsteroidDestroyed == true)
        {
            if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
            {
               
                SpawnEnemy();
            }
            
        }

    }
    public void StartSpawning()
    {
        _isAsteroidDestroyed = true;
        StartCoroutine(NextWave());
        StartCoroutine(SpawntPowerupsRoutine());
        StartCoroutine(SpawnMedKitRoutine());
        StartCoroutine(SpawnPowerupRarely());
    }

    void SpawnEnemy()
    {
        enemiesRemainingToSpawn--;
        nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;
        Vector3 randomPos = new Vector3(Random.Range(-9, 9), 8, 0);
        GameObject enemySpawned = Instantiate(_enemyPrefab, randomPos, Quaternion.identity);
        enemySpawned.transform.parent = _enemyContainer.transform;
        Enemy.onDeath = OnEnemyDeath;
    }

    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;
        if (enemiesRemainingAlive == 0)
        {
            StartCoroutine(NextWave());
        }
    }

    IEnumerator NextWave()
    {
        yield return new WaitForSeconds(4f);
        currentWaveNumber++;
        if (currentWaveNumber - 1 < waves.Length)
        {
            _uIManager.UpdatWaveText(currentWaveNumber);
            currentWave = waves[currentWaveNumber - 1];
            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
        }
    }



    IEnumerator SpawntPowerupsRoutine()
    {
        while (_isDead == false)
        {
            yield return new WaitForSeconds(3f);
            Vector3 randomPos = new Vector3(Random.Range(-9, 9), 8, 0);
            Instantiate(_PowerupPrefabs[Random.Range(0, 4)], randomPos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }


    IEnumerator SpawnMedKitRoutine()
    {
        while (_isDead == false)
        {
            yield return new WaitForSeconds(9f);
            Vector3 randomPos = new Vector3(Random.Range(-9, 9), 8, 0);
            Instantiate(_medkitPrefab, randomPos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(8f, 16f));

        }
    }

    IEnumerator SpawnPowerupRarely()
    {
        while (_isDead == false)
        {
            yield return new WaitForSeconds(15);
            Vector3 randomPos = new Vector3(Random.Range(-9, 9), 8, 0);
            Instantiate(_missilePowerupPrefab, randomPos, Quaternion.identity);
            yield return new WaitForSeconds(15f);
        }
    }

    public void OnPlayerDeath()
    {
        _isDead = true;
    }
}
