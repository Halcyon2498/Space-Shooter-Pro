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

        if (_enemyContainer.transform.childCount == 0 && _startWave == true)
        {
            _enemiesLeft = 0;
            StopCoroutine(SpawnEnemyRoutine());
            StopCoroutine(SpawnPowerUpRoutine());
            EndWave();
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
        new WaitForSeconds(3.0f);
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
                Vector3 postLoc = new Vector3(11.25f, Random.Range(2.0f, 6.0f), 0);
                GameObject newEnemy = Instantiate(_enemyPrefab, postLoc, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                enemiesSpawned++;
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
