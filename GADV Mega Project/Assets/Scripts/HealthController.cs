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
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;




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
            //have tried using Gamemaster.KillPlayerl, but the TakeDamage() function seems to be more stable
            //in this case, I will just give an exorbitant amount of damage to the player if they fall into a hole (i.e, out of the map).
            TakeDamage(999999999);
        }
    }

    public void TakeDamage(float dmgTaken)
    {
        //Clamping the damage that can be taken in case of any unexpected damage numbers.
        currentHealth = Mathf.Clamp(currentHealth - dmgTaken, 0, startingHealth);

        // checks if the player still has health after taking damage
        if (currentHealth > 0)
        {
            // set the "isHurt" trigger in animator to play out the hurt animation
            SoundManager.instance.PlaySound(hurtSound);
            anim.SetTrigger("isHurt");
            StartCoroutine(IFrames());
        }
        else
        {
            if (!isDead)
            {
                SoundManager.instance.PlaySound(deathSound);
                // set the "isDead" trigger to play out the death animation
                anim.SetTrigger("isDead");
                // disable the PlayerController script to prevent player from moving after death
                this.GetComponent<PlayerController>().enabled = false;
                isDead = true;
                //Kill player through GameMaster
                GameMaster.KillPlayer(this);

            }
        }
    }

    public void RespawnPlayer()
    {
        // regenerate the player's health to full
        currentHealth = startingHealth;
        isDead = false;
        // reset the "isDead" trigger and play the idle animation to return to normal animation transitions
        anim.ResetTrigger("isDead");
        anim.Play("Rob_idle");
        // enable the PlayerController script to allow player movement again
        this.GetComponent<PlayerController>().enabled = true;
        //give player IFrames to prevent them from dying immediately after respawn
        StartCoroutine(IFrames());
    }

    private IEnumerator IFrames()
    {
       // this coroutine for IFrames (invulnerability frames) disables the layer collisions between the Player and Enemy Layers
        Physics2D.IgnoreLayerCollision(8,9, true);
        yield return new WaitForSeconds(iFrameDuration);    
        Physics2D.IgnoreLayerCollision(8,9, false);
    }

}
