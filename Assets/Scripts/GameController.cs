﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Player[] _players;
    private SpawnManager _spawnManager;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    private Coroutine _watchPlayersCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The SpawnManager is NULL");
        }

        _watchPlayersCoroutine = StartCoroutine(WatchPlayersCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        HandleRestart();
    }

    private void HandleRestart()
    {
        if (GetAlivePlayersCount() == 0 && Input.GetKeyDown(KeyCode.R))
        {   
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private int GetAlivePlayersCount()
    {
        int alivePlayersCount = 0;
        for (int i = 0; i < _players.Length; i++)
        {
            if (_players[i] != null)
            {
                alivePlayersCount++;
            }
        }
        return alivePlayersCount;
    }

    IEnumerator WatchPlayersCoroutine()
    {
        while (true)
        {
            if (GetAlivePlayersCount() == 0)
            {
                _spawnManager.StopSpawning();
                _gameOverText.gameObject.SetActive(true);
                _restartText.gameObject.SetActive(true);
                StartCoroutine(FlickerGameOver());
                StopCoroutine(_watchPlayersCoroutine);
            }
            yield return new WaitForEndOfFrame();
        }
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
