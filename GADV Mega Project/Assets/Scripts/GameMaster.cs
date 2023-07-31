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
    private bool enableDevMode = false;

    public float spawnDelay = 2.0f;
    public Transform playerPrefab;
    public Transform playerCam;
    public Transform spawnPoint;
    public TextMeshProUGUI timerText;

    private float timerTime = 0;
    
    private Animator anim;

    public HealthController player;
    


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
        
    }

    void Update()
    {
        //timer only runs when player is alive
        if (!player.isDead)
        {
            // adds time passed to timerTime, which is a variable to store time value in seconds
            timerTime += Time.deltaTime;
            // converts the numerical value of timerTime into a TimeSpan object, which is a c# structure that represents time
            TimeSpan time = TimeSpan.FromSeconds(timerTime);
            // takes the TimeSpan obj and converts it into string, with a format of minutes:seconds:milliseconds
            timerText.text = time.ToString(@"mm\:ss\:fff");
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
    }

    public IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(spawnDelay);
        playerPrefab.position = spawnPoint.position;
        player.RespawnPlayer();
        Debug.Log("Player respawned");
    }


    //this coroutine to kill the player is in place simply to allow the death animation to play out before the player 'dies'
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
        StartCoroutine(RespawnPlayer());
    }

}
