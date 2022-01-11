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

        if (currentLives == 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        StartCoroutine(TextFlicker());
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
