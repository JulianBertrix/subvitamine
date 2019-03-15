using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownScalePU : PowerUp
{
    // ____________________Public ____________________________
    //Audio
    public AudioSource scaleCollisionSound;
    // Static bool
    public static bool canSpawn;

    // ____________________Private____________________________
    // Float
    private float spawnTimer;
    // Bool
    private bool move, hitableUp, hitableDown;

    void Start()
    {
        // Initial speed
        speed = 1.0f;
        // Initial the spawn point position
        initPos = new Vector2(0.0f, 6.0f);
    }

    new void Update()
    {
        // To lunch the timer    
        if (canSpawn)
        {
            spawnTimer += Time.deltaTime;
        }

        // After 20 seconds 
        if (spawnTimer >= 20.0f)
        {
            // Move the power up
            move = true;
            // Spawn the power up
            SpawnDownScalePU();
            // Stop the timer
            canSpawn = false;
            // Reset the timer 
            spawnTimer = 0.0f;
        }

        // When power up don't move
        if (!move)
        {
            // Reset the power up postion
            transform.position = initPos;
            // Reset the power up speed
            speed = 1.0f;
        }
    }

    public void SpawnDownScalePU()
    {
        // To know whitch side to spawn  
        SpawnScalePU();

        // When he spawn to the right
        if (spawnRight)
        {
            // To choose a random value between 0 and 2 
            float rand = Random.Range(0, 2);
            // When the random value is less than 1
            if (rand < 1)
            {
                // Spawn the power up to the world up right angle
                transform.position = new Vector2(7.57f, 4.35f);
                // Move the power up
                GetComponent<Rigidbody2D>().velocity = new Vector2(-2.0f, -5.0f) * speed;
            }
            else
            {
                // Spawn the power up to the world down right angle
                transform.position = new Vector2(7.57f, -4.35f);
                // Move the power up
                GetComponent<Rigidbody2D>().velocity = new Vector2(-2.0f, 5.0f) * speed;
            }

            // To calculate whitch side to move
            goLeft = true;
            goRight = false;
            // Stop to spwan
            spawnRight = false;
        }

        // When he spawn to the right
        if (spawnLeft)
        {
            // To choose a random value between 0 and 2 
            float rand = Random.Range(0, 2);
            // When the random value is less than 1
            if (rand < 1)
            {
                // Spawn the power up to the world up left angle
                transform.position = new Vector2(-7.57f, 4.35f);
                // Move the power up
                GetComponent<Rigidbody2D>().velocity = new Vector2(5.0f, -5.0f) * speed;
            }
            else
            {
                // Spawn the power up to the world down left angle
                transform.position = new Vector2(-7.57f, -4.35f);
                // Move the power up
                GetComponent<Rigidbody2D>().velocity = new Vector2(5.0f, 5.0f) * speed;
            }

            // To calculate whitch side to move
            goRight = true;
            goLeft = false;
            // Stop to spwan
            spawnLeft = false;
        }
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        // Hit the player 1
        if (trig.gameObject.tag == "Player")
        {
            // To restet scale power up position 
            transform.position = initPos;
            // Change the player 2 or the AI scale
            Paddle.scaleDownOther = true;

            // When the player play an online match
            if (Paddle.online)
            {
                // Change the player 2 scale
                NetworkPlayerController.scaleDownPlayer2 = true;
            }

            // To lunch the timer
            canSpawn = true;
            // Stop to move the power up
            move = false;
        }

        // Hit the player 2 or the AI
        if (trig.gameObject.tag == "Other")
        {
            // To restet scale power up position 
            transform.position = initPos;
            // Change the player 1 scale
            Paddle.scaleDownPlayer1 = true;

            // When the player play an online match
            if (Paddle.online)
            {
                // Change the player 1 scale
                NetworkPlayerController.scaleDownPlayer1 = true;
            }

            // To lunch the timer
            canSpawn = true;
            // Stop to move the power up
            move = false;
        }

        // Hit the player 1 or the player 2 or the AI score zone
        if (trig.gameObject.tag == "GoalPlayer1" || (trig.gameObject.tag == "GoalOther"))
        {
            // To restet scale power up position 
            transform.position = initPos;
            // To lunch the timer
            canSpawn = true;
            // Stop to move the power up
            move = false;
        }

        // Hit the wall up and the wall down when go to the left
        if (goLeft && trig.gameObject.tag == "WallUp" || goLeft && trig.gameObject.tag == "WallDown")
        {
            // Up the power up speed
            speed = 10.0f;
            // Calculate hit Factor
            float y = HitFactor(transform.position, trig.transform.position, trig.GetComponent<Collider2D>().bounds.size.y);
            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(-1.0f, y).normalized;
            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
            goRight = false;
            // Player the power up sound
            scaleCollisionSound.Play();
        }

        // Hit the wall up and the wall down when go to the right
        if (goRight && trig.gameObject.tag == "WallUp" || goRight && trig.gameObject.tag == "WallDown")
        {
            // Up the power up speed
            speed = 10.0f;
            // Calculate hit Factor
            float y = HitFactor(transform.position, trig.transform.position, trig.GetComponent<Collider2D>().bounds.size.y);
            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(1.0f, y).normalized;
            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
            goLeft = false;
            // Player the power up sound
            scaleCollisionSound.Play();
        }

        // Hit the side walls right 
        if (trig.gameObject.tag == "SideWallRight")
        {
            // Up the power up speed
            speed = 10.0f;
            // Calculate hit Factor
            float y = HitFactor(transform.position, trig.transform.position, trig.GetComponent<Collider2D>().bounds.size.y);
            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(-1.0f, y).normalized;
            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
            goLeft = true;
            // Player the power up sound
            scaleCollisionSound.Play();
        }

        // Hit the side walls left 
        if (trig.gameObject.tag == "SideWallLeft")
        {
            // Up the power up speed
            speed = 10.0f;
            // Calculate hit Factor
            float y = HitFactor(transform.position, trig.transform.position, trig.GetComponent<Collider2D>().bounds.size.y);
            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(1.0f, y).normalized;
            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
            goRight = true;
            // Player the power up sound
            scaleCollisionSound.Play();
        }
    }
}
