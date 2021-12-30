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
    private GameObject _tripleShotPowerupPrefab;
    [SerializeField]
    private GameObject _speedBoostPowerupPrefab;
    [SerializeField]
    private GameObject _ShieldPowerupPrefab;

    void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(SpawnTripleShotPowerupRoutine());
        StartCoroutine(SpawnSpeedBoostPowerDownRoutine());
        StartCoroutine(SpawnShieldPowerupRoutine());
    }

    IEnumerator EnemySpawnRoutine()
    {
        while (_isDead == false)
        {
            Vector3 randomPos = new Vector3(Random.Range(-9, 9), 8, 0);
            var newEnemy = Instantiate(_enemyPrefab, randomPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator SpawnTripleShotPowerupRoutine()
    {
        while (_isDead == false)
        {
            Vector3 randomPos = new Vector3(Random.Range(-9, 9), 8, 0);
            Instantiate(_tripleShotPowerupPrefab, randomPos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }

    IEnumerator SpawnSpeedBoostPowerDownRoutine()
    {
        while (_isDead == false)
        {
            Vector3 randomPos = new Vector3(Random.Range(-9, 9), 8, 0);
            Instantiate(_speedBoostPowerupPrefab, randomPos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(4f, 8f));
        }
    }

    IEnumerator SpawnShieldPowerupRoutine()
    {
        while (_isDead == false)
        {
            Vector3 randomPos = new Vector3(Random.Range(-9, 9), 8, 0);
            Instantiate(_ShieldPowerupPrefab, randomPos, Quaternion.identity);
            yield return new WaitForSeconds(5f);
        }
    }

    public void OnPlayerDeath()
    {
        _isDead = true;
    }
}
