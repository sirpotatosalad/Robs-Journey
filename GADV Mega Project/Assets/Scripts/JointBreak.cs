using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBreak : MonoBehaviour
{

    private HingeJoint2D hj;
    private DistanceJoint2D dj;
    private GameObject parentObj;

    // making an event for when a thrown object collides with the rope segments
    // this was made very early in development, and i now realise that this can probably be done more simply in one script
    // unfortunately i no longer have time to attempt to merge this script with RopeController
    public delegate void CollisionEventHandler(Collision2D collision);
    public event CollisionEventHandler OnCollisionEvent;

    // Start is called before the first frame update
    void Start()
    {
        hj = GetComponent<HingeJoint2D>();
        parentObj = transform.parent.gameObject;
        dj = parentObj.GetComponent<DistanceJoint2D>();

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        // when a "Throwable" tagged object collides with the rope segment above a certain velocity, it breaks the rope and broadcasts the event
        // this is done in BreakHinge()
        if (collision.gameObject.CompareTag("Throwable") && enabled)
        {
            if (collision.relativeVelocity.magnitude >= 5f)
            {
                BreakHinge();
                Debug.Log("Broken by " + collision.gameObject.name);
                OnCollisionEvent?.Invoke(collision);
            }
        }
            
    }

    //this method "breaks" the rope by disabling the hinge joint components at this specifc segment AND the distance joint at the parent
    void BreakHinge()
    {
        hj.enabled = false;
        dj.enabled = false;
    }
}
