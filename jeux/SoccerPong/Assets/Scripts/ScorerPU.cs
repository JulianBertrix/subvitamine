using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorerPU : PowerUp
{
    // ____________________Public ____________________________
    // Static bool 
    public static bool spawnScorer, activePlayer1Scorer, activeOtherScorer;
    // Game objects
    public GameObject player1Scorer, otherScorer;

    // ____________________Private____________________________
    //Vector
    private Vector2 player1ScorerPos, otherScorerPos;
    // Float
    private float spawnScorerTimer, scorerTimer;
    // Bool
    private bool backToPosition;

    void Start ()
    {
        // Inital speed
        speed = 3.0f;
        // To spawn the power up somewhere random
        scorerPos = new Vector2(Random.Range(-4.0f, 4.0f), Random.Range(-4.0f, 3.0f));
        // Initial the spawn point position
        initPos = new Vector2(0.0f, 6.0f);
        // Intial the player 1 scorer position
        player1ScorerPos = new Vector2(-3.46f, 0.0f);
        // Intial the player 2 or AI scorer position
        otherScorerPos = new Vector2(3.46f, 0.0f);
    }

    new void Update ()
    {
        // When the score of the player 1 or the player 2 or the AI is more than 2
        if (Paddle.scorePlayer1 > 2 || Paddle.scoreOther > 2)
        {
            // To lunch the timer to spawn the scorer power up
            spawnScorer = true;
            // Call the parent update function
            base.Update();
        }
        else
        {
            // The scorer power up is not usable
            spawnScorer = false;
        }
       
        // When the power up can spawn in the game
        if (spawnScorer)
        {
            // Lunch the timer
            spawnScorerTimer += Time.deltaTime;
        }

        // After 2 seconds
        if (spawnScorerTimer >= 10.0f)
        {
            // To spawn the power up
            SpawnScorerPU();
            // To stop the timer
            spawnScorer = false;
            // To reset the timer
            spawnScorerTimer = 0.0f;
        }

        // When the player 1 scorer is active
        if (activePlayer1Scorer)
        {          
            // Lunch the timer
            scorerTimer += Time.deltaTime;
            // To make the player 1 scorer visible
            player1Scorer.SetActive(true);
            // Move the player 1 scorer
            player1Scorer.GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;

            // When the player 1 scorer is to his end position
            if (player1Scorer.transform.position.x >= otherScorerPos.x)
            { 
                // Stop to move the player 1 scorer
                speed = 0.0f;
            }
        }

        // When the player 2 or the AI scorer is active
        if (activeOtherScorer)
        {           
            // Lunch the timer
            scorerTimer += Time.deltaTime;
            // To make the player 2 or the AI scorer visible
            otherScorer.SetActive(true);
            // Move the player 2 or AI scorer
            otherScorer.GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;

            // When the player 2 or AI scorer is to his end position
            if (otherScorer.transform.position.x <= player1ScorerPos.x)
            {
                // Stop to move the player 2 or AI scorer
                speed = 0.0f;
            }
        }

        // To Reset scorers position
        if (backToPosition)
        {
            // Reset player 1 scorer position
            player1Scorer.transform.position = player1ScorerPos;
            // Reset player 2 or AI scorer position
            otherScorer.transform.position = otherScorerPos;
            // Reset speed value
            speed = 3.0f;
            // To can move them again
            backToPosition = false;
        }

        // After 10 seconds
        if (scorerTimer >= 10.0f)
        {
            // Scorer go back to their initial position
            backToPosition = true;              
            // Hide the player 1 scorer 
            player1Scorer.SetActive(false);
            // Hide player 2 or the AI scorer
            otherScorer.SetActive(false);
            // Stop the timer
            activePlayer1Scorer = false;
            activeOtherScorer = false;
            // To can spawn the scorer power up again
            isActiveScorer = false;
            // Reset the timer
            scorerTimer = 0.0f;
        }
    }
}
