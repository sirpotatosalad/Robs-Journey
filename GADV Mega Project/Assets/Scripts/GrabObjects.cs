using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjects : MonoBehaviour
{
    [SerializeField]
    private Transform grabPos;
    [SerializeField]
    private Transform rayPos;
    [SerializeField]
    private float rayDist;

    public GameObject grabbedObj;
    private int layerIndex;

    // Start is called before the first frame update
    void Start()
    {
        layerIndex = LayerMask.NameToLayer("Object");
    }

    // Update is called once per frame
    void Update()
    {
        //setting raycast to rayPos gameObject
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPos.position, transform.right, rayDist);

        //check if raycast is colliding with a gameobject, and whether it is in the 'Objects' layer
        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex) 
        { 
            //'grab' gameobject when hands are empty
            if (Input.GetKeyDown(KeyCode.E) && grabbedObj == null)
            {
                //get gameObj infront of collider
                grabbedObj = hitInfo.collider.gameObject;
                //set state of grabbed obj's rigidbody to kinematic
                grabbedObj.GetComponent<Rigidbody2D>().isKinematic = true;
                // set position of object in front of player object
                grabbedObj.transform.position = grabPos.position;
                //set grabbed obj as child to player
                grabbedObj.transform.SetParent(transform);
            }
            //release gameObj in hand when grab key is pressed
            else if (Input.GetKeyDown(KeyCode.E))
            {
                //undo changes done when grabbed
                grabbedObj.GetComponent<Rigidbody2D>().isKinematic = false;
                grabbedObj.transform.SetParent(null);
                grabbedObj = null;
            }
        }

    }
}
