using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _highScoreText;

    [SerializeField]
    private Image _livesImage;

    [SerializeField]
    private Sprite[] _liveSprites;

    private int _highScore = 0;

    private void Start()
    {
        UpdateHighScore(PlayerPrefs.GetInt("HighScore", 0));
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
        if (score > _highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            UpdateHighScore(score);
        }
    }

    private void UpdateHighScore(int highScore)
    {
        _highScore = highScore;
        _highScoreText.text = "High score: " + _highScore;
    }

    public void UpdateLives(int lives)
    {
        _livesImage.sprite = _liveSprites[lives];
    }
}
