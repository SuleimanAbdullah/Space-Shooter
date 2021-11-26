using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _isDead = false;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
    }

    IEnumerator EnemySpawnRoutine()
    {
        while (_isDead == false)
        {
            Vector3 randomPos = new Vector3(Random.Range(-9, 9), 8, 0);
            var newEnemy =Instantiate(_enemyPrefab, randomPos, Quaternion.identity);
            //spawn our enemy inside the parent container to keep our hierarchy clean
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5f);
        }
    }

    public void OnPlayerDeath()
    {
        _isDead = true;
    }

}
