using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBreak : MonoBehaviour
{

    private HingeJoint2D hj;
    private DistanceJoint2D dj;
    private GameObject parentObj;


    public delegate void CollisionEventHandler(Collision2D collision);
    public event CollisionEventHandler OnCollisionEvent;

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
                Debug.Log("Broken by " + collision.gameObject.name);
                OnCollisionEvent?.Invoke(collision);
            }
        }
            
    }

    void BreakHinge()
    {
        hj.enabled = false;
        dj.enabled = false;
    }
}
