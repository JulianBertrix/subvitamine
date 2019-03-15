using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallPU : PowerUp
{
    // ____________________Public ____________________________
    // Static bool 
    public static bool spawnFB, activePlayer1FB, activeOtherFB;
    // Game objects
    public GameObject player1FB, otherFB;
    //Audio
    public AudioSource fireBallSound;
    // Transform
    public Transform player1, other;

    // ____________________Private____________________________
    // Float
    private float spawnFBTimer, fireBallTimer;
    //Bool
    private bool shootPlayer1FB, shootOtherFB;

    void Start()
    {
        // To spawn the power up somewhere random
        positionFB = new Vector2(Random.Range(-4.0f, 4.0f), Random.Range(-4.0f, 3.0f));
        // Initial the spawn point position
        initPos = new Vector2(0.0f, 6.0f);
        // Initial the fire ball speed
        speed = 10.0f;
    }

    new void Update()
    {
        // To lunch the timer when the player 1, the player 2 or the AI fire ball is not active
        if (!activePlayer1FB || !activeOtherFB)
        {         
            // Call the parent update function
            base.Update();
        }

        // When the power up can spawn in the game
        if (spawnFB)
        {
            // Lunch the timer
            spawnFBTimer += Time.deltaTime;
        }

        // After 50 seconds
        if (spawnFBTimer >= 50.0f)
        {
            // To spawn the power up
            SpawnFireBallPU();
            // To stop the timer
            spawnFB = false; // To spawn just once
            // To reset the timer
            spawnFBTimer = 0.0f;
        }

        // When the player 1 fire ball is active
        if (activePlayer1FB)
        {
            // To make the player 1 fire ball visible
            player1FB.SetActive(true);
            spawnFB = false;
            shootPlayer1FB = true;
        }

        // When the player 2 or the AI fire ball is active
        if (activeOtherFB)
        {
            // To make the player 2 or the AI fire ball visible
            otherFB.SetActive(true);
            spawnFB = false;
            // To move the fire ball
            shootOtherFB = true;
        }

        // When the player 1 shoot the fire ball 
        if (shootPlayer1FB)
        {
            // Shoot the player 1 fire ball
            ShootPlayer1FB();           
        }

        // When the player 1 shoot the fire ball 
        if (shootOtherFB)
        {
            // Shoot the player 1 fire ball
            ShootOtherFB();          
        }
    }

    // Function to shoot the player 1 fire ball
    void ShootPlayer1FB()
    {
        // Move the player 1 fire ball
        player1FB.GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
        // Play the fire ball sound
        fireBallSound.Play();

        // When the player 1 fire ball position is out
        if (player1FB.transform.position.x >= 8.0f)
        {
            // When the fire ball is in the player 2 or the AI goal position
            if (player1FB.transform.position.y >= -2.65f && player1FB.transform.position.y <= 2.65f)
            {
                // The player 1 scoring
                Paddle.scorePlayer1 += 1;
            }

            // Reset the player 2 or the AI fire ball position
            otherFB.transform.position = other.position;
            // To lunch the timer to spawn an other fire ball power up
            activeOtherFB = false;
            // To lunch the timer to spawn an other fire ball power up
            activePlayer1FB = false;
            // To stop to shoot the player 1 fire ball 
            shootPlayer1FB = false;
            // To lunch the timer
            spawnFB = true;
            // Hide the fire ball
            player1FB.SetActive(false);
            // To can spawn the fire ball power up again
            isActiveFB = false;
        }       
    }

    // Function to shoot the player 2 or th AI fire ball
    void ShootOtherFB()
    {
        // Move the player 2 or th AI fire ball 
        otherFB.GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;
        // Play the fire ball sound
        fireBallSound.Play();

        // When the player2 or the AI fire ball position is out       
        if (otherFB.transform.position.x <= -8.0f)
        {
            // When the fire ball is in the player 1 goal position
            if (otherFB.transform.position.y >= -2.65f && otherFB.transform.position.y <= 2.65f)
            {
                // The player2 or the AI scoring
                Paddle.scoreOther += 1;
            }

            // Reset the player 2 or the AI fire ball position
            otherFB.transform.position = other.position;
            // To lunch the timer to spawn an other fire ball power up
            activeOtherFB = false;
            // To lunch the timer to spawn an other fire ball power up
            activeOtherFB = false;
            // // To stop to shoot the player 2 or the AI fire ball 
            shootOtherFB = false;
            // To lunch the timer
            spawnFB = true;
            // Hide the fire ball
            otherFB.SetActive(false);
            // To can spawn the fire ball power up again
            isActiveFB = false;
        }
    }
}
