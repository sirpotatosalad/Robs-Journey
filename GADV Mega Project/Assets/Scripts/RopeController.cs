using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    private GameObject parentObj;
    private List<GameObject> children;
    
    void Start()
    {
        parentObj = gameObject;

        // create a list to store all child objects (i.e., individual rope segments)
        children = new List<GameObject>();

        GetAllChildren();
        Debug.Log(children.Count);
        
        // subscribe each child to the collision event in JointBreak script
        foreach (GameObject child in children)
        {
            JointBreak jointBreak = child.GetComponent<JointBreak>();
            jointBreak.OnCollisionEvent += HandleCollisionEvent;
        }

    }

    void GetAllChildren()
    {
        //finding each child object in the parent object
        foreach (Transform child in parentObj.transform)
        {
            JointBreak collisionHandlers = child.GetComponent<JointBreak>();
            // adds child objects that have the JointBreak script (i.e., the rope segments)
            if (collisionHandlers != null)
            {
                children.Add(child.gameObject);
                Debug.Log("Child added: " + child.gameObject.name);
            }
            else
            {
                Debug.Log("No JointBreak Component found in: " + child.gameObject.name);
            }
        }
    }

    // disables the JointBreak scripts in each child object
    // this prevents the player from breaking ropes multiple times
    public void DisableCollisonScripts()
    {
        foreach (GameObject child in children)
        {
            JointBreak collisionHandlers = child.GetComponent<JointBreak>();
            if (collisionHandlers != null)
            {
                collisionHandlers.enabled = false;
                Debug.Log("Rope break disabled");
            }
        }
    }

    // handles the collision event when it is broadcast
    void HandleCollisionEvent(Collision2D collision)
    {
        DisableCollisonScripts();
        Debug.Log("Collision event successfully handled");
    }

}
