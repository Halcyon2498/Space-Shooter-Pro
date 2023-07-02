using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
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
    [SerializeField]
    private int _ammoCount = 15;
    private int _shieldHits = 3;
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private GameObject _megalaserPrefab;
    [SerializeField]
    private Slider _thrusterGauge;
    [SerializeField]
    private float _maxEnergy = 100f;
    private bool _thrustersActive = false;
    private CameraShake _damageShake;




    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _leftEngine.SetActive(false);
        _rightEngine.SetActive(false);
        _megalaserPrefab.SetActive(false);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _laserSound;
        _spriteRenderer = transform.Find("ShieldVisualizer").GetComponent<SpriteRenderer>();
        _thrusterGauge = GameObject.Find("Canvas").GetComponentInChildren<Slider>();
        _damageShake = GameObject.Find("Camera_Container").GetComponent<CameraShake>();

        if (_spriteRenderer == null)
        {
            Debug.LogError("The Sprite Render is NULL");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
        
        if (_thrusterGauge == null)
        {
            Debug.LogError("The Slider is NULL");
        }
        if (_damageShake == null)
        {
            Debug.LogError("The Camera is NULL");
        }
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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isThrusting();
            StartCoroutine(ThrustRoutine()); 
            if (_maxEnergy <= 0)
            {
                _speed = 4f;
            }

        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            stopThrusting();
            StartCoroutine(ThrustRegenRoutine());
            
        }

        _thrusterGauge.value = _maxEnergy;

    }


    IEnumerator ThrustRoutine()
    {
        while (_thrustersActive == true && _maxEnergy >= 0)
        {
            _maxEnergy -= 0.1f;          
            yield return new WaitForSeconds(2.0f);
        }
    }

    IEnumerator ThrustRegenRoutine()
    {
        yield return new WaitForSeconds(1.2f);
        while (_thrustersActive == false && _maxEnergy <= 100)
        {
            _maxEnergy += 2.5f;
            yield return new WaitForSeconds(0.1f);
        }

    }    

    private void isThrusting()
    {
        _thrustersActive = true;
        _speed = 7f;
    }

    private void stopThrusting()
    {
        _thrustersActive = false;
        _speed = 4f;
    }


    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_ammoCount > 0)
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.95f, 0), Quaternion.identity);
            _ammoCount--;
            _uiManager.UpdateAmmo(_ammoCount);
            _audioSource.PlayOneShot(_laserSound, 1);
        }

        if (_triple == true)
        {
            Instantiate(_triplePrefab, transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_laserSound, 1);
        }

       
    }


public void Damage()
    {

        if (_shield == true)
        {
            _shieldHits--;
            
            if (_shieldHits == 2)
            {
           
                _spriteRenderer.color = new Color(255f, 0f, 255f, 255f);
                
            }
            if (_shieldHits == 1)
            {
              
                _spriteRenderer.color = new Color(255f, 0f, 0f, 255f);
              
            }
            if (_shieldHits < 1)
            {
                _shieldVisualizer.SetActive(false);
                _shield = false;
            }
        }
        else
        {
            _lives--;
            _damageShake.ShakeCamera();
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
        _shieldHits = 3;
        _spriteRenderer.color = new Color(200f, 200f, 200f, 255f);
        _audioSource.clip = _powerupSound;
        _audioSource.PlayOneShot(_powerupSound, 1);
        _shieldVisualizer.SetActive(true);
    }

    public void AmmoActive()
    {
        _ammoCount = 15;
        _uiManager.UpdateAmmo(_ammoCount);
        _audioSource.clip = _powerupSound;
        _audioSource.PlayOneShot(_powerupSound, 1);
    }

    public void LivesActive()
    {
        if (_lives < 3)
        {
            _lives += 1;
            if (_lives == 2)
            {
                _rightEngine.SetActive(false);
            }
            else if (_lives == 3)
            {
                _leftEngine.SetActive(false);
            }
            _uiManager.UpdateLives(_lives);
        }
        _audioSource.clip = _powerupSound;
        _audioSource.PlayOneShot(_powerupSound, 1);
    }

    public void MegaLaserActive()
    {
        StartCoroutine(MegaCooldown());
        _audioSource.clip = _powerupSound;
        _audioSource.PlayOneShot(_powerupSound, 1);
        return;
    }

    IEnumerator MegaCooldown()
    {
        _megalaserPrefab.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        _megalaserPrefab.SetActive(false);
    }

    public void addScore(int addPoints)
    {
        _score += addPoints;
        if (_uiManager != null)
        {
            _uiManager.UpdateScore(_score);
        }
    }

}
