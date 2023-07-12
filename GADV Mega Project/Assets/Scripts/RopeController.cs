using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    private GameObject parentObj;
    private int noOfChild;
    private List<GameObject> children;
    // Start is called before the first frame update
    void Start()
    {
        parentObj = gameObject;
        noOfChild = transform.childCount - 1;
        children = new List<GameObject>();

        GetAllChildren();
        Debug.Log(children.Count);
            

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetAllChildren()
    {
        foreach (Transform child in parentObj.GetComponentInChildren<Transform>())
        {
            JointBreak collisionHandlers = child.GetComponent<JointBreak>();
            if (collisionHandlers != null)
            {
                children.Add(child.gameObject);
            }
        }
    }

}
