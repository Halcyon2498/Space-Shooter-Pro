using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamEnemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 3.0f;
    [SerializeField]
    public float _enemyChaseSpeed = 5.0f;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject _thrusters;
    [SerializeField]
    private AudioClip _alertSound;
    [SerializeField]
    private GameObject _hitbox;
    [SerializeField]
    private GameObject _detector;
    private bool _isChasing = false;
    [SerializeField]
    private float _rotationModifier = 90f;
    [SerializeField]
    private float _turnSpeed = 20f;
    


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _explosionPrefab.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        transform.rotation = Quaternion.Euler(0, 0, 180);
        _thrusters.SetActive(false);

        if (_player == null)
        {
            Debug.Log("Player is null");
        }

        if(_spawnManager == null)
        {
            Debug.Log("Spawn Manager is null");
        }

    }


    void Update()
    {
        if (_isChasing == false || _player == null)
        {
            transform.Translate(Vector3.up * _enemySpeed * Time.deltaTime);
        }
        else if (_isChasing == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _enemyChaseSpeed * Time.deltaTime);
        }

        if (transform.position.y < -8.5f)
        {
            Destroy(this.gameObject);
            _spawnManager._enemiesLeft--;
        }

    }

    private void FixedUpdate()
    {
        if(_isChasing == true)
        {
            Vector3 vectorToTarget = _player.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - _rotationModifier;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _turnSpeed);
        }
    }

    public void PlayerHit()
    {
        _player.Damage();
        EnemyDestroySequence();
    }

    public void LaserHit()
    {       
        _player.addScore(25);
        EnemyDestroySequence();
    }

    public void MegaHit()
    {
        _player.addScore(25);
        EnemyDestroySequence();
    }

    public void ChasePlayer()
    {
        _audioSource.clip = _alertSound;
        _audioSource.PlayOneShot(_alertSound, 1);
        _thrusters.SetActive(true);
        _isChasing = true;
    }

    private void EnemyDestroySequence()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _spawnManager._enemiesLeft--;
        _enemySpeed = 0f;
        _enemyChaseSpeed = 0f;
        Destroy(this.gameObject);
    }
}
