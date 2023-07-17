using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool isDead = false;




    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float dmgTaken)
    {
        currentHealth = Mathf.Clamp(currentHealth - dmgTaken, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("isHurt");
            //player alive
        }
        else
        {
            if (!isDead)
            {
                anim.SetTrigger("isDead");
                this.GetComponent<PlayerController>().enabled = false;
                isDead = true;
            }
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
