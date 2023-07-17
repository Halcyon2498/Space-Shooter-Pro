using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _turnDegrees = 20f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;


    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

    }

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, _turnDegrees) * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _spawnManager.BeginWave();
            Destroy(this.gameObject);
        }

    }
}
