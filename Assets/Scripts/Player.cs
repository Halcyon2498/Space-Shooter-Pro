using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6f;
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
    private AudioClip _megaSound;
    [SerializeField]
    private AudioClip _explosionSound;
    [SerializeField]
    private AudioClip _freezeSound;
    [SerializeField]
    private int _ammoCount = 30;
    private int _shieldHits = 3;
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private GameObject _megalaserPrefab;
    [SerializeField]
    private Slider _thrusterGauge;
    [SerializeField]
    private float _maxEnergy = 100f;
    private bool _thrustersActive = false;
    [SerializeField]
    private GameObject _thrusters;
    private Animator _animator;
    private CameraShake _damageShake;
    [SerializeField]
    private GameObject _freeze;
    [SerializeField]
    private GameObject _misslePrefab;
    [SerializeField]
    private AudioClip _missleSound;
    [SerializeField]
    private AudioClip _reloadSound;
    private bool _isFrozen = false;
    [SerializeField]
    private int _barrageCount = 5;


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _thrusters.transform.localPosition = new Vector3(0, -2.4f, 0);
        _thrusters.transform.localScale = new Vector3(0.45f, 0.45f, 1);
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
        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.Log("Animator is null");
        }

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


    void Update()
    {
        CalculateMovement();
        CalculateAnim();
        FireWeapons();
    }

    void CalculateAnim()
    {
        float inputX = Input.GetAxisRaw("Horizontal");

        if (inputX < -0.2f)
        {
            _animator.SetBool("AisDown", true);
            _animator.SetBool("DisDown", false);
            _animator.SetBool("DefaultState", false);
        }
        else if (inputX > 0.2f)
        {
            _animator.SetBool("AisDown", false);
            _animator.SetBool("DisDown", true);
            _animator.SetBool("DefaultState", false);
        }
        else if (inputX > -0.2f && inputX < 0.2f)
        {
            _animator.SetBool("AisDown", false);
            _animator.SetBool("DisDown", false);
            _animator.SetBool("DefaultState", true);
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


        if (transform.position.y >= 7.67f)
        {
            transform.position = new Vector3(transform.position.x, 7.67f, 0);
        }
        else if (transform.position.y <= -5.47f)
        {
            transform.position = new Vector3(transform.position.x, -5.47f, 0);
        }

        if (transform.position.x >= 14.2f)
        {
            transform.position = new Vector3(-14.2f, transform.position.y, 0);
        }
        else if (transform.position.x <= -14.2f)
        {
            transform.position = new Vector3(14.2f, transform.position.y, 0);
        }

        if (_isFrozen == false)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isThrusting();
                _thrusters.transform.localPosition = new Vector3(0, -3.4f, 0);
                _thrusters.transform.localScale = new Vector3(1, 1, 1);
                StartCoroutine(ThrustRoutine());
                if (_maxEnergy <= 0)
                {
                    _thrusters.transform.localPosition = new Vector3(0, -2.4f, 0);
                    _thrusters.transform.localScale = new Vector3(0.45f, 0.45f, 1);
                    _speed = 6f;
                }

            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                stopThrusting();
                _thrusters.transform.localPosition = new Vector3(0, -2.4f, 0);
                _thrusters.transform.localScale = new Vector3(0.45f, 0.45f, 1);
                StartCoroutine(ThrustRegenRoutine());

            }
        }
        _thrusterGauge.value = _maxEnergy;

    }

    void FireWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        if (Input.GetKeyDown(KeyCode.X) && _barrageCount > 0)
        {
            _barrageCount--;
            _uiManager.UpdateMissiles(_barrageCount);
            StartCoroutine(MissleBarrage());
        }
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
        _speed = 9f;
    }

    private void stopThrusting()
    {
        _thrustersActive = false;
        _speed = 6f;
    }


    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isFrozen == false)
        {

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
    }

    IEnumerator MissleBarrage()
    {
        int _misslesFired = 6;

        while (_misslesFired > 0)
        {
            _audioSource.PlayOneShot(_missleSound, 1f);
            Instantiate(_misslePrefab, transform.position + new Vector3(1, 1f, 0), Quaternion.identity);
            Instantiate(_misslePrefab, transform.position + new Vector3(-1, 1f, 0), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            _misslesFired--;
        }
        if (_misslesFired == 0)
        {
            StopBarrage();
        }
    }
    private void StopBarrage()
    {
        StopCoroutine(MissleBarrage());
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

        if(other.tag == "BossBullet1")
        {
            other.gameObject.SetActive(false);
            _audioSource.clip = _explosionSound;
            _audioSource.PlayOneShot(_explosionSound, 1);
            Damage();
        }

        if(other.tag == "EnemyMega")
        {
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
        _ammoCount = 30;
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
        _megalaserPrefab.SetActive(true);
        StartCoroutine(MegaCooldown());
        _audioSource.clip = _powerupSound;
        _audioSource.PlayOneShot(_powerupSound, 1);
        return;
    }

    IEnumerator MegaCooldown()
    { 
        _audioSource.PlayOneShot(_megaSound, 1);
        yield return new WaitForSeconds(5.0f);
        _megalaserPrefab.SetActive(false);
    }

    public void FreezeActive()
    {
        _isFrozen = true;
        _speed = 0;
        _audioSource.clip = _freezeSound;
        _audioSource.PlayOneShot(_freezeSound, 1);
        _freeze.gameObject.SetActive(true);
        StartCoroutine(FreezeCooldown());
    }

    IEnumerator FreezeCooldown()
    {
        yield return new WaitForSeconds(4.0f);
        _speed = 6;
        _isFrozen = false;
        _freeze.gameObject.SetActive(false);
    }

    public void RefillBarrage()
    {
        _barrageCount = 5;
        _uiManager.RefillMissles();
        _audioSource.clip = _reloadSound;
        _audioSource.PlayOneShot(_reloadSound, 1);
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
