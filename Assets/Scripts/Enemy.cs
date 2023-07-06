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
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explodeSound;
    [SerializeField]
    private GameObject _enemyLaser;
    [SerializeField]
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    private float sinCenterY;
    [SerializeField]
    private float _amplitude = -2;
    [SerializeField]
    private float _frequency = 2;
    private SpawnManager _spawnManager;
 



    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _explodeSound;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _animator = transform.GetComponent<Animator>();
        sinCenterY = transform.position.y;

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

        _enemySpeed = 3.0f;

    }

    // Update is called once per frame
    void Update()
    {     

        if (Time.time > _canFire && _enemySpeed != 0)
        {
            _fireRate = Random.Range(4f, 8f);
            _canFire = Time.time + _fireRate;
            Instantiate(_enemyLaser, transform.position + new Vector3(0, -1f, 0), Quaternion.identity);
        }

    }


    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
       
        float sin = Mathf.Sin(pos.x *_frequency) * _amplitude;
        pos.x -= _enemySpeed * Time.fixedDeltaTime;
        pos.y = sinCenterY + sin;

        transform.position = pos;

        if (transform.position.x < -12)
        {
            Destroy(this.gameObject);
            _spawnManager._enemiesLeft--;
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
            _player.addScore(10);
            
            EnemyDestroySequence();

        }

        if (other.tag == "MegaLaser")
        {
            _player.addScore(10);
           
            EnemyDestroySequence();
        }

    }

    private void EnemyDestroySequence()
    {
        Destroy(this._boxCollider2D);
        _spawnManager._enemiesLeft--;
        _audioSource.PlayOneShot(_explodeSound, 1);
        _enemySpeed = 0f;
        _animator.SetTrigger("OnEnemyDown");
        Destroy(this.gameObject, 2.5f);
    }



}
