using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;

    [SerializeField]
    private GameObject gameOverUi;
    [SerializeField] 
    private GameObject pauseUi;
    [SerializeField]
    private bool enableDevMode = false;

    public float spawnDelay = 2.0f;
    public Transform playerPrefab;
    public Transform playerCam;
    public Transform spawnPoint;
    public TextMeshProUGUI timerText;

    private float timerTime = 0f;
    
    private Animator anim;

    public HealthController player;
    public Transform currentCheckpoint;
    public bool isRunning;

    [SerializeField]
    private CheckpointBehaviour triggerDetection;

    private bool isPaused;

    void Awake()
    {
        // making the gamemaster a singleton
        if (gm == null)
        {
            gm = this;
        }
        else
        {
            Destroy(gameObject);
        }

        anim = playerPrefab.GetComponent<Animator>();

        
        if (!enableDevMode)
        {
            playerPrefab.position = spawnPoint.position;
        }

       
        triggerDetection.OnTriggerEvent += OnTriggerEventOccured;

        currentCheckpoint = spawnPoint;

        isRunning = true;

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
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
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

    public static void KillPlayer(HealthController player)
    {
        player.StartCoroutine(KillPlayerWithDelay(player));
        gm.EndGame();
    }

    public void EndGame()
    {
        gameOverUi.SetActive(true);
        isRunning = false;
    }

    // coroutine in place to allow a spawn delay, punishing players who respawn to the last checkpoint
    public IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(spawnDelay);
        playerPrefab.position = currentCheckpoint.position;
        player.RespawnPlayer();
        gameOverUi.SetActive(false);
        Debug.Log("Player respawned");
    }

    private void OnTriggerEventOccured(Collider2D collision)
    {
        Debug.Log("new checkpoint set to: " + currentCheckpoint);
    }


    //this coroutine to kill the player is in place to allow the death animation to play out before the player 'dies'
    public static IEnumerator KillPlayerWithDelay(HealthController player)
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Killed Player");

    }


    public void QuitToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isRunning = true;
    }

    public void LastCheckpoint()
    {
        if (currentCheckpoint != spawnPoint)
        {
            Debug.Log("teleporting player to last checkpoint");
        }
        StartCoroutine(RespawnPlayer());
        isRunning = true;
    }

}
