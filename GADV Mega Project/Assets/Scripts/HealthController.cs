using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float startingHealth;
    public float currentHealth { get; private set; }





    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(float dmgTaken)
    {
        currentHealth = Mathf.Clamp(currentHealth - dmgTaken, 0, startingHealth);

        if (currentHealth > 0)
        {
            //player alive
        }
        else
        {
            //player dead
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            TakeDamage(1);
        }
    }
}
