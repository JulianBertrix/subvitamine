using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // ____________________Public ____________________________
    // Static bool
    public static bool startEngage;
    //Audio
    public AudioSource ballSound, goalSound, scorerSound, goalKeeperSound;
    // Float
    public float speed = 30.0f;

    // ____________________Private____________________________
    // Vector
    private Vector2 startPos;
    // Float
    private float engageTimer, debugTimer; // Timer
    private float debugBallPosition = 4.74f; // Debug the ball position
    // Bool
    private bool scoring; // Goal
    private bool player1Shoot, otherShoot, playerBallGoBack, otherBallGoBack; // To calculate the ball direction in collision with the player 1, the player 2 or the AI
    private bool scorerPlayer1Shoot, scorerOtherShoot; // To calculate the ball direction in collision with the player 1, the player 2 or the AI scorer power up

    void Start()
    {
        // Initial start postion  
        startPos = new Vector2(0.0f, 0.0f);
    }

    void Update()
    {
        // When the match begin.
        if (startEngage)
        {
            // Initial velocity
            EngageBall();
            startEngage = false;
        }

        // When the player 1, the player 2 or the AI scoring
        if (scoring)
        {
            // Up the timer value
            engageTimer += Time.deltaTime;
            // Reset the ball position to the start position
            transform.position = startPos;
            // To reset the AI to the ball position
            Paddle.shoot = true;
        }

        // After 2 seconds
        if (engageTimer >= 3.0f)
        {
            // Stop to up the timer value and to place the ball position to the start position
            scoring = false;
            // Reset the timer at 0
            engageTimer = 0.0f;
            // Initial velocity
            EngageBall();
            // To restart to move the AI random
            Paddle.shoot = true;
        }

        // When the match is finished
        if (Paddle.scorePlayer1 >= Paddle.maxScore || Paddle.scoreOther >= Paddle.maxScore)
        {
            // Reset the ball position to the start position
            transform.position = startPos;
            // Restet the scoring boolean
            scoring = false;
            // Reset the engage timer at 0
            engageTimer = 0.0f;
        }

        // When the ball move only on X axis to the up or to the down of the area
        if (transform.position.y >= debugBallPosition || transform.position.y <= -debugBallPosition)
        {
            // Lunch the timer to debug the ball position
            debugTimer += Time.deltaTime;
        }
        // When the ball is just collided with the wall up or the wall down  
        else
        {
            // Reset the timer
            debugTimer = 0.0f;
        }

        // After 2 seconds
        if (debugTimer >= 2.0f)
        {
            // When the ball is blocked to the up
            if (transform.position.y >= debugBallPosition)
            {
                // Change the ball position on Y axis
                transform.position = new Vector2(transform.position.x, transform.position.y - 0.5f);
            }

            // When the ball is blocked to the down
            if (transform.position.y <= -debugBallPosition)
            {
                // Change the ball position on Y axis
                transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
            }

            // Reset the timer
            debugTimer = 0.0f;
        }
    }
       
    void EngageBall()
    {
        // Stop to calculate who than players or AI shoot the ball    
        player1Shoot = false;
        otherShoot = false;

        // Inital velocity in random direction when a player engage
        float rand = Random.Range(0, 2);
        if (rand < 1)
        {
            // Move to the right
            GetComponent<Rigidbody2D>().velocity = Vector2.right * speed;
        }
        else
        {
            // Move to the left
            GetComponent<Rigidbody2D>().velocity = Vector2.left * speed;
        }
    }

    // Used to calculate angle direction when the ball collide with something.
    float HitFactor(Vector2 ballPos, Vector2 paddlePos, float paddleHeight)
    {               
        return (ballPos.y - paddlePos.y) / paddleHeight;
    }

    void OnCollisionEnter2D(Collision2D col)
    {    
        // Hit the player 1
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "Player1GK")
        {
            // To calculate walls direction when the player 1 hit the ball 
            player1Shoot = true;
            // To calculate AI direction when the player hit the ball  
            Paddle.shoot = true;
            // Calculate hit Factor
            float y = HitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);
            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(1.0f, y).normalized;
            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
            // To stop to calculate walls direction when the player 2 or AI hit the ball 
            otherShoot = false;
            // To stop to calculate walls direction when the player 2 or AI scorer hit the ball 
            scorerOtherShoot = false;
            // Play the ball sound
            ballSound.Play();
        }

        // Hit the player 2 or the AI
        if (col.gameObject.tag == "Other" || col.gameObject.tag == "OtherGK")
        {
            // To calculate walls direction when the player 2 or AI hit the ball
            otherShoot = true;
            // To stop to calculate AI direction when the ai hit the ball
            Paddle.shoot = false;
            // Calculate hit Factor
            float y = HitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);
            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(-1.0f, y).normalized;
            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
            // To stop to calculate walls direction when the player 1 hit the ball
            player1Shoot = false;
            // To stop to calculate walls direction when the player 1 scorer hit the ball
            scorerPlayer1Shoot = false;
            // Play the ball sound
            ballSound.Play();
        }

        // Hit the wall up and the wall down when player 1 shoot or the player 1 scorer shoot
        if (player1Shoot || scorerPlayer1Shoot)
        {
            // Stop to inverse the ball direction in player 2 or AI side direction
            otherBallGoBack = false;

            if (col.gameObject.tag == "WallUp" || col.gameObject.tag == "WallDown")
            {
                // Calculate hit Factor
                float y = HitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);
                // Calculate direction, make length=1 via .normalized
                Vector2 dir = new Vector2(1.0f, y).normalized;
                // Set Velocity with dir * speed
                GetComponent<Rigidbody2D>().velocity = dir * speed;
                // Play the ball sound
                ballSound.Play();
            }
        }

        // Hit the wall up and the wall down when player 2 or AI shoot or the player 2 or AI scorer shoot
        if (otherShoot || scorerOtherShoot)
        {
            // Stop to inverse the ball direction in player 1 side direction
            playerBallGoBack = false;

            if (col.gameObject.tag == "WallUp" || col.gameObject.tag == "WallDown")
            {
                // Calculate hit Factor
                float y = HitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);
                // Calculate direction, make length=1 via .normalized
                Vector2 dir = new Vector2(-1.0f, y).normalized;
                // Set Velocity with dir * speed
                GetComponent<Rigidbody2D>().velocity = dir * speed;
                // Play the ball sound
                ballSound.Play();
            }
        }

        // Hit the side walls right when player 1 shoot
        if (col.gameObject.tag == "SideWallRight")
        {
            // To inverse the ball direction in player 1 side direction
            playerBallGoBack = true;
            // To stop to calculate walls direction when the player 1 scorer hit the ball
            scorerPlayer1Shoot = false;
            // To stop to calculate walls direction when the player 1 hit the ball
            player1Shoot = false;
            // Stop to inverse the ball direction in player 2 or AI side direction
            otherBallGoBack = false;
            // Calculate hit Factor
            float y = HitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);
            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(-1.0f, y).normalized;
            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
            // Play the ball sound
            ballSound.Play();
        }

        // Hit the side walls left when player 2 or AI shoot
        if (col.gameObject.tag == "SideWallLeft")
        {
            // To inverse the ball direction in player 2 or AI side direction
            otherBallGoBack = true;
            // To stop to calculate walls direction when the player 2 or AI scorer hit the ball 
            scorerOtherShoot = false;
            // To stop to calculate walls direction when the player 2 or AI hit the ball 
            otherShoot = false;
            // Stop to inverse the ball direction in player 1 side direction
            playerBallGoBack = false;
            // Calculate hit Factor
            float y = HitFactor(transform.position, col.transform.position, col.collider.bounds.size.y);
            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(1.0f, y).normalized;
            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
            // Play the ball sound
            ballSound.Play();
        }

        // Hit the player 1 score zone
        if (col.gameObject.tag == "GoalPlayer1")
        {
            // Add a new value to the score text of player 2 or AI
            Paddle.scoreOther += 1;
            // Player 2 or AI scoring
            scoring = true;
            Paddle.goal = true;
            // Play the goal sound
            goalSound.Play();
        }

        // Hit the player 2 or AI score zone
        if (col.gameObject.tag == "GoalOther")
        {
            // Add a new value to the score text of player 1
            Paddle.scorePlayer1 += 1;
            // Player 1 scoring
            scoring = true;
            Paddle.goal = true;
            // Play the goal sound
            goalSound.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        // Hit the player 1 back
        if (trig.gameObject.tag == "PlayerBack")
        {
            // Calculate hit Factor
            float y = HitFactor(transform.position, trig.transform.position, trig.GetComponent<Collider2D>().bounds.size.y);
            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(-1.0f, y).normalized;
            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
            player1Shoot = true;
            // Play the ball sound
            ballSound.Play();
        }

        // Hit the player 2 or AI back
        if (trig.gameObject.tag == "OtherBack")
        {
            // Calculate hit Factor
            float y = HitFactor(transform.position, trig.transform.position, trig.GetComponent<Collider2D>().bounds.size.y);
            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(1.0f, y).normalized;
            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
            otherShoot = true;
            // Play the ball sound
            ballSound.Play();
        }

        // Hit the goalkeeper power up when the player 1 shoot
        if (player1Shoot && trig.gameObject.tag == "GoalKeeperPU")
        {
            // To active the player 1 goalkeeper
            PowerUp.isActiveGK = true;
            GoalKeeperPU.activePlayer1GK = true;
            goalKeeperSound.Play();
        }

        // Hit the goalkeeper power up when the player 2 or the AI shoot
        if (otherShoot && trig.gameObject.tag == "GoalKeeperPU")
        {
            // To active the player 2 or the AI goalkeeper
            PowerUp.isActiveGK = true;
            GoalKeeperPU.activeOtherGK = true;
            goalKeeperSound.Play();
        }

        // Hit the scorer power up when the player 1 shoot
        if (player1Shoot && trig.gameObject.tag == "ScorerPU")
        {
            // To active the player 1 scorer
            PowerUp.isActiveScorer = true;
            ScorerPU.activePlayer1Scorer = true;
            scorerSound.Play();
        }

        // Hit the scorer power up when the player 2 or the AI shoot
        if (otherShoot && trig.gameObject.tag == "ScorerPU")
        {
            // To active the player 2 or the AI scorer
            PowerUp.isActiveScorer = true;
            ScorerPU.activeOtherScorer = true;
            scorerSound.Play();
        }

        // Hit the scorer power up when the player 1 shoot
        if (player1Shoot && trig.gameObject.tag == "FireBallPU")
        {
            // To active the player 1 fire ball
            PowerUp.isActiveFB = true;
            FireBallPU.activePlayer1FB = true;
        }

        // Hit the scorer power up when the player 1 shoot
        if (otherShoot && trig.gameObject.tag == "FireBallPU")
        {
            // To active the player 1 fire ball
            PowerUp.isActiveFB = true;
            FireBallPU.activeOtherFB = true;
        }

        // Hit the player 1 scorer power up
        if (trig.gameObject.tag == "ScorerPlayer1")
        {
            // Calculate hit Factor
            float y = HitFactor(transform.position, trig.transform.position, trig.GetComponent<Collider2D>().bounds.size.y);
            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(1.0f, y).normalized;
            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
            scorerPlayer1Shoot = true;
            // Play the ball sound
            ballSound.Play();
        }

        // Hit the player 2 or AI scorer power up
        if (trig.gameObject.tag == "ScorerOther")
        {
            // Calculate hit Factor
            float y = HitFactor(transform.position, trig.transform.position, trig.GetComponent<Collider2D>().bounds.size.y);
            // Calculate direction, make length=1 via .normalized
            Vector2 dir = new Vector2(-1.0f, y).normalized;
            // Set Velocity with dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
            scorerOtherShoot = true;
            // Play the ball sound
            ballSound.Play();
        }
    }
}
