using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private HealthController playerHealth;
    [SerializeField]
    private Image totalHealth;
    [SerializeField]
    private Image currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        // sets the number of total hearts to fill based on player's current health
        // divided by 10 as the sprite accounts for 10 total hearts
        totalHealth.fillAmount = playerHealth.currentHealth / 10;
    }

    // Update is called once per frame
    void Update()
    {
        // shows the current health the player is at
        // reason for dividing by 10 is the same as above
        currentHealth.fillAmount = playerHealth.currentHealth / 10;
    }
}
