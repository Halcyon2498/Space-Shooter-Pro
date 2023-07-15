using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;




public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private GameObject[] _misslesRemaining;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _ammoCounter;
    [SerializeField]
    private Slider _thrusterGauge;
    [SerializeField]
    private Image _thrusterColor;
    [SerializeField]
    private Text _waveCounter;
    [SerializeField]
    private Text _bossWave;
    [SerializeField]
    private Text _victory;
    [SerializeField]
    private Text _finalScore;
    private int updateScore = 0;


    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _restartText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        _waveCounter.gameObject.SetActive(false);
        _bossWave.gameObject.SetActive(false);
        _victory.gameObject.SetActive(false);
        _finalScore.gameObject.SetActive(false);
        _ammoCounter.text = "Ammo: " + 30 + "/30";
        _thrusterGauge = GetComponentInChildren<Slider>();
        _thrusterColor = _thrusterGauge.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();

        if (_thrusterGauge == null)
        {
            Debug.Log("Gauge is NULL");
        }
        if (_thrusterColor== null)
        {
            Debug.Log("Color is NULL");
        }

    }


    private void GaugeColor()
    {
        
        if (_thrusterGauge.value <= 102f && _thrusterGauge.value >= 75f)
        {
            _thrusterColor.color = Color.green;
        }
        else if (_thrusterGauge.value <= 75f && _thrusterGauge.value >= 35f)
        {
            _thrusterColor.color = Color.yellow;
        }
        else if (_thrusterGauge.value <= 35f && _thrusterGauge.value >= 0f)
        {
            _thrusterColor.color = Color.red;
        }
    }

    public void UpdateWaves(int currentWave)
    {
        _waveCounter.gameObject.SetActive(true);
        _waveCounter.text = "Wave: " + currentWave.ToString();
        StartCoroutine(WaveDown());
    }

    public void UpdateScore(int _playerScore)
    {
        _scoreText.text = "Score: " + _playerScore.ToString();
        updateScore = _playerScore;
    }

    public void UpdateLives(int currentLives)
    {
 
        _livesImage.sprite = _livesSprites[currentLives];        

    }

    public void UpdateBoss()
    {
        _bossWave.gameObject.SetActive(true);
        StartCoroutine(BossFlicker());
    }

    public void UpdateMissiles(int mCount)
    {
        _misslesRemaining[mCount].gameObject.SetActive(false);
    }

    public void Victory()
    {
        _victory.gameObject.SetActive(false);
        _finalScore.gameObject.SetActive(true);
        _finalScore.text = "Final Score: " + updateScore.ToString();
        _restartText.gameObject.SetActive(true);
    }

    public void RefillMissles()
    {
        _misslesRemaining[0].gameObject.SetActive(true);
        _misslesRemaining[1].gameObject.SetActive(true);
        _misslesRemaining[2].gameObject.SetActive(true);
        _misslesRemaining[3].gameObject.SetActive(true);
        _misslesRemaining[4].gameObject.SetActive(true);
    }

    public void UpdateAmmo(int _ammo)
    {
        _ammoCounter.text = "Ammo: " + _ammo.ToString() + "/30";
    }

    private IEnumerator WaveDown()
    {

        yield return new WaitForSeconds(6.0f);
        _waveCounter.gameObject.SetActive(false);

    }

    private IEnumerator BossFlicker()
    {
            transform.GetChild(10).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            transform.GetChild(10).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            transform.GetChild(10).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            transform.GetChild(10).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            transform.GetChild(10).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            transform.GetChild(10).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            transform.GetChild(10).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            transform.GetChild(10).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            transform.GetChild(10).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            transform.GetChild(10).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            transform.GetChild(10).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            transform.GetChild(10).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            transform.GetChild(10).gameObject.SetActive(false);
    }
    private IEnumerator GameOverFlicker()
    {
        while(true)
        {
            transform.GetChild(2).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            transform.GetChild(2).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void GameOverText()
    {
        StartCoroutine(GameOverFlicker());
    }

    public void RestartLevelText()
    {
        _restartText.gameObject.SetActive(true);
    }

    public void Update()
    {
        GaugeColor();

        if (_restartText == true)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("Application has Closed");
        }

    }
}
