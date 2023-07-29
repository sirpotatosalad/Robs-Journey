using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    public float startingHealth;
    public float currentHealth { get; private set; }

    public int fallBoundary;

    private Animator anim;

    public bool isDead = false;



    [SerializeField] private float iFrameDuration;




    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //kills player if they go below the fall boundary
        if (transform.position.y <= fallBoundary)
        {
            //have tried using Gamemaster.KillPlayer, though this still feels better and is less buggy.
            //also, falling off the map nets an instant loss.
            TakeDamage(999999999);
        }
    }

    public void TakeDamage(float dmgTaken)
    {
        //Clamping the damage that can be taken in case of any unexpected damage numbers.
        currentHealth = Mathf.Clamp(currentHealth - dmgTaken, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("isHurt");
            StartCoroutine(IFrames());
        }
        else
        {
            if (!isDead)
            {
                anim.SetTrigger("isDead");
                this.GetComponent<PlayerController>().enabled = false;
                isDead = true;
                GameMaster.KillPlayer(this);

            }
        }
    }

    public void RespawnPlayer()
    {
        currentHealth = startingHealth;
        isDead = false;
        anim.ResetTrigger("isDead");
        anim.Play("Rob_idle");
        this.GetComponent<PlayerController>().enabled = true;
        //give player IFrames to prevent them from dying immediately after respawn.
        StartCoroutine(IFrames());
    }

    private IEnumerator IFrames()
    {
        Physics2D.IgnoreLayerCollision(8,9, true);
        yield return new WaitForSeconds(iFrameDuration);    
        Physics2D.IgnoreLayerCollision(8,9, false);
    }

}
