using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    void Update()
    {
        
    }

    private IEnumerator ActivateWaterSpout()
    {
        while (!playerPrefab.isDead)
        {
            isActive = true;
            waterSpout.SetActive(true); // Enable the water spout collider.
            yield return new WaitForSeconds(activationInterval);
            isActive = false;
            waterSpout.SetActive(false); // Disable the water spout collider.
            yield return new WaitForSeconds(activationInterval);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("collision detected");
        Debug.Log(isDamagingPlayer);
        Debug.Log(isActive);
        while (isActive && collision.CompareTag("Player") && !isDamagingPlayer)
        {
            Debug.Log("Taking damage!");
            StartCoroutine(DamageDelay());

        }

    }


    private IEnumerator DamageDelay()
    {
        isDamagingPlayer = true;
        yield return new WaitForSeconds(0.15f);
        playerPrefab.TakeDamage(damageAmount);
        isDamagingPlayer = false;
    }

}
