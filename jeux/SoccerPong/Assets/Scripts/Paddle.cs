using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paddle : MonoBehaviour
{
    // ____________________Public ____________________________
    // Static int
    public static int scorePlayer1, scoreOther, maxScore;
    // Static bool
    public static bool shoot, goal, startGame; // Basic 
    public static bool scaleUpPlayer1, scaleDownPlayer1, scaleUpOther, scaleDownOther; // Scale power up
    public static bool online, scalePaddle; // Online multiplayer

    // Game objects
    public GameObject player1, other, ball;
    // Audio
    public AudioSource scaleSound;

    // ----Game UI objects----
    // Text
    public Text scorePlayer1Txt, scoreOtherTxt;
    // Image
    public Image goalImg;
    //------------------------

    // ----Menu UI objects----
    // Images
    public Image mainMenu, titlePart1, titlePart2; // Main menu
    public Image championship, championsLigue, worldCup; // Game modes selection
    //Buttons
    public Button soloBtn, multiBtn, quitBtn; // Main menu
    public Button localBtn, onlineBtn, playBtn, backToMainLvlBtn; // Multiplayer 
    public Button nextBtn, progressionBtn, backToMenuBtn, newModeBtn; // One player menu
    public Button championshipBtn, championsLigueBtn, worldCupBtn; // Game competitions selection
    public Button continueBtn, retryBtn, backBtn; // End of match
    public Button backToSoloMenu; // Progression menu
    // Text
    public Text player1WinsTxt, player2WinsTxt, youWinsTxt, youLoseTxt; // End of match
    public Text lockChampionsLigueTxt, lockWorldCupTxt; // Conditions to play the champions ligue or the world cup
    public Text championshipProgressTxt, championsLigueProgressTxt, worldCupProgressTxt, totalMatchTitleTxt, victoryTitleTxt; // Progression menu
    public Text remainingNumberTxt, RemainingWCTxt, championshipVictoryTxt, championsLigueVictoryTxt, worldCupVictoryTxt; // Value to change in progression menu 
    // Slider
    public Slider championBar;
    //------------------------

    // Float
    public float speed = 7.0f;

    // ____________________Private____________________________
    // Vector
    private Vector2 startPlayer1Pos, startOtherPos;
    // Float
    private float aiMoveTimer, goalTimer, scaleTmer; // Timer
    private float newScale, initScale; // Scale
    // Bool
    private bool multiplayer, ai, moveAI; // Basic
    private bool onChampionship, onChampionsLigue, onWorldCup, isChampions, isLooser; // One player competition
    // Int
    private int totalMatch, championshipVictory, championsLigueVictory, worldCupVictory, maxVictory;

	void Start ()
    {
        // Set the score maximum at 5 goal
        maxScore = 5;
        // Set the victory maximum to be the champion 
        maxVictory = 10;
        // Set the total match to play in game modes
        totalMatch = 15;
        // Set the new scale value
        newScale = 0.5f;
        // Set the new scale value
        initScale = 1.0f;
        // Wait that the player choose a game mode before to lunch the match.
        startGame = false;
        // Initial start postion for the player 1
        startPlayer1Pos = new Vector2(-6.0f, 0.0f);
        // Initial start postion for the player 2 or the AI
        startOtherPos = new Vector2(6.0f, 0.0f);
        // Spawn The player 1 and the player 2 or the AI to the great position
        player1.transform.position = startPlayer1Pos;
        other.transform.position = startOtherPos;
        // When player click on a button   
        soloBtn.onClick.AddListener(OnSoloButtonClick); // Call OnSoloButtonClick function to choose a game competition
        championshipBtn.onClick.AddListener(OnChampionshipButtonClick); // Call OnChampionshipButtonClick function to begin the championship
        championsLigueBtn.onClick.AddListener(OnChampionsLigueButtonClick); // Call OnChampionsLigueButtonClick function to begin the champions ligue
        worldCupBtn.onClick.AddListener(OnWorldCupButtonClick); // Call OnWorldCupButtonClick function to begin the world cup
        multiBtn.onClick.AddListener(OnMultiButtonClick); // Call OnMultiButtonClick function to go to the mutiplayer match selection
        localBtn.onClick.AddListener(OnLocalButtonClick); // Call OnLocalButtonClick function to play with a friend
        onlineBtn.onClick.AddListener(OnOnlineButtonClick); // Call OnOnlineButtonClick function to play against someone
        quitBtn.onClick.AddListener(OnQuitButtonClick); // Call OnQuitButtonClick to quit and close the game
        retryBtn.onClick.AddListener(OnRetryButtonClick); // Call OnRetryButtonClick to retry the match
        nextBtn.onClick.AddListener(OnNextButtonClick); // Call OnNextButtonClick to play the next match
        backBtn.onClick.AddListener(OnBackButtonClick); // Call OnBackButtonClick to go back to the main menu
        backToMenuBtn.onClick.AddListener(OnBackToMenuButtonClick); // Call OnBackToMenuButtonClick to go back to the main menu
        continueBtn.onClick.AddListener(OnContinueButtonClick); // Call OnContinueButtonClick to go back to the solo main menu
        progressionBtn.onClick.AddListener(OnProgressionButtonClick); // Call OnProgressionButtonClick to go to the progression menu
        backToSoloMenu.onClick.AddListener(OnBackToSoloMenuButtonClick); // Call OnBackToSoloMenuButtonClick to go back to the solo main menu
        newModeBtn.onClick.AddListener(OnNewModeButtonClick); // Call OnNewModeButtonClick to choose a new game competition
        playBtn.onClick.AddListener(OnPlayButtonClick); // Call OnPlayButtonClick to start a match against someone
        backToMainLvlBtn.onClick.AddListener(OnBackToMainLvlButtonClick); // Call OnBackButtonClick to go back to the main level
    }
	
	void Update ()
    {
        // To update the player 1 victory in championship
        UpdateChampionshipVictory();
        // To update the player 1 victory in champions ligue
        UpdateChampionsLigueVictory();
        // To update the player 1 victory in world cup
        UpdateWorldCupVictory();

        // When the match begin.
        if (startGame)
        {
            // To update the player 1 score 
            UpdateScorePlayer1();
            // To update the player 2 or AI score
            UpdateScoreOther();
            // To update the player 1 basic remaining match
            UpdateRemainingMatch();
            // To update the player 1 world cup remaining match
            UpdateRemainingWCMatch();
            // To call all player(s) inputs
            PlayerInputs();
            // To scale the player 1, the player 2 or the AI
            ScalePaddle();

            // When player choose to play against an AI
            if (ai)
            {
                // To call all AI mechanics
                AI();
            }

            // When someone scoring
            if (goal)
            {
                // Show the goal image
                goalImg.gameObject.SetActive(true);
                // Up the timer value
                goalTimer += Time.deltaTime;
            }

            // After 2 seconds
            if (goalTimer >= 2.0f)
            {
                // Hide the goal image
                goalImg.gameObject.SetActive(false);
                // Reset the timer at 0
                goalTimer = 0.0f;
                // Wait an other goal
                goal = false;
            }
        }

        // When the player 1, the player 2 or the AI pick a scale power up
        if (scalePaddle)
        {
            // Lunch the timer
            scaleTmer += Time.deltaTime;
        }

        // After 10 seconds
        if (scaleTmer >= 10.0f)
        {
            // Reset the player 1 scale
            player1.transform.localScale = new Vector2(player1.transform.localScale.x, initScale);
            // Reset the player 2 or the AI scale
            other.transform.localScale = new Vector2(other.transform.localScale.x, initScale);
            // Reset the timer
            scaleTmer = 0.0f;
            // Don't lunch the timer again
            scalePaddle = false;
        }

        // When the match is finished
        if (!startGame)
        {          
            // Set the player 1 to the start postion
            player1.transform.position = startPlayer1Pos;
            // Set the player 2 or the AI to the start postion
            other.transform.position = startOtherPos;        
        }

        // When the player 1 win the match
        if (scorePlayer1 >= maxScore)
        {            
            // Stop the player 1 and the player 2 or the AI mechanics
            startGame = false;
            // Stop to engage the ball
            Ball.startEngage = false;
            // Reset goal boolean
            goal = false;
            // Hide the goal image
            goalImg.gameObject.SetActive(false);
            // Reset the spawn power up mechanics
            GoalKeeperPU.spawnGK = false;
            ScalePU.canSpawn = false;
            DownScalePU.canSpawn = false;
            ScorerPU.spawnScorer = false;
            PowerUp.isActiveGK = true;
            PowerUp.isActiveScorer = true;

            // When the player play in a championship
            if (onChampionship)
            {
                // Add a victory in championship
                championshipVictory += 1;
                // Up the slider
                championBar.value += 0.1f;
            }

            // When the player play in a champions ligue
            if (onChampionsLigue)
            {
                // Add a victory in champions ligue
                championsLigueVictory += 1;
                // Up the slider
                championBar.value += 0.1f;
            }

            // When the player play in a world cup
            if (onWorldCup)
            {
                // Add a victory in world cup
                worldCupVictory += 1;
                // Up the slider
                championBar.value += 0.1f;
            }

            // When the player win against an AI
            if (ai)
            {   
                // Show the win text            
                youWinsTxt.gameObject.SetActive(true);
                
                // Open the menu
                mainMenu.gameObject.SetActive(true); // Background
                continueBtn.gameObject.SetActive(true); // Continue button             
            }
            // When the player win against a friend 
            else
            {
                // Show the win text             
                player1WinsTxt.gameObject.SetActive(true);
                // Open the menu
                mainMenu.gameObject.SetActive(true); // Background
                retryBtn.gameObject.SetActive(true); // Retry button
                backBtn.gameObject.SetActive(true); // Back button 
            }

            // Restet the player score at 0
            scorePlayer1 = 0;
        }

        // When the player 2 or AI win the match
        if (scoreOther >= maxScore)
        {
            // Stop the player 1 and the player 2 or the AI mechanics
            startGame = false;
            // Stop to engage the ball
            Ball.startEngage = false;
            // Reset goal boolean
            goal = false;
            // Hide the goal image
            goalImg.gameObject.SetActive(false);
            // Reset the spawn power up mechanics
            GoalKeeperPU.spawnGK = false;
            ScalePU.canSpawn = false;
            DownScalePU.canSpawn = false;
            ScorerPU.spawnScorer = false;
            FireBallPU.spawnFB = false;
            PowerUp.isActiveGK = true;
            PowerUp.isActiveScorer = true;
            PowerUp.isActiveFB = true;           

            // When the player win against an AI
            if (ai) 
            {
                // Show the win text
                youLoseTxt.gameObject.SetActive(true);
                // Open the menu
                mainMenu.gameObject.SetActive(true); // Background
                continueBtn.gameObject.SetActive(true); // Continue button
            }
            // When the player win against a friend 
            else
            {
                // Show the win text
                player2WinsTxt.gameObject.SetActive(true);
                // Open the menu
                mainMenu.gameObject.SetActive(true); // Background
                retryBtn.gameObject.SetActive(true); // Retry button
                backBtn.gameObject.SetActive(true); // Back button
            }
        }

        // When the player win a competition
        if (championshipVictory >= maxVictory || championsLigueVictory >= maxVictory || worldCupVictory >= maxVictory)
        {
            // The player is the champions
            isChampions = true;
        }

        // When the player play all his matches in a competition
        if (totalMatch <= 0)
        {
            // The player loose the competion
            isLooser = true;
        }

        // When the player win the champions ligue
        if(onChampionsLigue && isChampions)
        {
            // Restore the championship victory 
            championshipVictory = maxVictory;
        }

        // When the player win the world cup
        if (onWorldCup && isChampions)
        {
            // Restore the championship and the champions ligue victory 
            championshipVictory = maxVictory;
            championsLigueVictory = maxVictory;
        }
    }

    void PlayerInputs()
    {
        // Inputs for Android mobile
        // Player 1 inpunts
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // To move player 1
            // Follow a finger position
            player1.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, Input.GetTouch(0).deltaPosition.y);
        }

        // When player choose to play against a friend
        // Player 2 inputs
        if (multiplayer && !online)
        {
            if (Input.touchCount >= 2 && Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                // To move player 2
                // Follow a second finger position
                other.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, Input.GetTouch(1).deltaPosition.y);
            }
        }

        // inputs for PC
        // Player 1 inputs
        // Input to move to the up
        if (Input.GetKey(KeyCode.Z))
        {
            // Move to the up
            player1.transform.Translate(new Vector2(0.0f, speed * Time.deltaTime));
        }
       
        // Input to move to the down
        if (Input.GetKey(KeyCode.S))
        {
            // Move to the down
            player1.transform.Translate(new Vector2(0.0f, -speed * Time.deltaTime));
        }

        //// When player choose to play against a friend
        //// Player 2 inputs
        if (multiplayer && !online)
        {
            // Input to move to the up
            if (Input.GetKey(KeyCode.UpArrow))
            {
                // Move to the up
                other.transform.Translate(new Vector2(0.0f, speed * Time.deltaTime));
            }
            // Input to move to the down
            if (Input.GetKey(KeyCode.DownArrow))
            {
                // Move to the down
                other.transform.Translate(new Vector2(0.0f, -speed * Time.deltaTime));
            }
        }
    }

    // AI mechanics
    void AI()
    {
        // Up the timer value
        aiMoveTimer += Time.deltaTime;

        // After 0.5 second 
        if (aiMoveTimer >= 0.5f)
        {
            // Move ai random to the up or to the right  
            MoveAIRandom();
            // Reset the timer at 0
            aiMoveTimer = 0.0f;
        }

        // Move to the ball
        if (shoot)
        {
            // Stop to move random the AI when player shoot to move to the ball position
            moveAI = false;
            // To calculate AI position with ball position
            if (ball.transform.position.y > other.transform.position.y)
            {
                // Move to the up
                other.GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
            }
            // To calculate AI position with ball position
            if (ball.transform.position.y < other.transform.position.y)
            {
                // Move to the down
                other.GetComponent<Rigidbody2D>().velocity = Vector2.down * speed;
            }
        }
    }
     
    // To move ai random to the up or to the right 
    void MoveAIRandom()
    {
        // To move random the AI
        moveAI = true;

        float rand = Random.Range(0, 2);
        if (moveAI && rand < 1)
        {
            // Move to the up
            other.GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
            // Stop to move random the AI
            moveAI = false;
        }
        else
        {
            // Move to the down
            other.GetComponent<Rigidbody2D>().velocity = Vector2.down * speed;
            // Stop to move random the AI
            moveAI = false;
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
            scalePaddle = true;
            // Stop to scale
            scaleUpPlayer1 = false;
            // Play the scale sound
            scaleSound.Play();
        }

        // When the player 1 pick the down scale power up 
        if (scaleDownPlayer1)
        {
            // Down the player 1 scale
            player1.transform.localScale = new Vector2(player1.transform.localScale.x, player1.transform.localScale.y - newScale);
            // To lunch the timer
            scalePaddle = true;
            // Stop to scale
            scaleDownPlayer1 = false;
            // Play the scale sound
            scaleSound.Play();
        }

        // When the player 2 or the AI pick the up scale power up 
        if (scaleUpOther)
        {
            // UP the player 2 or the AI scale
            other.transform.localScale = new Vector2(other.transform.localScale.x, other.transform.localScale.y + newScale);
            // To lunch the timer
            scalePaddle = true;
            // Stop to scale
            scaleUpOther = false;
            // Play the scale sound
            scaleSound.Play();
        }

        // When the player 2 or the AI pick the down scale power up 
        if (scaleDownOther)
        {
            // Down the player 2 or the AI scale
            other.transform.localScale = new Vector2(other.transform.localScale.x, other.transform.localScale.y - newScale);
            // To lunch the timer
            scalePaddle = true;
            // Stop to scale
            scaleDownOther = false;
            // Play the scale sound
            scaleSound.Play();
        }
    }

    // To add new value to the player 1 score text
    public void AddScorePlayer1(int newScoreValue)
    {
        // Add new value to the player 1 score text
        scorePlayer1 += newScoreValue;
        // Update it
        UpdateScorePlayer1();
    }

    // To update the player 1 score
    void UpdateScorePlayer1()
    {
        // To make modification to the player 1 score text
        scorePlayer1Txt.text = "" + scorePlayer1;
    }

    // To add new value to the player 2 or AI score text
    public void AddScoreOther(int newScoreValue)
    {
        // Add new value to the player 2 or AI score text
        scoreOther += newScoreValue;
        // Update it
        UpdateScoreOther();
    }

    // To update the player 2 or AI score
    void UpdateScoreOther()
    {
        // To make modification to the player 2 or AI score text
        scoreOtherTxt.text = "" + scoreOther;
    }

    // To add new value to the player 1 victory text in championship
    public void AddChampionshipVictory(int newChampionshipVictoryValue)
    {
        // Add new value to the player 1 victory text in championship
        championshipVictory += newChampionshipVictoryValue;
        // Update it
        UpdateChampionshipVictory();
    }

    // To update the player 1 victory in championship
    void UpdateChampionshipVictory()
    {
        // To make modification to the player 1 victory text in championship
        championshipVictoryTxt.text = "" + championshipVictory;
    }

    // To add new value to the player 1 victory text in champions ligue
    public void AddChampionsLigueVictory(int newChampionsLigueVictoryValue)
    {
        // Add new value to the player 1 victory text in champions ligue
        championsLigueVictory += newChampionsLigueVictoryValue;
        // Update it
        UpdateChampionsLigueVictory();
    }

    // To update the player 1 victory in champions ligue
    void UpdateChampionsLigueVictory()
    {
        // To make modification to the player 1 victory text in champions ligue
        championsLigueVictoryTxt.text = "" + championsLigueVictory;
    }

    // To add new value to the player 1 victory text in world cup
    public void AddWorldCupVictory(int newWorldCupVictoryValue)
    {
        // Add new value to the player 1 victory text in world cup
        worldCupVictory += newWorldCupVictoryValue;
        // Update it
        UpdateWorldCupVictory();
    }

    // To update the player 1 victory in world cup
    void UpdateWorldCupVictory()
    {
        // To make modification to the player 1 victory text in world cup
        worldCupVictoryTxt.text = "" + worldCupVictory;
    }

    // To add new value to the basic remaining match value
    public void AddRemainingMatch(int newRemainingMatchValue)
    {
        // Add new value to the basic remaining match value
        totalMatch += newRemainingMatchValue;
        // Update it
        UpdateRemainingMatch();
    }

    // To update the basic remaining match value
    void UpdateRemainingMatch()
    {
        // To make modification to the basic remaining match value
        remainingNumberTxt.text = "" + totalMatch;
    }

    // To add new value to the world cup remaining match value
    public void AddRemainingWCMatch(int newRemainingWCMatchValue)
    {
        // Add new value to the world cup remaining match value
        totalMatch += newRemainingWCMatchValue;
        // Update it
        UpdateRemainingWCMatch();
    }

    // To update the world cup remaining match value
    void UpdateRemainingWCMatch()
    {
        // To make modification to the world cup remaining match value
        RemainingWCTxt.text = "" + totalMatch;
    }

    // To choose a game competition
    void OnSoloButtonClick()
    {      
        // Hide main menu buttons
        soloBtn.gameObject.SetActive(false);
        multiBtn.gameObject.SetActive(false);
        quitBtn.gameObject.SetActive(false);
        // Show game competitions elements
        championship.gameObject.SetActive(true);
        championshipBtn.gameObject.SetActive(true);
        championsLigue.gameObject.SetActive(true);
        worldCup.gameObject.SetActive(true);

        // When the player win the championship
        if (championshipVictory >= maxVictory)
        {
            // Unlock the champions ligue 
            championsLigueBtn.gameObject.SetActive(true);
            lockChampionsLigueTxt.gameObject.SetActive(false);
        }
        else
        {
            // Else keep the champions ligue locked
            lockChampionsLigueTxt.gameObject.SetActive(true);
            championsLigueBtn.gameObject.SetActive(false);
        }

        // When the player win the champions ligue
        if (championsLigueVictory >= maxVictory)
        {
            // Unlock the world cup 
            worldCupBtn.gameObject.SetActive(true);
            lockWorldCupTxt.gameObject.SetActive(false);
        }
        else
        {
            // Else keep the world cup locked
            lockWorldCupTxt.gameObject.SetActive(true);
            worldCupBtn.gameObject.SetActive(false);
        }
    }

    // To begin a championship
    void OnChampionshipButtonClick()
    {
        // The match can started 
        startGame = true;
        // The player begin a championship
        onChampionship = true;
        // Engage the ball 
        Ball.startEngage = true;
        // To lunch the timer to spawn the goalkeeper power up 
        GoalKeeperPU.spawnGK = true;
        // To lunch the timer to spawn the scale power up 
        ScalePU.canSpawn = true;
        // To lunch the timer to spawn the down scale power up 
        DownScalePU.canSpawn = true;
        // To lunch the timer to spawn the scorer power up
        ScorerPU.spawnScorer = true;
        // To lunch the timer to spawn the fire ball power up
        FireBallPU.spawnFB = true;
        // The player play against an AI
        ai = true;
        // Reset the player 1 score at 0
        scorePlayer1 = 0;
        // Reset the player 2 or the AI score at 0
        scoreOther = 0;
        // Reset the player victory in championship
        championshipVictory = 0;
        // Reset the player victory in champions ligue
        championsLigueVictory = 0;
        // Reset the player victory in world cup
        worldCupVictory = 0;
        // Reset the slider
        championBar.value += 0.0f;
        // Reset the champions and looser boolean
        isChampions = false;
        isLooser = false;
        // Reset the champions ligue and world cup boolean
        onChampionsLigue = false;
        onWorldCup = false;
        // Reset the remaining match 
        totalMatch = 15;
        // Substarct a match to the total match of the championship
        totalMatch -= 1;
        // Hide all game competitions elements
        mainMenu.gameObject.SetActive(false);
        titlePart1.gameObject.SetActive(false);
        titlePart2.gameObject.SetActive(false);
        championship.gameObject.SetActive(false);
        championshipBtn.gameObject.SetActive(false);
        championsLigue.gameObject.SetActive(false);
        championsLigueBtn.gameObject.SetActive(false);
        lockChampionsLigueTxt.gameObject.SetActive(false);
        worldCup.gameObject.SetActive(false);
        worldCupBtn.gameObject.SetActive(false);
        lockWorldCupTxt.gameObject.SetActive(false);
    }

    // To begin a champions ligue
    void OnChampionsLigueButtonClick()
    {
        // The match can started 
        startGame = true;
        // The player begin a champions ligue
        onChampionsLigue = true;
        // Engage the ball 
        Ball.startEngage = true;
        // To lunch the timer to spawn the goalkeeper power up 
        GoalKeeperPU.spawnGK = true;
        // To lunch the timer to spawn the scale power up 
        ScalePU.canSpawn = true;
        // To lunch the timer to spawn the down scale power up 
        DownScalePU.canSpawn = true;
        // To lunch the timer to spawn the scorer power up
        ScorerPU.spawnScorer = true;
        // To lunch the timer to spawn the fire ball power up
        FireBallPU.spawnFB = true;
        // The player play against an AI
        ai = true;
        // Reset the player 1 score at 0
        scorePlayer1 = 0;
        // Reset the player 2 or the AI score at 0
        scoreOther = 0;
        // Reset the player victory in championship
        championshipVictory = 0;
        // Reset the player victory in champions ligue
        championsLigueVictory = 0;
        // Reset the player victory in world cup
        worldCupVictory = 0;
        // Reset the slider
        championBar.value += 0.0f;
        // Reset the champions and looser boolean
        isChampions = false;
        isLooser = false;
        // Reset the championship and world cup boolean
        onChampionship = false;
        onWorldCup = false;
        // Reset the remaining match 
        totalMatch = 15;
        // Substarct a match to the total match of the championship
        totalMatch -= 1;
        // Hide all game competitions elements
        mainMenu.gameObject.SetActive(false);
        titlePart1.gameObject.SetActive(false);
        titlePart2.gameObject.SetActive(false);
        championship.gameObject.SetActive(false);
        championshipBtn.gameObject.SetActive(false);
        championsLigue.gameObject.SetActive(false);
        championsLigueBtn.gameObject.SetActive(false);
        lockChampionsLigueTxt.gameObject.SetActive(false);
        worldCup.gameObject.SetActive(false);
        worldCupBtn.gameObject.SetActive(false);
        lockWorldCupTxt.gameObject.SetActive(false);
    }

    // To begin a champions ligue
    void OnWorldCupButtonClick()
    {
        // The match can started 
        startGame = true;
        // The player begin a world cup
        onWorldCup = true;
        // Engage the ball 
        Ball.startEngage = true;
        // To lunch the timer to spawn the goalkeeper power up 
        GoalKeeperPU.spawnGK = true;
        // To lunch the timer to spawn the scale power up 
        ScalePU.canSpawn = true;
        // To lunch the timer to spawn the down scale power up 
        DownScalePU.canSpawn = true;
        // To lunch the timer to spawn the scorer power up
        ScorerPU.spawnScorer = true;
        // To lunch the timer to spawn the fire ball power up
        FireBallPU.spawnFB = true;
        // The player play against an AI
        ai = true;
        // Reset the player 1 score at 0
        scorePlayer1 = 0;
        // Reset the player 2 or the AI score at 0
        scoreOther = 0;
        // Reset the player victory in championship
        championshipVictory = 0;
        // Reset the player victory in champions ligue
        championsLigueVictory = 0;
        // Reset the player victory in world cup
        worldCupVictory = 0;
        // Reset the slider
        championBar.value += 0.0f;
        // Reset the champions and looser boolean
        isChampions = false;
        isLooser = false;
        // Reset the championship and champions ligue boolean
        onChampionship = false;
        onChampionsLigue = false;      
        // Set the remaining match at 10 
        totalMatch = 10;
        // Substarct a match to the total match of the championship
        totalMatch -= 1;
        // Hide all game competitions elements
        mainMenu.gameObject.SetActive(false);
        titlePart1.gameObject.SetActive(false);
        titlePart2.gameObject.SetActive(false);
        championship.gameObject.SetActive(false);
        championshipBtn.gameObject.SetActive(false);
        championsLigue.gameObject.SetActive(false);
        championsLigueBtn.gameObject.SetActive(false);
        lockChampionsLigueTxt.gameObject.SetActive(false);
        worldCup.gameObject.SetActive(false);
        worldCupBtn.gameObject.SetActive(false);
        lockWorldCupTxt.gameObject.SetActive(false);
    }

    // To begin a match against a friend
    void OnMultiButtonClick()
    {
        // Hide all main menu buttons
        soloBtn.gameObject.SetActive(false);
        multiBtn.gameObject.SetActive(false);
        quitBtn.gameObject.SetActive(false);
        // Show the multiplayer menu buttons
        localBtn.gameObject.SetActive(true);
        onlineBtn.gameObject.SetActive(true);
    }

    // To begin a match against a friend in the same screen
    void OnLocalButtonClick()
    {
        // The match can started 
        startGame = true;
        // Engage the ball 
        Ball.startEngage = true;
        // To lunch the timer to spawn the goalkeeper power up 
        GoalKeeperPU.spawnGK = true;
        // To lunch the timer to spawn the scale power up 
        ScalePU.canSpawn = true;
        // To lunch the timer to spawn the down scale power up 
        DownScalePU.canSpawn = true;
        // To lunch the timer to spawn the scorer power up
        ScorerPU.spawnScorer = true;
        // To lunch the timer to spawn the fire ball power up
        FireBallPU.spawnFB = true;
        // The player play against a friend
        multiplayer = true;
        // Reset the player 1 score at 0
        scorePlayer1 = 0;
        // Reset the player 2 or the AI score at 0
        scoreOther = 0;
        // Hide all main menu elements
        mainMenu.gameObject.SetActive(false);
        titlePart1.gameObject.SetActive(false);
        titlePart2.gameObject.SetActive(false);
        localBtn.gameObject.SetActive(false);
        onlineBtn.gameObject.SetActive(false);
    }

    // To play an online match against someone
    void OnOnlineButtonClick()
    {    
        // Active the online inputs
        online = true;
        // Load the online level
        Application.LoadLevel("NetworkLvl");
    }

    // To retry the match 
    void OnRetryButtonClick()
    {
        // The match can started 
        startGame = true;
        // Engage the ball 
        Ball.startEngage = true;
        // To lunch the timer to spawn the goalkeeper power up 
        GoalKeeperPU.spawnGK = true;
        // To lunch the timer to spawn the scale power up 
        ScalePU.canSpawn = true;
        // To lunch the timer to spawn the down scale power up 
        DownScalePU.canSpawn = true;
        // To lunch the timer to spawn the scorer power up
        ScorerPU.spawnScorer = true;
        // To lunch the timer to spawn the fire ball power up
        FireBallPU.spawnFB = true;
        // Reset the power up position
        PowerUp.isActiveGK = false;
        PowerUp.isActiveScorer = false;
        PowerUp.isActiveFB = false;
        // Reset the player 1 score at 0
        scorePlayer1 = 0;
        // Reset the player 2 or the AI score at 0
        scoreOther = 0;
        // Hide the goal image
        goalImg.gameObject.SetActive(false);
        // Hide all end menu elements
        mainMenu.gameObject.SetActive(false);
        player1WinsTxt.gameObject.SetActive(false);
        player2WinsTxt.gameObject.SetActive(false);
        youWinsTxt.gameObject.SetActive(false);
        youLoseTxt.gameObject.SetActive(false);
        retryBtn.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(false);
    }

    // To play the next match 
    void OnNextButtonClick()
    {
        // The match can started 
        startGame = true;
        // Engage the ball 
        Ball.startEngage = true;
        // To lunch the timer to spawn the goalkeeper power up 
        GoalKeeperPU.spawnGK = true;
        // To lunch the timer to spawn the scale power up 
        ScalePU.canSpawn = true;
        // To lunch the timer to spawn the down scale power up 
        DownScalePU.canSpawn = true;
        // To lunch the timer to spawn the scorer power up
        ScorerPU.spawnScorer = true;
        // To lunch the timer to spawn the fire ball power up
        FireBallPU.spawnFB = true;
        // Reset the power up position
        PowerUp.isActiveGK = false;
        PowerUp.isActiveScorer = false;
        PowerUp.isActiveFB = false;
        // Reset the player 1 score at 0
        scorePlayer1 = 0;
        // Reset the player 2 or the AI score at 0
        scoreOther = 0;
        // Substarct a match to the total match of the championship
        totalMatch -= 1;    
        // Hide all end menu elements
        mainMenu.gameObject.SetActive(false);
        titlePart1.gameObject.SetActive(false);
        titlePart2.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(false);
        progressionBtn.gameObject.SetActive(false);
        backToMenuBtn.gameObject.SetActive(false);
    }

    // To go back to the main menu
    void OnBackButtonClick()
    {
        // Reset previously game data 
        multiplayer = false;       
        online = false;
        // Reset the player 1 score at 0
        scorePlayer1 = 0;
        // Reset the player 2 or the AI score at 0
        scoreOther = 0;
        // Reset the power up position
        PowerUp.isActiveGK = false;
        PowerUp.isActiveScorer = false;
        PowerUp.isActiveFB = false;
        // Show all main menu elements
        titlePart1.gameObject.SetActive(true);
        titlePart2.gameObject.SetActive(true);
        soloBtn.gameObject.SetActive(true);
        multiBtn.gameObject.SetActive(true);
        quitBtn.gameObject.SetActive(true);
        // Hide the goal image
        goalImg.gameObject.SetActive(false);
        // Hide all end menu elements
        player1WinsTxt.gameObject.SetActive(false);
        player2WinsTxt.gameObject.SetActive(false);
        youWinsTxt.gameObject.SetActive(false);
        youLoseTxt.gameObject.SetActive(false);
        retryBtn.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(false);
    }

    // To go back to the main menu
    void OnBackToMenuButtonClick()
    {
        // Reset previously game data 
        multiplayer = false;
        ai = false;
        // Reset the player 1 score at 0
        scorePlayer1 = 0;
        // Reset the player 2 or the AI score at 0
        scoreOther = 0;
        // Reset the power up position
        PowerUp.isActiveGK = false;
        PowerUp.isActiveScorer = false;
        PowerUp.isActiveFB = false;
        // Show all main menu elements
        titlePart1.gameObject.SetActive(true);
        titlePart2.gameObject.SetActive(true);
        soloBtn.gameObject.SetActive(true);
        multiBtn.gameObject.SetActive(true);
        quitBtn.gameObject.SetActive(true);
        // Hide all solo menu elements
        nextBtn.gameObject.SetActive(false);
        progressionBtn.gameObject.SetActive(false);
        backToMenuBtn.gameObject.SetActive(false);
    }

    // To go back to the solo main menu
    void OnContinueButtonClick()
    {
        // Stop the player 1 and the player 2 or the AI mechanics
        startGame = false;
        // Stop to engage the ball
        Ball.startEngage = false;
        // Reset the player 1 score at 0
        scorePlayer1 = 0;
        // Reset the player 2 or the AI score at 0
        scoreOther = 0;
        // Reset the spawn power up mechanics
        GoalKeeperPU.spawnGK = false;
        ScalePU.canSpawn = false;
        DownScalePU.canSpawn = false;
        ScorerPU.spawnScorer = false;
        FireBallPU.spawnFB = false;
        PowerUp.isActiveGK = true;
        PowerUp.isActiveScorer = true;
        PowerUp.isActiveFB = true;
        // Show all solo main menu elements
        titlePart1.gameObject.SetActive(true);
        titlePart2.gameObject.SetActive(true);
        nextBtn.gameObject.SetActive(true);
        progressionBtn.gameObject.SetActive(true);
        backToMenuBtn.gameObject.SetActive(true);
        // Hide the goal image
        goalImg.gameObject.SetActive(false);
        // Hide all end menu elements
        youWinsTxt.gameObject.SetActive(false);
        youLoseTxt.gameObject.SetActive(false);
        continueBtn.gameObject.SetActive(false);

        // When the player win or loose a competition
        if (isChampions || isLooser)
        {
            // Show the new mode button
            newModeBtn.gameObject.SetActive(true);
            // Hide the next button
            nextBtn.gameObject.SetActive(false);
        }
        // when player is on comptetion or start a new competition
        else
        {
            // Show the next button
            nextBtn.gameObject.SetActive(true);
            // Hide the new mode button
            newModeBtn.gameObject.SetActive(false);
        }
    }

    // To go back to game competitions menu
    void OnNewModeButtonClick()
    {
        // Hide main menu buttons
        newModeBtn.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(false);
        progressionBtn.gameObject.SetActive(false);
        backToMenuBtn.gameObject.SetActive(false);
        // Show game competitions elements
        championship.gameObject.SetActive(true);
        championshipBtn.gameObject.SetActive(true);
        championsLigue.gameObject.SetActive(true);
        worldCup.gameObject.SetActive(true);

        // When the player win the championship
        if (championshipVictory >= maxVictory)
        {
            // Unlock the champions ligue 
            championsLigueBtn.gameObject.SetActive(true);
            lockChampionsLigueTxt.gameObject.SetActive(false);
        }
        else
        {
            // Else keep the champions ligue locked
            lockChampionsLigueTxt.gameObject.SetActive(true);
            championsLigueBtn.gameObject.SetActive(false);
        }

        // When the player win the champions ligue
        if (championsLigueVictory >= maxVictory)
        {
            // Unlock the world cup 
            worldCupBtn.gameObject.SetActive(true);
            lockWorldCupTxt.gameObject.SetActive(false);
        }
        else
        {
            // Else keep the world cup locked
            lockWorldCupTxt.gameObject.SetActive(true);
            worldCupBtn.gameObject.SetActive(false);
        }
    }

    // To see the player progression in championship, champions ligue or world cup
    void OnProgressionButtonClick()
    {
        // Hide solo main menu buttons     
        nextBtn.gameObject.SetActive(false);
        newModeBtn.gameObject.SetActive(false);
        progressionBtn.gameObject.SetActive(false);
        backToMenuBtn.gameObject.SetActive(false);
        // Show all progression menu elements
        totalMatchTitleTxt.gameObject.SetActive(true);
        victoryTitleTxt.gameObject.SetActive(true);
        backToSoloMenu.gameObject.SetActive(true);
        championBar.gameObject.SetActive(true);

        // When the player play a championship
        if (onChampionship)
        {
            // Show the championship elements
            championshipProgressTxt.gameObject.SetActive(true);
            remainingNumberTxt.gameObject.SetActive(true);
            championshipVictoryTxt.gameObject.SetActive(true);
            // Hide the others titles 
            championsLigueProgressTxt.gameObject.SetActive(false);
            worldCupProgressTxt.gameObject.SetActive(false);
        }

        // When the player play a champions ligue
        if (onChampionsLigue)
        {
            // Show the champions ligue elements
            championsLigueProgressTxt.gameObject.SetActive(true);
            remainingNumberTxt.gameObject.SetActive(true);
            championsLigueVictoryTxt.gameObject.SetActive(true);
            // Hide the others titles 
            championshipProgressTxt.gameObject.SetActive(false);
            worldCupProgressTxt.gameObject.SetActive(false);
        }

        // When the player play a world cup
        if (onWorldCup)
        {
            // Show the specific world cup elements 
            worldCupProgressTxt.gameObject.SetActive(true);
            RemainingWCTxt.gameObject.SetActive(true);
            worldCupVictoryTxt.gameObject.SetActive(true);
            // Hide the others elements
            remainingNumberTxt.gameObject.SetActive(false);
            championshipProgressTxt.gameObject.SetActive(false);
            championsLigueProgressTxt.gameObject.SetActive(false);
        }
    }

    // To go back to the solo main menu
    void OnBackToSoloMenuButtonClick()
    {
        // Show solo main menu buttons     
        nextBtn.gameObject.SetActive(true);
        progressionBtn.gameObject.SetActive(true);
        backToMenuBtn.gameObject.SetActive(true);
        // Hide all progression menu elements
        championshipProgressTxt.gameObject.SetActive(false);
        championsLigueProgressTxt.gameObject.SetActive(false);
        worldCupProgressTxt.gameObject.SetActive(false);
        totalMatchTitleTxt.gameObject.SetActive(false);
        victoryTitleTxt.gameObject.SetActive(false);
        championshipVictoryTxt.gameObject.SetActive(false);
        championsLigueVictoryTxt.gameObject.SetActive(false);
        worldCupVictoryTxt.gameObject.SetActive(false);
        backToSoloMenu.gameObject.SetActive(false);
        remainingNumberTxt.gameObject.SetActive(false);
        RemainingWCTxt.gameObject.SetActive(false);
        championBar.gameObject.SetActive(false);

        // When the player win or loose a competition
        if (isChampions || isLooser)
        {
            // Show the new mode button
            newModeBtn.gameObject.SetActive(true);
            // Hide the next button
            nextBtn.gameObject.SetActive(false);
        }
        // when player is on comptetion or start a new competition
        else
        {
            // Show the next button
            nextBtn.gameObject.SetActive(true);
            // Hide the new mode button
            newModeBtn.gameObject.SetActive(false);
        }
    }

    // To start an online match
    void OnPlayButtonClick()
    {
        // The match can started 
        startGame = true;
        // Lunch an online match
        online = true; 
        // Engage the ball 
        Ball.startEngage = true;
        // To lunch the timer to spawn the goalkeeper power up 
        GoalKeeperPU.spawnGK = true;
        // To lunch the timer to spawn the scale power up 
        ScalePU.canSpawn = true;
        // To lunch the timer to spawn the down scale power up 
        DownScalePU.canSpawn = true;
        // To lunch the timer to spawn the scorer power up
        ScorerPU.spawnScorer = true;
        // To lunch the timer to spawn the fire ball power up
        FireBallPU.spawnFB = true;
        // Reset the player 1 score at 0
        scorePlayer1 = 0;
        // Reset the player 2 or the AI score at 0
        scoreOther = 0;
        // Hide all the online menu elements
        playBtn.gameObject.SetActive(false);
        backToMainLvlBtn.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(false);
        titlePart1.gameObject.SetActive(false);
        titlePart2.gameObject.SetActive(false);
    }

    // To go back to the main menu
    void OnBackToMainLvlButtonClick()
    {
        // Load the main level
        Application.LoadLevel("PongLvl");
    }

    // To quit the game
    void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
