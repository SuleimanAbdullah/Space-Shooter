using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator EnemySpawnRoutine()
    {
        while (true)
        {
            //this loop will crush our app
            //we need to give our app a break
            Vector3 randomPos = new Vector3(Random.Range(-9, 9), 8, 0);
            var newEnemy =Instantiate(_enemyPrefab, randomPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5f);
        }
    }


}
