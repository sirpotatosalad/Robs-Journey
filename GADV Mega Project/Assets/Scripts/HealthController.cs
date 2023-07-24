using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float startingHealth;
    public float currentHealth { get; private set; }

    private Animator anim;
    public bool isDead = false;

    [SerializeField] private float iFrameDuration;




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
            StartCoroutine(IFrames());
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

    private IEnumerator IFrames()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true);
        yield return new WaitForSeconds(iFrameDuration);    
        Physics2D.IgnoreLayerCollision(8,9, false);
    }

}
