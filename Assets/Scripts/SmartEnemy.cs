using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    [SerializeField]
    Transform[] waypoints;
    [SerializeField]
    private float _enemySpeed = 3.0f;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private Collider2D _boxCollider2D;
    [SerializeField]
    private GameObject _smartLaserPrefab;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private AudioClip _smartSound;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    private SpawnManager _spawnManager;
    public Transform _castPoint;
    [SerializeField]
    private float rotationModifier = 90f;
    [SerializeField]
    private float turnSpeed = 20f;
    private bool _detectPlayer = false;
    private float _rayDistance = 15f;
    int waypointIndex = 0;

    void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>();

        _audioSource.clip = _smartSound;

        if (_player == null)
        {
            Debug.Log("Player is null");
        }

        if (_boxCollider2D == null)
        {
            Debug.Log("Box Collider is null");
        }

        if (_spawnManager == null)
        {
            Debug.Log("Spawn Manager is null");
        }

    }


    void Update()
    {
        Move();
        PlayerLOS();
        ShootPlayer();
    }

    private void FixedUpdate()
    {
        Vector3 vectorToTarget = _player.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * turnSpeed);
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, _enemySpeed * Time.deltaTime);

        if (transform.position == waypoints[waypointIndex].transform.position)
        {
            waypointIndex += 1;
        }

        if (waypointIndex == waypoints.Length)
        {
            Destroy(this.gameObject);
            _spawnManager._enemiesLeft--;
        }
    }

    void ShootPlayer()
    {
        if (_detectPlayer == true)
        {
            if (Time.time > _canFire && _enemySpeed != 0)
            {
                _fireRate = Random.Range(4f, 8f);
                _canFire = Time.time + _fireRate;
                _audioSource.PlayOneShot(_smartSound, 1);
                Instantiate(_smartLaserPrefab, transform.position + new Vector3(0, -1f, 0), Quaternion.identity);
            }
        }
    }
    


    void PlayerLOS()
    {
        Debug.DrawRay(_castPoint.transform.position, (transform.up * _rayDistance), Color.red);

        RaycastHit2D hit = Physics2D.Raycast(_castPoint.transform.position, transform.up, 18);

        if (hit)
        {
            if (hit.collider.tag == "Player" && _detectPlayer == false)
            {
                _detectPlayer = true;
                ShootPlayer();

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.transform.GetComponent<Player>();

        if (other.tag == "Player")
        {

            if (player != null)
            {
                player.Damage();
            }
            EnemyDestroySequence();

        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _player.addScore(50);
            EnemyDestroySequence();

        }

        if(other.tag == "Missle")
        {
            Destroy(other.gameObject);
            _player.addScore(50);
            EnemyDestroySequence();
        }

        if (other.tag == "MegaLaser")
        {
            _player.addScore(50);
            EnemyDestroySequence();
        }
    }

    private void EnemyDestroySequence()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _spawnManager._enemiesLeft--;
        _enemySpeed = 0f;
        Destroy(this.gameObject);
    }


}
