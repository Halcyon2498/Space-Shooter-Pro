using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 3.0f;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Collider2D _boxCollider2D;
    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject _enemyLaser;
    [SerializeField]
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    private float _sinCenterY;
    [SerializeField]
    private float _amplitude = -2;
    [SerializeField]
    private float _frequency = 2;
    [SerializeField]
    private GameObject _shieldVisualizer;
    private int _shieldHits;
    private bool _shield = false;
    private SpawnManager _spawnManager;
    public Transform _castPoint;
    private bool _detectPower = false;
    private float _rayDistance = 12f;
    private bool _hasFired = false;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _animator = transform.GetComponent<Animator>();
        _sinCenterY = transform.position.y;
        _shieldVisualizer.SetActive(false);
        _enemySpeed = 3.0f;

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

        int ifShield = Random.Range(0, 9);

        if (ifShield <= 2)
        {
            _shieldVisualizer.SetActive(true);
            _shield = true;
            _shieldHits = 1;
        }
      
    }

    void Update()
    {
        FireEnemyLaser();
        PowerLOS();
    }

    void FireEnemyLaser()
    {
        if (Time.time > _canFire && _enemySpeed != 0)
        {
            _fireRate = Random.Range(4f, 8f);
            _canFire = Time.time + _fireRate;
            Instantiate(_enemyLaser, transform.position + new Vector3(0, -1f, 0), Quaternion.identity);
        }
    }

    void ShootPower()
    {

        if (_hasFired == false)
        {
            Instantiate(_enemyLaser, transform.position + new Vector3(0, -1f, 0), Quaternion.identity);
            _detectPower = false;
            _hasFired = true;
        }
    }

    void PowerLOS()
    {

        Debug.DrawRay(_castPoint.transform.position, (-transform.up * _rayDistance), Color.red);

        RaycastHit2D hit = Physics2D.Raycast(_castPoint.transform.position, -transform.up, 6);

        if (hit)
        {
            if (hit.collider.tag == "Powerup" && _detectPower == false)
            {
                _detectPower = true;
                ShootPower();
               
            }
        }
    }


    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
       
        float sin = Mathf.Sin(pos.x *_frequency) * _amplitude;
        pos.x -= _enemySpeed * Time.fixedDeltaTime;
        pos.y = _sinCenterY + sin;

        transform.position = pos;

        if (transform.position.x < -14.3f)
        {
            Destroy(this.gameObject);
            _spawnManager._enemiesLeft--;
        }
    }

    public void Damage()
    {
        if (_shield == true)
        {
            _shieldHits--;
            _shieldVisualizer.SetActive(false);
            _shield = false;
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

            if (_shield == true)
            {
                Damage();
            }
            else
            {
                EnemyDestroySequence();
            }

        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _player.addScore(10);
            if (_shield == true)
            {
                Damage();
            }
            else
            {
                EnemyDestroySequence();
            }

        }
        

        if (other.tag == "MegaLaser")
        {
            _player.addScore(10);
            _shieldVisualizer.SetActive(false);
            EnemyDestroySequence();         
        }

        

    }

    private void EnemyDestroySequence()
    {
        Destroy(this._boxCollider2D);
        _spawnManager._enemiesLeft--;
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _enemySpeed = 0f;
        Destroy(this.gameObject);
    }



}
