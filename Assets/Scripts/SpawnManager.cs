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
    private GameObject _bossPrefab;
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
    public bool _startBossWave = false;
 


    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _currentWave = 4;
        _enemiesToSpawn = 0;
        if (_uiManager == null)
        {
            Debug.Log("UI is NULL");
        }
    }

    private void Update()
    {

        if (_startWave == true && _enemyContainer.transform.childCount == 0)
        {
            StopCoroutine(SpawnPowerUpRoutine());
            _enemiesLeft = 0;
            EndWave();
        }

    }

    private void StartBossWave()
    {
        _stopSpawn = false;
        _uiManager.UpdateBoss();
        Instantiate(_bossPrefab);
        StartCoroutine(SpawnPowerUpRoutine());
    }

    private void StartSpawning()
    {
        _enemiesLeft = _enemiesToSpawn;
        _startWave = false;
        _stopSpawn = false;
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine()); 
        
    }

    private void StartWave()
    {
        _uiManager.UpdateWaves(_currentWave); 
        _enemiesLeft = _enemiesToSpawn;             
        StartSpawning();

    }

    public void EndWave()
    {     
        _currentWave += 1;
        if (_currentWave < 5)
        {
            _enemiesToSpawn += 10;
            StartWave();
        }
        else if (_currentWave == 5)
        {
            StartBossWave();
        }
    }



    IEnumerator SpawnEnemyRoutine()
    {
        int enemiesSpawned = 0;     

        yield return new WaitForSeconds(4.0f);

        while (_stopSpawn == false && _currentWave != 5)
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
                else if (randomNumber >= 8 && randomNumber <= 16)
                {
                    GameObject newEnemy2 = Instantiate(_enemyLeftPrefab, postLoc2, Quaternion.identity);
                    newEnemy2.transform.parent = _enemyContainer.transform;
                }
                else if (randomNumber >= 17 && randomNumber <= 18)
                {
                    GameObject newEnemy3 = Instantiate(_RamEnemyPrefab, postLoc3, Quaternion.identity);
                    newEnemy3.transform.parent = _enemyContainer.transform;
                }
                else if (randomNumber == 19)
                {
                    GameObject newEnemy4 = Instantiate(_smartEnemyPrefab);
                    newEnemy4.transform.parent = _enemyContainer.transform;
                }
                else if (randomNumber == 20)
                {
                    GameObject newEnemy5 = Instantiate(_speedEnemyPrefab);
                    newEnemy5.transform.parent = _enemyContainer.transform;
                }
                else if (_currentWave >= 2 && randomNumber == 21)
                {
                    GameObject newEnemy6 = Instantiate(_enemy2Prefab);
                    newEnemy6.transform.parent = _enemyContainer.transform;
                }
                else
                {
                    Debug.Log("Invalid ENemy");
                }

                enemiesSpawned++;

            }
            else if (enemiesSpawned == _enemiesToSpawn)
            {
                _stopSpawn = true;
                enemiesSpawned = 0;
                _startWave = true;
            }

            yield return new WaitForSeconds(1.0f);
        }
       
    }


    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawn == false)
        {
            if (_currentWave != 5)
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
            else if (_currentWave == 5)
            {
                Vector3 powerLoc = new Vector3(Random.Range(-12.1f, 12.1f), 13.4f, 0);

                int randomCommon = Random.Range(0, 5);
                GameObject _commonPowerup = Instantiate(_commonPowerups[randomCommon], powerLoc, Quaternion.identity);

                yield return new WaitForSeconds(Random.Range(2, 5));
            }
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawn = true;
    }

    public void OnBossDeath()
    {
        _stopSpawn = true;
    }
}
