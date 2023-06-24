using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _turnDegrees = 20f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private GameObject _boom;
    private SpawnManager _spawnManager;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explodeSound;


    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("Audio source doesnt exist");
        }
        else
        {
            _audioSource.clip = _explodeSound;
        }


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
            _boom = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(_boom, 2.7f);
            _audioSource.PlayOneShot(_explodeSound, 1);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.4f);
        }
    }
}
