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
    private Image _imageLives;
    [SerializeField]
    private Sprite[] _livesSprite;
    [SerializeField]
    private GameObject _gameOverText;
    [SerializeField]
    private GameObject _restartText;
    private GameManager _gameManager;


    void Start()
    {
        _scoreText.text = "Score:" + 0;

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.Log("GameManager is Null");
        }
    }


    public void UpdateScore(int score)
    {
        _scoreText.text = "Score:" + score;
    }

    public void UpdateLives(int currentLives)
    {
        _imageLives.sprite = _livesSprite[currentLives];

        if (currentLives == 0)
        {
            GameOver();
            RestartText();
            _gameManager.GameOver();
        }
    }

    public void GameOver()
    {
        StartCoroutine(TextFlicker());
    }

    public void RestartText()
    {
        _restartText.SetActive(true);
    }

    IEnumerator TextFlicker()
    {
        while (true)
        {
            _gameOverText.SetActive(true);
            yield return new WaitForSeconds(.2f);
            _gameOverText.SetActive(false);
            yield return new WaitForSeconds(.2f);
            _gameOverText.SetActive(true);
        }
    }

}
