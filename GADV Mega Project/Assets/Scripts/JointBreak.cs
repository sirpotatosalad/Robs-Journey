using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBreak : MonoBehaviour
{

    private HingeJoint2D hj;
    private DistanceJoint2D dj;
    //private bool isBroken = false;
    //not sure whether i should implement this or not
    private GameObject parentObj;

    // Start is called before the first frame update
    void Start()
    {
        hj = GetComponent<HingeJoint2D>();
        parentObj = transform.parent.gameObject;
        dj = parentObj.GetComponent<DistanceJoint2D>();

    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Throwable"))
        {
            if (collision.relativeVelocity.magnitude >= 15f)
            {
                BreakHinge();
            }
        }
            
    }

    void BreakHinge()
    {
        hj.enabled = false;
        dj.enabled = false;
    }
}
