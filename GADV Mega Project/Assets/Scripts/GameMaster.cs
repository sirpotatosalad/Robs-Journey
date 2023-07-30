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

    private float timerTime;
    
    private Animator anim;

    public HealthController player;
    


    void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
        else
        {
            Destroy(gameObject);
        }

        timerTime = 0;
        anim = playerPrefab.GetComponent<Animator>();

        if (!enableDevMode)
        {
            playerPrefab.position = spawnPoint.position;
        }
        
    }

    void Update()
    {
        //timer behaviour
        // will work it out to only reset the timer when all lives are taken.
        if (!player.isDead)
        {
            timerTime += Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(timerTime);
            timerText.text = time.ToString(@"mm\:ss\:fff");
            timerTime += Time.deltaTime;
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
