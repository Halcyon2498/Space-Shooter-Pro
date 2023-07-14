using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [SerializeField]
    Transform[] waypoints;
    [SerializeField]
    float movespeed = 2f;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Collider2D _boxCollider2D;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explodeSound;
    [SerializeField]
    private GameObject _rapidFire;
    [SerializeField]
    private AudioClip _rapidSound;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private GameObject _rightEngine;
    private SpawnManager _spawnManager;
    private float _burstCooldown = 4.0f;
    private float _burstTimer = 1f;
    private int _laserCount = 10;
    int waypointIndex = 0;
    private int _lives = 6;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _explodeSound;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _animator = transform.GetComponent<Animator>();

        if (_player == null)
        {
            Debug.Log("Player is null");
        }

        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.Log("Animator is null");
        }

        _boxCollider2D = GetComponent<BoxCollider2D>();
        if (_boxCollider2D == null)
        {
            Debug.Log("Box Collider is null");
        }

        if (_spawnManager== null)
        {
            Debug.Log("Spawn Manager is null");
        }

        _leftEngine.SetActive(false);
        _rightEngine.SetActive(false);
        StartCoroutine(FiringBurst());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


   

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, movespeed * Time.deltaTime);

        if (transform.position == waypoints[waypointIndex].transform.position)
        {
            waypointIndex += 1;
        }

        if (waypointIndex == waypoints.Length)
        {
            Destroy(this.gameObject);
        }
    }

    void Damage()
    {

        _lives--;

        if (_lives == 4)
        {
            _leftEngine.SetActive(true);
        }

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }

        if (_lives < 1)
        {
            _leftEngine.SetActive(false);
            _rightEngine.SetActive(false);
            EnemyDestroySequence();
        }

    }

    IEnumerator FiringBurst()
    {
        yield return new WaitForSeconds(_burstCooldown);
        float waitForNext = 1f / _laserCount;

        while(movespeed > 0)
        {
            for(int i = 0; i < _laserCount; i++)
            {
                Instantiate(_rapidFire, transform.position, Quaternion.identity);
                _audioSource.PlayOneShot(_rapidSound, 1);
                yield return new WaitForSeconds(waitForNext);
            }

            yield return new WaitForSeconds(_burstTimer);
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
                Damage();
            }

        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _player.addScore(100);
            Damage();

        }

        if (other.tag == "MegaLaser")
        {
            _player.addScore(100);

            EnemyDestroySequence();
        }


    }

    private void EnemyDestroySequence()
    {
        Destroy(this._boxCollider2D);
        _spawnManager._enemiesLeft--;
        _audioSource.PlayOneShot(_explodeSound, 1);
        movespeed = 0f;
        _animator.SetTrigger("OnEnemy2Down");
        Destroy(this.gameObject, 2.5f);
    }


}
