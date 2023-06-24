using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _triplePrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private float _fireRate = 0.3f;
    [SerializeField]
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private bool _triple = false;
    [SerializeField]
    private bool _speedBoost = false;
    [SerializeField]
    private bool _shield = false;
    [SerializeField]
    private SpawnManager _spawnManager;   
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private int _score;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioClip _powerupSound;
    [SerializeField]
    private AudioClip _explosionSound;




    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _leftEngine.SetActive(false);
        _rightEngine.SetActive(false);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _laserSound;
        

    }
    

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }


    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // alt way transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
        // Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        if (_speedBoost == true)
        {
            transform.Translate(Vector3.right * horizontalInput * _speed * _speedMultiplier * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speed * _speedMultiplier * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
            transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);
        }


        // alt transform.position - new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 5.9f, -3.9f), 0)

        if (transform.position.y >= 5.9f)
        {
            transform.position = new Vector3(transform.position.x, 5.9f, 0);
        }
        else if (transform.position.y <= -3.9f)
        {
            transform.position = new Vector3(transform.position.x, -3.9f, 0);
        }

        if (transform.position.x >= 11.25f)
        {
            transform.position = new Vector3(-11.25f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.25f)
        {
            transform.position = new Vector3(11.25f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_triple == true)
        {

            Instantiate(_triplePrefab, transform.position, Quaternion.identity);
        }
        else
        {

            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.95f, 0), Quaternion.identity);
        }

        _audioSource.PlayOneShot(_laserSound, 1);
    }

    public void Damage()
    {
 
        if (_shield == true)
        {
            _shield = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        else
        {
            _lives--;
            _uiManager.UpdateLives(_lives);
        }
        
        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }

        if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.RestartLevelText();
            _uiManager.GameOverText();
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
            _audioSource.clip = _explosionSound;
            _audioSource.PlayOneShot(_explosionSound, 1);
            Damage();
        }
    }


    public void TripleShotActive()
    {
     
        _triple = true;
        _audioSource.clip = _powerupSound;
        _audioSource.PlayOneShot(_powerupSound, 1);
        StartCoroutine(TripleShotPowerDownRoutine());

    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _triple = false;
    }

    public void SpeedActive()
    {
        _speedBoost = true;
        _audioSource.clip = _powerupSound;
        _audioSource.PlayOneShot(_powerupSound, 1);
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speedBoost = false;
    }

    public void ShieldActive()
    {
        _shield = true;
        _audioSource.clip = _powerupSound;
        _audioSource.PlayOneShot(_powerupSound, 1);
        _shieldVisualizer.SetActive(true);
    }

    public void addScore(int addPoints)
    {
        _score += addPoints;
        if(_uiManager != null)
        {
            _uiManager.UpdateScore(_score);
        }
    }

}
