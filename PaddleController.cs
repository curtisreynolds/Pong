using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    // Start is called before the first frame update


    [NonSerialized]
    public bool AIOn;
    public bool isRight;

    [NonSerialized]
    public int aiDifficulty;

    public float paddleSpeed = 4.0f;

    public GameObject paddle;

    private string input;

    private float height;
    private float move;
    private float paddleToBallDifferance;

    private GameObject ball;

    //initialize the paddles setting them to the correct side and if its Ai controller or not
    public void Init(bool isRightPaddle)
    {
        isRight = isRightPaddle;
        Vector2 pos = Vector2.zero;
        if (isRightPaddle)
        {
            //place paddle on right side of screen
            pos = new Vector2(GameManager.topRight.x, 0);
            pos -= Vector2.right * transform.localScale.x;
            input = "Vertical";
            height = GetComponent<SpriteRenderer>().bounds.size.y;
            transform.name = "Right Paddle";
        }
        else
        {
            //place paddle on left side of screen
            pos = new Vector2(GameManager.bottomLeft.x, 0);
            pos += Vector2.right * transform.localScale.x;
            input = "Horizontal";
            height = GetComponent<SpriteRenderer>().bounds.size.y;
            transform.name = "Left Paddle";
        }
        //updates paddle possiton
        transform.position = pos;
    }


    // Update is called once per frame
    void Update()
    {
        //stops movement if game is paused
        if (!GameManager.notpaused)
        {
            if (AIOn)
            {
                PaddleAI();
            }
            else
            {
                move = Input.GetAxis(input) * Time.deltaTime * paddleSpeed;
            }

            //Restrect paddle if it's out of bounds
            if (transform.position.y < GameManager.bottomLeft.y + height / 2 && move < 0)
            {
                move = 0;
            }
            if (transform.position.y > GameManager.topRight.y - height / 2 & move > 0)
            {
                move = 0;
            }
            //applies direction to paddle
            transform.Translate(move * Vector2.up);
        }
    }
    //Ai for paddle
    private void PaddleAI()
    {
        //checks to see if the ball is referenced if not gets reference
        if (ball != null)
        {
            //gets distance from paddle to ball in the y axis
            paddleToBallDifferance = transform.position.y - ball.transform.position.y;
            //moves paddles based on difficulty of AI and distance from ball
            //Hard Difficulty
            if(aiDifficulty == 0)
            {
                if (paddleToBallDifferance < -.5)
                {
                    move = 1 * Time.deltaTime * 8;
                }
                else if (paddleToBallDifferance < -.1f)
                {
                    move = 1 * Time.deltaTime * 2f;
                }
                else if (paddleToBallDifferance > -0.1f && paddleToBallDifferance < 0.1f)
                {
                    move = 0 * Time.deltaTime * 3;
                }

                else if (paddleToBallDifferance > .1f && paddleToBallDifferance < .5)
                {
                    move = -1 * Time.deltaTime * 2f;
                }
                else if (paddleToBallDifferance > .5)
                {
                    move = -1 * Time.deltaTime * 8;
                }
            //Medium Difficulty
            }else if(aiDifficulty == 1)
            {
                if (paddleToBallDifferance < -.7)
                {
                    move = 1 * Time.deltaTime * 7.25f;
                }
                else if (paddleToBallDifferance < -.1f)
                {
                    move = 1 * Time.deltaTime * 1f;
                }
                else if (paddleToBallDifferance > -0.1f && paddleToBallDifferance < 0.1f)
                {
                    move = 0 * Time.deltaTime * 3;
                }
                else if (paddleToBallDifferance > .1f && paddleToBallDifferance < .5)
                {
                    move = -1 * Time.deltaTime * 1f;
                }
                else if (paddleToBallDifferance > .7)
                {
                    move = -1 * Time.deltaTime * 7.25f;
                }
            //Easy Difficulty
            }else if(aiDifficulty == 2)
            {
                if (paddleToBallDifferance < -.8f)
                {
                    move = 1 * Time.deltaTime * 6;
                }
                else if (paddleToBallDifferance < -.1f)
                {
                    move = 1 * Time.deltaTime * 2f;
                }
                else if (paddleToBallDifferance > -0.1f && paddleToBallDifferance < 0.1f)
                {
                    move = 0 * Time.deltaTime * 3;
                }
                else if (paddleToBallDifferance > .1f && paddleToBallDifferance < .5)
                {
                    move = -1 * Time.deltaTime * 2f;
                }
                else if (paddleToBallDifferance > .8f)
                {
                    move = -1 * Time.deltaTime * 6;
                }
            }

        }
        else
        {
            //finds ball if its destroyed or not referenced
            ball = GameObject.FindGameObjectWithTag("ball");
        }
    }

}