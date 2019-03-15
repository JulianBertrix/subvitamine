using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKeeperPU : PowerUp
{
    // ____________________Public ____________________________
    // Static bool 
    public static bool spawnGK, activePlayer1GK, activeOtherGK;
    // Game objects
    public GameObject player1GK, otherGK;

    // ____________________Private____________________________
    // Float
    private float spawnGKTimer, goalKeeperTimer;

    void Start ()
    {
        // To spawn the power up somewhere random
        positionGK = new Vector2(Random.Range(-4.0f, 4.0f), Random.Range(-4.0f, 3.0f));
        // Initial the spawn point position
        initPos = new Vector2(0.0f, 6.0f);
    }

    new void Update()
    {
        // When the player begin a competition
        if (Paddle.startGame)
        {
            // When the score of the player 1 or the player 2 or the AI is between 0 and 2
            if (Paddle.scorePlayer1 <= 4 || Paddle.scoreOther <= 4)
            {
                // To lunch the timer to spawn the goalkeeper power up
                spawnGK = true;
                // Call the parent update function
                base.Update();
            }
        }
        // When the power up can spawn in the game
        if (spawnGK)
        {
            // Lunch the timer
            spawnGKTimer += Time.deltaTime;
        }

        // After 2 seconds
        if (spawnGKTimer >= 10.0f)
        {
            // To spawn the power up
            SpawnGoalKeeperPU();
            // To stop the timer
            spawnGK = false; // To spawn just once
            // To reset the timer
            spawnGKTimer = 0.0f;
        }

        // When the player 1 goalkeeper is active
        if (activePlayer1GK)
        {
            // Lunch the timer
            goalKeeperTimer += Time.deltaTime;
            // To make the player 1 goalkeeper visible
            player1GK.SetActive(true);
        }

        // When the player 2 or the AI goalkeeper is active
        if (activeOtherGK)
        {
            // Lunch the timer
            goalKeeperTimer += Time.deltaTime;
            // To make the player 2 or the AI goalkeeper visible
            otherGK.SetActive(true);
        }

        // After 10 seconds
        if (goalKeeperTimer >= 10.0f)
        {
            // Hide the player 1 goalkeeper 
            player1GK.SetActive(false);
            // Hide player 2 or the AI goalkeeper
            otherGK.SetActive(false);
            // Stop the timer
            activePlayer1GK = false;
            activeOtherGK = false;
            // Reset the timer
            goalKeeperTimer = 0.0f;
            // To can spawn the goalkeeper power up again
            isActiveGK = false;
        }
    }
}
