using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    //making a static variable for GameMaster
    public static GameMaster gm { get; private set; }

    [SerializeField]
    private GameObject gameOverUi;
    [SerializeField] 
    private GameObject pauseUi;
    [SerializeField] 
    private GameObject gameEndUi;


    [SerializeField]
    private bool enableDevMode = false;

    [SerializeField]
    private Transform playerPrefab;

    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI pauseTimerText;
    [SerializeField]
    private TextMeshProUGUI deadTimerText;
    [SerializeField]
    private TextMeshProUGUI endTimerText;
    [SerializeField]
    private HealthController player;
    [SerializeField]
    private GameObject endZone;

    
    public Transform currentCheckpoint;


    //as a way to circumvent the issue of the FireHydrantController script not working after the player respawns @ a checkpoint,
    //i decided to make use of what i learned making events to let the FireHydrantController script know that the player has respawned to start the coroutine again
    public delegate void TriggerEventHandler(bool isRespawning);
    public event TriggerEventHandler RespawnEvent;

    [SerializeField]
    private EndZoneController endZoneDetection;
    [SerializeField] private AudioClip endGameSound;

    private bool isPaused;
    private float timerTime = 0f;

    void Awake()
    {
        // making the gamemaster a singleton pattern
        if (gm == null)
        {
            gm = this;
        }
        else
        {
            Destroy(gameObject);
        }


        //dev mode simply allows me to move around the player without teleporting me back to the spawn position
        // if the game starts elsewhere other than the start, this may possibly be left on without me noticing
        if (!enableDevMode)
        {
            playerPrefab.position = spawnPoint.position;
        }

        // subscribing the GameMaster to the EndZoneController's event
        endZoneDetection.OnLevelEndEvent += OnLevelEndOccured;
  
        currentCheckpoint = spawnPoint;

    }

    void Update()
    {
        //timer only runs when player is alive

        if (!player.isDead && !isPaused)
        {
            // adds time passed to timerTime, which is a variable to store time value in seconds
            timerTime += Time.deltaTime;
            // converts the numerical value of timerTime into a TimeSpan object, which is a c# structure that represents time
            TimeSpan time = TimeSpan.FromSeconds(timerTime);
            // takes the TimeSpan obj and converts it into string, with a format of minutes:seconds:milliseconds
            timerText.text = time.ToString(@"mm\:ss\:fff");
        }
        else
        {
            // sets the text for the PauseTimer, DeadTimer and EndTimer only when the game is either paused or the player is dead
            TimeSpan time = TimeSpan.FromSeconds(timerTime);
            pauseTimerText.text = time.ToString(@"mm\:ss\:fff");
            deadTimerText.text = time.ToString(@"mm\:ss\:fff");
            endTimerText.text = time.ToString(@"mm\:ss\:fff");

        }
        
        // Pauses the game by pressing Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //toggle between true and false states when Escape key is pressed
            isPaused = !isPaused;
            // when paused, it will set the pauseUi to be active and disable the PlayerController script on the playerPrefab
            if (isPaused)
            {
                pauseUi.SetActive(true);
                playerPrefab.GetComponent<PlayerController>().enabled = false;
            }
            else
            {
                pauseUi.SetActive(false);
                playerPrefab.GetComponent<PlayerController>().enabled = true;
            }
        }



    }

    //method is specifically in public static void to allow the HealthController to kill the player without creating another instance of GameMaster.
    public static void KillPlayer(HealthController player)
    {
        player.StartCoroutine(KillPlayerWithDelay(player));
        gm.LoseGame();
    }


    //LoseGame() and EndGame() simply enables their respective Uis when called and pauses the game
    public void LoseGame()
    {
        gameOverUi.SetActive(true);
        isPaused = true;
    }

    private void EndGame()
    {
        gameEndUi.SetActive(true);
        isPaused = true;
    }

    //function to handle respawning the player in GameMaster
    public void RespawnPlayer()
    {
        //sets the players position to the last checkpoint they reached.
        playerPrefab.position = currentCheckpoint.position;
        gameOverUi.SetActive(false);
        // penalty of 2.5s after respawning
        timerTime += 2.5f;
        //uses RespawnPlayer() in HealthController
        player.RespawnPlayer();
        RespawnEvent?.Invoke(true);
        Debug.Log("Player respawned");
    }

    // handling the LevelEndEvent that the script was subscribed to
    private void OnLevelEndOccured(bool isCompleted)
    {
        // disables the PlayerController script and executes EndGame()
        if (isCompleted)
        {
            playerPrefab.GetComponent<PlayerController>().enabled = false;
            SoundManager.instance.PlaySound(endGameSound);
            Debug.Log("Game is now ending.");
            EndGame();
        } 
    }


    //this coroutine to kill the player is in place to allow the death animation to play out before the player 'dies'
    public static IEnumerator KillPlayerWithDelay(HealthController player)
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Killed Player");

    }



    //these next few functions handle the buttons in each of the Ui screens where applicable (Pause, Death and Complete)

    // quits to the MainMenu scene by taking this scene's buildIndex - 1
    public void QuitToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }

    // resets the entire level by reloading the current scene
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isPaused = false;
    }

    // respawns the player to the previous checkpoint (if any) and unpauses the game
    public void LastCheckpoint()
    {
        RespawnPlayer();
        isPaused = false;
    }

}
