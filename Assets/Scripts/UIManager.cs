using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Image _livesImage;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartText;

    void Start()
    {
        UpdateScore(0);
        UpdateLives(3);
        StartCoroutine(FlickerGameOver());
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int lives)
    {
        _livesImage.sprite = _liveSprites[lives];
        bool isGameOver = lives <= 0;
        _gameOverText.gameObject.SetActive(isGameOver);
        _restartText.gameObject.SetActive(isGameOver);
    }

    IEnumerator FlickerGameOver()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _gameOverText.enabled = !_gameOverText.enabled;
        }
    }
}
