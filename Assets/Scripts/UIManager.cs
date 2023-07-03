﻿using System.Collections;
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



    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _restartText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        _ammoCounter.text = "Ammo: " + 15 + "/15";
        _thrusterGauge = GetComponentInChildren<Slider>();
        _thrusterColor = _thrusterGauge.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        if(_thrusterGauge == null)
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

    public void UpdateScore(int _playerScore)
    {
        _scoreText.text = "Score: " + _playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
 
        _livesImage.sprite = _livesSprites[currentLives];        

    }

    public void UpdateAmmo(int _ammo)
    {
        _ammoCounter.text = "Ammo: " + _ammo.ToString() + "/15";
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
