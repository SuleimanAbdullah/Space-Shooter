using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private bool _isDead = false;

    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _PowerupPrefabs;

    [SerializeField]
    private GameObject _medkitPrefab;
    [SerializeField]
    private GameObject _missilePowerupPrefab;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void StartSpawning()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(SpawntPowerupsRoutine());
        StartCoroutine(SpawnMedKitRoutine());
        StartCoroutine(SpawnPowerupRarely());
        
    }

    IEnumerator EnemySpawnRoutine()
    {
        while (_isDead == false)

        {
            yield return new WaitForSeconds(3f);
            Vector3 randomPos = new Vector3(Random.Range(-9, 9), 8, 0);
            var enemy = Instantiate(_enemyPrefab, randomPos, Quaternion.identity);
            enemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SpawntPowerupsRoutine()
    {
        while (_isDead == false)
        {
            yield return new WaitForSeconds(3f);
            Vector3 randomPos = new Vector3(Random.Range(-9, 9), 8, 0);
            Instantiate(_PowerupPrefabs[Random.Range(0,4)], randomPos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 8f));
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
