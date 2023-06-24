using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD

=======
>>>>>>> 56686b94c8f9145068e5aca2326c6b35c1f53afe
public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private bool _stopSpawn = false;
<<<<<<< HEAD
    [SerializeField]
    private GameObject[] powerups;


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
=======
   
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnRoutine()
    {
        //while loop
        //instantiate enemyprefab
        //yield wait for 4 seconds
        while (_stopSpawn == false)
        {
            Vector3 postLoc = new Vector3(Random.Range(-9.5f, 9.5f), 9.5f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, postLoc, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(3.0f); 
>>>>>>> 56686b94c8f9145068e5aca2326c6b35c1f53afe
        }

    }

<<<<<<< HEAD
    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawn == false)
        {
            Vector3 powerLoc = new Vector3(Random.Range(-9.5f, 9.5f), 9.5f, 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(powerups[randomPowerUp], powerLoc, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(4, 8));
        }
    }
=======
>>>>>>> 56686b94c8f9145068e5aca2326c6b35c1f53afe
    public void OnPlayerDeath()
    {
        _stopSpawn = true;
    }
}
