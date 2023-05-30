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
        }

    }

    public void OnPlayerDeath()
    {
        _stopSpawn = true;
    }
}
