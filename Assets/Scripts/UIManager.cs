using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    void Start()
    {
        _scoreText.text = "Score:" + 0;
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score:" + score;
    }

    public void UpdateLives(int currentLives)
    {
        _imageLives.sprite = _livesSprite[currentLives];
    }

    public void GameOver()
    {
        _gameOverText.SetActive(true);
    }


}
