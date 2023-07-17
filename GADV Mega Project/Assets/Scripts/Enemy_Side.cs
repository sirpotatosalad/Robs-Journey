using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Side : MonoBehaviour
{
    [SerializeField]
    private float moveDist;
    [SerializeField]
    private float moveSpeed;

    private float leftConstraint;
    private float rightConstraint;
    private bool movingLeft;

    [SerializeField]
    private float damage;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<HealthController>().TakeDamage(damage);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        leftConstraint = transform.position.x - moveDist;
        rightConstraint = transform.position.x + moveDist;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingLeft)
        {
            if (transform.position.x > leftConstraint) 
            {
                transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                movingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightConstraint)
            {
                transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                movingLeft = true;
            }
        }
    }
}
