using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHydrantBehaviour : MonoBehaviour
{
    [SerializeField] private float activationInterval = 2.0f;
    [SerializeField] private int damageAmount = 1;

    private bool isActive = false;
    [SerializeField]
    private GameObject waterSpout;

    private void Start()
    {       
        waterSpout.SetActive(false); // Disable the water spout collider initially.
        StartCoroutine(ActivateWaterSpout());
    }

    void Update()
    {
        Debug.Log(isActive);
    }

    private IEnumerator ActivateWaterSpout()
    {
        while (true)
        {
            isActive = true;
            waterSpout.SetActive(true); // Enable the water spout collider.
            yield return new WaitForSeconds(activationInterval);
            isActive = false;
            waterSpout.SetActive(false); // Disable the water spout collider.
            yield return new WaitForSeconds(activationInterval);
        }
    }

    private void OnTriggerEnter2D(Collider2D player)
    {
        Debug.Log("collision detected");

        if (isActive && player.CompareTag("Player"))
        {
            Debug.Log("Taking damage!");
            HealthController healthController = player.GetComponent<HealthController>();
            healthController.TakeDamage(damageAmount);

        }
    }
}
