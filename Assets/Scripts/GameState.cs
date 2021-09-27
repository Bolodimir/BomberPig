using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public delegate void StartGameHandler();
    public event StartGameHandler StartGame;

    [SerializeField] private GameOverMenu _gameOverMenu;
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _winMenu;

    private int _enemyNumber;
    private bool _playerDead;
    private bool _playerWon;

    private void Start()
    {
        FindObjectOfType<Player>().PlayerDie += OnPlayerDie;
    }
    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnStartButtonClick()
    {
        StartGame?.Invoke();
        _startMenu.SetActive(false);
    }
    public void OnPlayerDie()
    {
        if (_playerWon) return;
        _gameOverMenu.gameObject.SetActive(true) ;
        _gameOverMenu.Appear();
        _playerDead = true;
    }
    public void RegisterEnemy()
    {
        _enemyNumber++;
    }
    public void EnemyKilled()
    {
        _enemyNumber--;
        if(_enemyNumber == 0 && !_playerDead)
        {
            Win();
        }
    }
    public void Win()
    {
        _playerWon = true;
        _winMenu.SetActive(true);
    }
}
