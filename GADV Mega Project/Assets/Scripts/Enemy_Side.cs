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
        //sets the constraints the gameObject is limited to based on the desired moveDist.
        leftConstraint = transform.position.x - moveDist;
        rightConstraint = transform.position.x + moveDist;
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the attached gameObject is moving left or right
        if (movingLeft)
        {
            // if it detects the gameObject's position.x > leftConstraint (i.e., to the right of leftConstraint)
            // it will move the gameObject toward the leftConstrant by applying a negative moveSpeed (i.e., movingLeft = true)
            // if not, movingLeft = false
            if (transform.position.x > leftConstraint) 
            {
                transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                movingLeft = false;
            }
        }
        // the same logic applies here when the gameObject is considered to be !movingLeft
        // in this case it simply checks if its position.x < rightConstraint (i.e., to the left of the rightConstraint)
        // this will then move the gameObject toward the rightConstraint by applying a positive moveSpeed.
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
