using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    private GameObject parentObj;
    private List<GameObject> children;
    // Start is called before the first frame update
    void Start()
    {
        parentObj = gameObject;
        children = new List<GameObject>();

        GetAllChildren();
        Debug.Log(children.Count);

        foreach (GameObject child in children)
        {
            JointBreak jointBreak = child.GetComponent<JointBreak>();
            //jointBreak.OnCollisionEvent += HandleCollisionEvent;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetAllChildren()
    {
        foreach (Transform child in parentObj.transform)
        {
            JointBreak collisionHandlers = child.GetComponent<JointBreak>();
            if (collisionHandlers != null)
            {
                children.Add(child.gameObject);
            }
        }
    }

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

    void HandleCollisionEvent()
    {

    }

}
