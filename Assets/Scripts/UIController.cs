using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public enum GameStatusEnum
{
    menu,
    unpaused,
    paused
}
public class UIController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject firstPanel;
    public GameObject nextLevel;
    public GameObject winScreen;
    public GameObject endScreen;
    public Text mapScoreText;
    public Text totalScoreText;
    public Text endScore;
    public static UIController instance;
    public GameStatusEnum gameStatus;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        Scores.instance.OnMapScoreChangedEvent.AddListener(ScoresUI);   
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        totalScoreText.text = "Score = " + Scores.totalScore.ToString();
        if (Scores.totalScore == 0) firstPanel.SetActive(true);
        else nextLevel.SetActive(true);
        gameStatus = GameStatusEnum.paused;
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&!(gameStatus == GameStatusEnum.paused))
        {
            if(gameStatus == GameStatusEnum.menu)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void ScoresUI(int mapScore)
    {
        mapScoreText.text = "Map Score : " + mapScore.ToString();
    }
    public void StartGame()
    {
        firstPanel.SetActive(false);
        nextLevel.SetActive(false);
        Time.timeScale = 1f;
        gameStatus = GameStatusEnum.unpaused;
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameStatus = GameStatusEnum.unpaused;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameStatus = GameStatusEnum.menu;
    }
    public void NextLevel()
    {
        winScreen.SetActive(false);
        Initialize();
        Scores.instance.Restart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void EndGame()
    {
        endScreen.SetActive(true);
        Time.timeScale = 0f;
        endScore.text += Scores.totalScore.ToString();
        gameStatus = GameStatusEnum.paused;
    }
    public void MainMenu()
    {
        Destroy(instance.gameObject);
        Destroy(Scores.instance.gameObject);
        SceneManager.LoadScene("MenuStart");
    }

    public void RestartGame()
    {
        endScreen.SetActive(false);
        pauseMenuUI.SetActive(false);
        Initialize();
        Scores.instance.RestartGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndLevel()
    {
        winScreen.SetActive(true);
        Scores.totalScore += Scores.mapScore;
        gameStatus = GameStatusEnum.paused;
    }
}
