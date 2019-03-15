using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Static bool
    public static bool isActiveGK, isActiveFB, isActiveScorer;
    // Transform
    public Transform goalKeeperPU, scorerPU, fireBallPU;
    // Vector   
    public Vector2 positionGK, positionFB, scorerPos, initPos;
    // Bool
    public bool spawnRight, spawnLeft, goLeft, goRight;
    // Float
    public float speed;

    void Start()
    {
       
    }

    public void Update()
    {
        // When the player 1, the player 2 or the AI take the goalkeeper power up
        if (isActiveGK)
        {
            // Reset the power up position 
            goalKeeperPU.position = initPos;
        }

        // When the player 1, the player 2 or the AI take the fire ball power up
        if (isActiveFB)
        {
            // Reset the power up position 
            fireBallPU.position = initPos;
        }

        // When the player 1, the player 2 or the AI take the scorer power up
        if (isActiveScorer)
        {
            // Reset the power up position 
            scorerPU.position = initPos;
        }
    }

    // Function to spawn the scorer power up
    public void SpawnScorerPU()
    {
        // When the score of the player 1 or the player 2 or the AI is more than 2
        if (Paddle.scorePlayer1 > 2 || Paddle.scoreOther > 2)
        {
            // To spawn the scorer power up to a random position
            scorerPU.position = scorerPos;
        }
    }

    // Function to spawn the goalkeeper power up
    public void SpawnGoalKeeperPU()
    {
        // When the score of the player 1 or the player 2 or the AI is between 0 and 2
        if (Paddle.scorePlayer1 <= 2 || Paddle.scoreOther <= 2)
        {
            // To spawn the goalkeeper power up to a random position
            goalKeeperPU.position = positionGK;
        }
    }

    // Function to spawn the fire ball power up
    public void SpawnFireBallPU()
    {
        // To spawn the fire Ball power up to a random position
        fireBallPU.position = positionFB;
    }

    // Function to spawn the scale power up to the left or to the right 
    public void SpawnScalePU()
    {
        // To choose a random value between 0 and 2
        float rand = Random.Range(0, 2);
        // When the random value is less than 1
        if (rand < 1)
        {
            // Spawn to the right
            spawnRight = true;
        }
        else
        {
            // Spawn to the left
            spawnLeft = true;
        }
    }

    // Used to calculate angle direction when the ball collide with something.
    public float HitFactor(Vector2 ballPos, Vector2 paddlePos, float paddleHeight)
    {
        return (ballPos.y - paddlePos.y) / paddleHeight;
    }
}
