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
    private Image _livesImage;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;


    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _restartText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);

    }

    public void UpdateScore(int _playerScore)
    {
        _scoreText.text = "Score: " + _playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
 
        _livesImage.sprite = _livesSprites[currentLives];        

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
        if(_restartText == true)
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
