using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerController : NetworkBehaviour
{
    // ____________________Public____________________________
    // Static bool
    public static bool scaleUpPlayer1, scaleDownPlayer1, scaleUpPlayer2, scaleDownPlayer2;
    // Float
    public float speed = 7.0f;

    // ____________________Private____________________________
    // GameObject
    private GameObject player1, player2;
    // Vector
    private Vector2 startPlayer1Pos, startPlayer2Pos;
    // Float
    private float newScale, initScale;

    void Start()
    {
        // The player play an online match
        Paddle.online = true;
        // Initial start postion for the player 1
        startPlayer1Pos = new Vector2(-6.0f, 0.0f);
        // Initial start postion for the player 2
        startPlayer2Pos = new Vector2(6.0f, 0.0f);
        // Set the new scale value
        newScale = 0.5f;
        // Set the new scale value
        initScale = 1.0f;

        // Check if is the network local player 1 who spawn 
        if (isLocalPlayer)
        {
            // Spawn the player 1 to his position
            transform.position = startPlayer1Pos;
            // Add a tag for the collisions
            gameObject.tag = "Player";
        }
        else
        {
            // Spawn the player 2 to his position
            transform.position = startPlayer2Pos;
            // Change the player 2 color
            GetComponent<SpriteRenderer>().color = Color.blue;
            // Add a tag for the collisions
            gameObject.tag = "Other";
        }
    }

    void Update ()
    {
        // Use for the collision and take the scale up or the scale down power up
        // Detect which paddle is the player 1
        player1 = GameObject.FindGameObjectWithTag("Player");
        // Detect which paddle is the player 2
        player2 = GameObject.FindGameObjectWithTag("Other");

        // Only the local player processes input.
        if (!isLocalPlayer)
        {
            return;
        }

        // When the player start an online match
        if (Paddle.startGame)
        {
            // Move the player
            PlayerInputs();
            // To scale the player 1 or the player 2
            ScalePaddle();
        }
    }

    // To move the player
    void PlayerInputs()
    {
        // Inputs for Android mobile
        // Player inpunts
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // To move player 
            // Follow a finger position
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, Input.GetTouch(0).deltaPosition.y);
        }

        // inputs for PC
        // Player inputs
        // Input to move to the up
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Move to the up
            transform.Translate(new Vector2(0.0f, speed * Time.deltaTime));
        }
        // Input to move to the down
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Move to the down
            transform.Translate(new Vector2(0.0f, -speed * Time.deltaTime));
        }
    }

    // Scale power up
    void ScalePaddle()
    {
        // When the player 1 pick the up scale power up 
        if (scaleUpPlayer1)
        {
            // Up the player 1 scale
            player1.transform.localScale = new Vector2(player1.transform.localScale.x, player1.transform.localScale.y + newScale);
            // To lunch the timer
            Paddle.scalePaddle = true;
            // Stop to scale
            scaleUpPlayer1 = false;
        }

        // When the player 1 pick the down scale power up 
        if (scaleDownPlayer1)
        {
            // Down the player 1 scale
            player1.transform.localScale = new Vector2(player1.transform.localScale.x, player1.transform.localScale.y - newScale);
            // To lunch the timer
            Paddle.scalePaddle = true;
            // Stop to scale
            scaleDownPlayer1 = false;
        }

        // When the player 2 pick the up scale power up 
        if (scaleUpPlayer2)
        {
            // UP the player 2 scale
            player2.transform.localScale = new Vector2(player2.transform.localScale.x, player2.transform.localScale.y + newScale);
            // To lunch the timer
            Paddle.scalePaddle = true;
            // Stop to scale
            scaleUpPlayer2 = false;
        }

        // When the player 2 pick the down scale power up 
        if (scaleDownPlayer2)
        {
            // Down the player 2 scale
            player2.transform.localScale = new Vector2(player2.transform.localScale.x, player2.transform.localScale.y - newScale);
            // To lunch the timer
            Paddle.scalePaddle = true;
            // Stop to scale
            scaleDownPlayer2 = false;
        }
    }
}
