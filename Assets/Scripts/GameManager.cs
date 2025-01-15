using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //the singleton for overall game speed 
    public static GameManager Instance { get; private set; }
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f; // kinda like a multiplier component
    public float gameSpeed { get; private set; }

    public int winScore = 500;
    private float score;

    public bool isGameStarted { get; private set; }

    public TextMeshProUGUI gameTitle;
    public TextMeshProUGUI reach500;
    public TextMeshProUGUI gameOver;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winMessage;
    public TextMeshProUGUI ThankYouMessage;
    public Button retryButton;
    private Player player;
    private Spawner spawner;

    private void Awake()
    {
        //set up the singleton method foe the general game speed
        if(Instance == null){
            Instance = this;
        }
        else
        {
            //to make sure a second one is not created at all, can't have more than one instance 
            DestroyImmediate(gameObject);
        }
    }

    private void Start()
    {
        PauseGame(); //starting the game on pause state

        gameSpeed = initialGameSpeed;

        gameTitle.gameObject.SetActive(true);
        reach500.gameObject.SetActive(true);

        player = FindObjectOfType<Player>();
        spawner = FindObjectOfType<Spawner>();
    }

    private void Update()
    {
        if (isGameStarted == false && Input.GetButtonDown("Jump"))  
        {
            NewGame();
        }
        // Increase game speed over time
        if (isGameStarted)
        {
            gameSpeed += gameSpeedIncrease * Time.deltaTime;
            score += gameSpeed * Time.deltaTime;
            scoreText.text = Mathf.RoundToInt(score).ToString("D3");

            if (Mathf.RoundToInt(score) >= winScore)
            {
                WinGame();
            }
        }

    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        isGameStarted = false;
    }

    public void NewGame()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        foreach(var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        Time.timeScale = 1f;
        enabled = true;
        isGameStarted = true;
        gameSpeed = initialGameSpeed;
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
        score = 0f;
        scoreText.text = "000";


        spawner.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        gameTitle.gameObject.SetActive(false);
        reach500.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        winMessage.gameObject.SetActive(false);
        ThankYouMessage.gameObject.SetActive(false);

        player.ResetPlayer();
    }

    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;

        spawner.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

    private void WinGame()
    {
        Time.timeScale = 0f;
        isGameStarted = false;
        gameSpeed = 0f;
        enabled = false;

        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        spawner.gameObject.SetActive(false);
        winMessage.gameObject.SetActive(true);
        ThankYouMessage.gameObject.SetActive(true);

        player.PlayerWin();

    }
}
