using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script is called FireHydrantBehaviour, but I decided to use it for my generator trap as well.
// I didnt want to break my code through renaming scripts, so the name is a little strange.

public class FireHydrantBehaviour : MonoBehaviour
{
    [SerializeField] private float activationInterval = 2.0f;
    [SerializeField] private int damageAmount = 1;

    private bool isActive = false;
    private bool isDamagingPlayer = false;


    [SerializeField]
    private GameObject waterSpout;
    [SerializeField]
    private HealthController playerPrefab;


    private void Start()
    {       
        waterSpout.SetActive(false); // Disable the water spout collider initially.
        StartCoroutine(ActivateWaterSpout());

        GameMaster.gm.RespawnEvent += OnRespawnOccured;
    }

    //the coroutine is currently called in Start(). this may change if i find a better method that is more reliable
    private IEnumerator ActivateWaterSpout()
    {
        while (!playerPrefab.isDead)
        {
            //activates and deactivates the trap within set intervals,
            // also does so for the waterSpout object to show the trap activating by setting the gameObject between SetActive states
            isActive = true;
            waterSpout.SetActive(true);
            yield return new WaitForSeconds(activationInterval);
            isActive = false;
            waterSpout.SetActive(false);
            yield return new WaitForSeconds(activationInterval);
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //checks if the trap is active and not already damaging the player
        while (isActive && collision.CompareTag("Player") && !isDamagingPlayer)
        {
            // damages the player when they stay in the water spray (i.e., trap is active)
            Debug.Log("Taking damage!");
            StartCoroutine(DamageDelay());

        }

    }

    //make use of the RespawnEvent in GameMaster to start up the main coroutine that makes the trap timed.
    private void OnRespawnOccured(bool isRespawning)
    {
        if (isRespawning)
        {
            StartCoroutine(ActivateWaterSpout());
        }
    }


    //currently, i am making use of a coroutine to delay the damage given to the player to prevent instant death when crossing the trap while its active.
    private IEnumerator DamageDelay()
    {
        isDamagingPlayer = true;
        yield return new WaitForSeconds(0.2f);
        playerPrefab.TakeDamage(damageAmount);
        isDamagingPlayer = false;
    }

}
