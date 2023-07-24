using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FireHydrant : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float trapDelay;
    [SerializeField] private float trapTime;
    private Animator anim;
    private SpriteRenderer sr;

    private bool isTriggered;
    private bool isActive;

    private HealthController playerHealth;


    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive && playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!isTriggered)
            {
                StartCoroutine(ActivateFireHydrant());
                playerHealth = collision.GetComponent<HealthController>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerHealth = null;
    }

    private IEnumerator ActivateFireHydrant()
    {
        isTriggered = true;
        sr.color = Color.blue;

       
        yield return new WaitForSeconds(trapDelay);
        sr.color = Color.white; 
        isActive = true;
        //anim.SetBool("activated", true);

        
        yield return new WaitForSeconds(trapTime);
        isActive = false;
        isTriggered = false;
        //anim.SetBool("activated", false);
    }

}
