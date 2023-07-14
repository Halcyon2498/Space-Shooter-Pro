using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemy2Prefab;
    [SerializeField]
    private GameObject _enemyLeftPrefab;
    [SerializeField]
    private GameObject _RamEnemyPrefab;
    [SerializeField]
    private GameObject _smartEnemyPrefab;
    [SerializeField]
    private GameObject _speedEnemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private bool _stopSpawn = false;
    [SerializeField]
    private GameObject[] _commonPowerups;
    [SerializeField]
    private GameObject[] _rarePowerups;
    private UIManager _uiManager;
    public int _currentWave = 0;
    public int _enemiesToSpawn = 0;
    public int _enemiesLeft = 0;
    public bool _startWave = false;
 


    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _currentWave = 0;
        _enemiesToSpawn = 0;
        if (_uiManager == null)
        {
            Debug.Log("UI is NULL");
        }
    }

    private void Update()
    {

        if (_startWave == true)
        {
            StopCoroutine(SpawnEnemyRoutine());
            StopCoroutine(SpawnPowerUpRoutine());
            if (_enemyContainer.transform.childCount == 0)
            {
                _enemiesLeft = 0;
                EndWave();
            }
        }
    }

    private void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
        _startWave = false;
        _enemiesLeft = _enemiesToSpawn;
    }

    private void StartWave()
    {
        _uiManager.UpdateWaves(_currentWave); 
        new WaitForSeconds(3.0f);
        _stopSpawn = false;
        _enemiesLeft = _enemiesToSpawn;             
        StartSpawning();

    }

    public void EndWave()
    {       
        _currentWave += 1;
        _enemiesToSpawn += 20;
        new WaitForSeconds(4.0f);
        StartWave();
    }



    IEnumerator SpawnEnemyRoutine()
    {
        int enemiesSpawned = 0;     

        yield return new WaitForSeconds(3.0f);

        while (_stopSpawn == false)
        {
            if (enemiesSpawned != _enemiesToSpawn)
            {
                Vector3 postLoc = new Vector3(14.5f, Random.Range(2.0f, 8.0f), 0);
                Vector3 postLoc2 = new Vector3(-14.5f, Random.Range(2.0f, 8.0f), 0);
                Vector3 postLoc3 = new Vector3(Random.Range(-10f, 10f), 9f, 0);
                int randomNumber = Random.Range(0, 21);
                if (randomNumber >= 0 && randomNumber <= 7)
                {
                    GameObject newEnemy = Instantiate(_enemyPrefab, postLoc, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                }
                else if (randomNumber >= 9 && randomNumber <= 15)
                {
                    GameObject newEnemy2 = Instantiate(_enemyLeftPrefab, postLoc2, Quaternion.identity);
                    newEnemy2.transform.parent = _enemyContainer.transform;
                }
                else if(randomNumber >= 16 && randomNumber <= 17)
                {
                    Instantiate(_RamEnemyPrefab, postLoc3, Quaternion.identity);
                }
                else if(randomNumber == 18)
                {
                    Instantiate(_smartEnemyPrefab);
                }
                else if(randomNumber == 19)
                {
                    Instantiate(_speedEnemyPrefab);
                }
                else if (_currentWave >= 2 && randomNumber == 20)
                {
                    Instantiate(_enemy2Prefab);
                }
                
                
                enemiesSpawned ++;
            }
            else if(enemiesSpawned == _enemiesToSpawn)
            {
                _stopSpawn = true;
                _startWave = true;
                enemiesSpawned = 0;
            }

            

            yield return new WaitForSeconds(1.0f);
        }
       
    }


    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawn == false)
        {
            Vector3 powerLoc = new Vector3(Random.Range(-12.1f, 12.1f), 13.4f, 0);
            int randomNumber = Random.Range(0, 9);

            if (randomNumber >= 0 && randomNumber <= 6)
            {
                int randomCommon = Random.Range(0, 5);
                GameObject _commonPowerup = Instantiate(_commonPowerups[randomCommon], powerLoc, Quaternion.identity);
            }
            else if (randomNumber >= 7 && randomNumber <= 9)
            {
                int randomRare = Random.Range(0, 3);
                GameObject _rarePowerup = Instantiate(_rarePowerups[randomRare], powerLoc, Quaternion.identity);
            }
            else
            {
                Debug.LogError("No Valid Spawn");
            }


            yield return new WaitForSeconds(Random.Range(2, 5));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawn = true;
    }
}
