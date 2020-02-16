using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerUps;
    
    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
     
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.5f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);

        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.5f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            
            Instantiate(powerUps[Random.Range(0, 6)], posToSpawn, Quaternion.identity);

            if (powerUps[4])
            {
                yield return new WaitForSeconds(Random.Range(2, 5));
                Debug.Log("p4");
            }
            else if (powerUps[5])
            {
                yield return new WaitForSeconds(Random.Range(20, 30));
                Debug.Log("p5");
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(3, 8));
                Debug.Log("p - others");
            }
        }
    }

    
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
