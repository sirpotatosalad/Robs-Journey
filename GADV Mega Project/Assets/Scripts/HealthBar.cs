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
        totalHealth.fillAmount = playerHealth.currentHealth / 10;
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth.fillAmount = playerHealth.currentHealth / 10;
    }
}
