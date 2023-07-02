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
    private bool _stopSpawn = false;
    [SerializeField]
    private GameObject[] _commonPowerups;
    [SerializeField]
    private GameObject[] _rarePowerups;


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
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawn == false)
        {
            Vector3 postLoc = new Vector3(Random.Range(-9.5f, 9.5f), 9f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, postLoc, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(1.5f);

        }

    }


    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawn == false)
        {
            Vector3 powerLoc = new Vector3(Random.Range(-9.5f, 9.5f), 9.5f, 0);
            int randomNumber = Random.Range(0, 9);
 
            if (randomNumber >= 0 && randomNumber <= 5)
            {
                int randomCommon = Random.Range(0, 4);
                GameObject _commonPowerup = Instantiate(_commonPowerups[randomCommon], powerLoc, Quaternion.identity);
            }
            else if (randomNumber >= 6 && randomNumber <= 9)
            {
                int randomRare = Random.Range(0, 2);
                GameObject _rarePowerup = Instantiate(_rarePowerups[randomRare], powerLoc, Quaternion.identity);
            }
            else
            {
                Debug.LogError("No Valid Spawn");
            }
            
           
            yield return new WaitForSeconds(Random.Range(3, 7));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawn = true;
    }
}
