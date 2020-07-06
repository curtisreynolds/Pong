using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static Vector2 bottomLeft;
    public static Vector2 topRight;

    public static bool notpaused;

    public GameObject scoreText;
    public GameObject ball;
    public GameObject paddle;

    private Text[] playerScores;
    private CanvasGroup[] canvases;

    private int leftScore;
    private int rightScore;
    private int aiDifficulty;

    private bool wentRight;
    private bool singlePlayer;
    private bool reloaded;
    private bool gameStarted = false;

    //prepares canvas at start
    private void Start()
    {
        reloaded = true;
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        playerScores = scoreText.GetComponentsInChildren<Text>();
        canvases = gameObject.GetComponentsInChildren<CanvasGroup>();

    }

    // Update is called once per frame
    void Update()
    {
        //allows pause/unpause on esc press
        if (Input.GetKeyDown(KeyCode.Escape) && !notpaused && gameStarted) {
            notpaused = true;
            canvases[3].alpha = 1f;
            canvases[3].blocksRaycasts = true;
            
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && notpaused || reloaded && gameStarted)
        {
            notpaused = false;
            canvases[3].alpha = 0f;
            canvases[3].blocksRaycasts = false;
            reloaded = false;
        }
    }


    //increases score by one for the player that scored
    public void IncrementScore(bool leftPlayerScored)
    {
        if (leftPlayerScored)
        {
            leftScore += 1;
            playerScores[0].text = leftScore.ToString();
        }
        else
        {
            rightScore += 1;
            playerScores[1].text = rightScore.ToString();
        }
    }


    //respawns the ball once its died and swaps starting direction
    public void RespawnBall()
    {
        if (!wentRight)
        {
            GameObject newBall = Instantiate(ball);
            newBall.GetComponent<BallMovment>().startRight = true;
            wentRight = true;
        }
        else
        {
            GameObject newBall = Instantiate(ball);
            newBall.GetComponent<BallMovment>().startRight = false;
            wentRight = false;
        }

    }

    //starts game, hides canvases, loads paddles and  ball
    public void StartGame()
    {
        canvases[0].alpha = 1f;
        GameObject leftPaddle = Instantiate(paddle);
        leftPaddle.GetComponent<PaddleController>().Init(false);
        leftPaddle.GetComponent<PaddleController>().AIOn = false;
        GameObject rightPaddle = Instantiate(paddle);
        rightPaddle.GetComponent<PaddleController>().Init(true);
        rightPaddle.GetComponent<PaddleController>().AIOn = singlePlayer;
        rightPaddle.GetComponent<PaddleController>().aiDifficulty = aiDifficulty;
        Instantiate(ball);
        gameStarted = true;
    }

    //opens difficulty options
    public void SinglePlayer()
    {
        canvases[1].alpha = 0f;
        canvases[1].blocksRaycasts = false;
        canvases[2].alpha = 1f;
        canvases[2].blocksRaycasts = true;
    }

    //starts game in two player mode
    public void TwoPlayer()
    {
        singlePlayer = false;
        canvases[1].alpha = 0f;
        canvases[1].blocksRaycasts = false;
        canvases[2].alpha = 0f;
        canvases[2].blocksRaycasts = false;
        StartGame();
    }

    //sets difficulty then starts game
    public void SetDifficulty(int difficulty)
    {
        singlePlayer = true;
        aiDifficulty = difficulty;
        canvases[1].alpha = 0f;
        canvases[1].blocksRaycasts = false;
        canvases[2].alpha = 0f;
        canvases[2].blocksRaycasts = false;
        StartGame();
    }

    //reloads scene
    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
        notpaused = true;
    }
    
    //quits application
    public void Quit()
    {
        Application.Quit();
    }
}
