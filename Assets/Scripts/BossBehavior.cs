using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{

    [SerializeField]
    private float _enemySpeed = 0f;
    [SerializeField]
    private GameObject _megaLaser;
    [SerializeField]
    private GameObject _megaLaser2;
    [SerializeField]
    private GameObject _charging;
    [SerializeField]
    private GameObject _charging2;
    private float _canFire = -1;
    [SerializeField]
    private float _fireRate = 10.0f;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _megaSound;
    [SerializeField]
    private AudioClip _chargeSound;
    [SerializeField]
    private AudioClip _rapidSound;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private GameObject _dmg1;
    [SerializeField]
    private GameObject _dmg2;
    [SerializeField]
    private GameObject _dmg3;
    [SerializeField]
    private GameObject _dmg4;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _burstPrefab;
    private float _burstCooldown = 4.0f;
    private float _burstTimer = 3f;
    private int _laserCount = 3;
    private int _hitPoints = 50;

    void Start()
    {
        transform.position = new Vector3(-0.68f, 14.4f, 0);
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_player == null)
        {
            Debug.Log("Player is null");
        }
        if (_spawnManager == null)
        {
            Debug.Log("Spawn Manager is null");
        }

        StartCoroutine(FiringBurst());
    }

    void Update()
    {
        Move();
        FireSpecialLaser();
    }

    void Move()
    {
        float y = 7f;
        if (transform.position.y >= y)
        {
            _enemySpeed = 2.0f;
            transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        }

        if (transform.position.y <= y)
        {
            _enemySpeed = 0f;
        }

    }

    void FireSpecialLaser()
    {
        if (Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            StartCoroutine(MegaLaser());
            return;
        }
    }

    IEnumerator FiringBurst()
    {
        yield return new WaitForSeconds(_burstCooldown);
        float waitForNext = 1f / _laserCount;

        while (_hitPoints > 0)
        {
            for (int i = 0; i < _laserCount; i++)
            {
                Instantiate(_burstPrefab, transform.position, Quaternion.identity);
                _audioSource.PlayOneShot(_rapidSound, 1);
                yield return new WaitForSeconds(waitForNext);
            }

            yield return new WaitForSeconds(_burstTimer);
        }
    }


    IEnumerator MegaLaser()
    {
        yield return new WaitForSeconds(10.0f);
        _audioSource.clip = _chargeSound;
        _audioSource.PlayOneShot(_chargeSound, 1);
        _charging.gameObject.SetActive(true);
        _charging2.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        _charging.gameObject.SetActive(false);
        _charging2.gameObject.SetActive(false);
        _audioSource.clip = _megaSound;
        _audioSource.PlayOneShot(_megaSound, 1);
        _megaLaser.gameObject.SetActive(true);
        _megaLaser2.gameObject.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        _megaLaser.gameObject.SetActive(false);
        _megaLaser2.gameObject.SetActive(false);
        yield return new WaitForSeconds(15.0f);
    }

    void Damage()
    {

        _hitPoints--;

        if (_hitPoints == 40)
        {
            _dmg1.SetActive(true);
        }

        if (_hitPoints == 30)
        {
            _dmg2.SetActive(true);
        }

        if (_hitPoints == 20)
        {
            _dmg3.SetActive(true);
        }

        if (_hitPoints == 10)
        {
            _dmg4.SetActive(true);
        }

        if (_hitPoints < 1)
        {
            EnemyDestroySequence();
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
            Damage();

        }

    }

    private void EnemyDestroySequence()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _uiManager.Victory();
        Destroy(this.gameObject);
    }
}
