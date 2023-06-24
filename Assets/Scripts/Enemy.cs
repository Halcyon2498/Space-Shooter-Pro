using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 2.0f;
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



    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _explodeSound;
        _player = GameObject.Find("Player").GetComponent<Player>();


        if(_player == null)
        {
            Debug.Log("Player is null");
        }

        _animator = GetComponent<Animator>();
        if(_animator == null)
        {
            Debug.Log("Animator is null");
        }

        _boxCollider2D = GetComponent<Collider2D>();
        if(_boxCollider2D == null)
        {
            Debug.Log("Box Collider is null");
        }

        _enemySpeed = 2.0f;

    }

    // Update is called once per frame
    void Update()
    {
 
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            Instantiate(_enemyLaser, transform.position + new Vector3(0, -1f, 0), Quaternion.identity);
        }


    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y <= -7.1f)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomX, 9.5f, 0);
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
            if (_player != null)
            {
                _player.addScore(10);
            }            
            EnemyDestroySequence();
           

        }

    }

    private void EnemyDestroySequence()
    {
        Destroy(this._boxCollider2D);
        _audioSource.PlayOneShot(_explodeSound, 1);
        _enemySpeed = 0f; 
        _animator.SetTrigger("OnEnemyDown");      
        Destroy(this.gameObject, 2.5f);
    }
 
}
