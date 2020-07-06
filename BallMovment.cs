using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovment : MonoBehaviour
{
    private GameObject gameManager;

    private Vector2 direction;

    private float radius;
    private float speed = 10f;

    [NonSerialized]
    public bool startRight;

    private AudioSource[] audioSources;

    // Start is called before the first frame update
    void Start()
    {
        audioSources = gameObject.GetComponents<AudioSource>();
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        radius = transform.localScale.x / 2;
        //swaps which way the ball starts moving
        if (startRight)
        {
            direction = Vector2.one.normalized;
        }
        else
        {
            direction = Vector2.one.normalized * -1;
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //stops everything when game is paused
        if (!GameManager.notpaused)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            if (transform.position.y < GameManager.bottomLeft.y + radius && direction.y < 0)
            {
                direction.y = -direction.y;
               // audioSources[1].Play();
            }
            else if (transform.position.y > GameManager.topRight.y - radius && direction.y > 0)
            {
                direction.y = -direction.y;
                //audioSources[1].Play();
            }

            //increment score, destroy the ball and spawns next ball when passes camera bounds
            if (transform.position.x < GameManager.bottomLeft.x - radius && direction.x < 0)
            {
                audioSources[2].Play();
                gameManager.GetComponent<GameManager>().IncrementScore(false);
                Destroy(gameObject);
                gameManager.GetComponent<GameManager>().RespawnBall();
            }
            if (transform.position.x > GameManager.topRight.x + radius && direction.x > 0)
            {
                audioSources[2].Play();
                gameManager.GetComponent<GameManager>().IncrementScore(true);
                Destroy(gameObject);
                gameManager.GetComponent<GameManager>().RespawnBall();
            }
        }

    }

    //finds the position the ball hit the paddle and returns the angle of the bounce
    float HitFactor(Vector2 ballPos, Vector2 paddlePos, float racketHeight)
    {
        return (ballPos.y - paddlePos.y) / racketHeight;
    }

    //on collision swaps direction of  the ball and speeds up
    private void OnCollisionEnter2D(Collision2D collision)
    {


        if(speed  < 20f)
        {
            speed += 1f;
        }
        float y = HitFactor(transform.position, collision.transform.position, collision.collider.bounds.size.y);
        Vector2 dir = new Vector2(direction.x *= -1, y).normalized;
        direction = dir;
        audioSources[0].Play();
    }
}
