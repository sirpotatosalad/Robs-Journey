using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractionController : MonoBehaviour
{
    [SerializeField]
    private Transform grabPos;
    [SerializeField]
    //initial ray position for grab detection
    private Transform rayPos;
    [SerializeField]
    private Transform throwPos;

    [SerializeField]
    private float rayDist;

    public GameObject grabbedObj;
    private int layerIndex;
    public bool isGrabbing;
    public bool isThrowing;

    public float throwMultiplier = 10.0f;


    private Vector3 throwingVector;
    private Rigidbody2D rb;
    private LineRenderer lr;

    [SerializeField]
    private float maxMouseDist = 10.0f;
    [SerializeField]
    private int numberOfPoints = 20;
    [SerializeField]
    private float timeBetweenPoints = 0.1f;



    // Start is called before the first frame update
    void Start()
    {
        layerIndex = LayerMask.NameToLayer("Throwable");
        
    }

    // Update is called once per frame
    void Update()
    {
        // check if the player is grabbing any object
        if (isGrabbing && grabbedObj != null)
        {
            // toggle between throwing and grab mode if player is grabbing an object
            if (Input.GetKeyDown(KeyCode.R))
            {
                ToggleThrowing();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                lr.enabled = false;
                ReleaseObject();
            }
              
        }
        else
        {
            //allow the player to grab an object
            if (Input.GetKey(KeyCode.E)) 
            {
                GrabObject();
            }
        }

        if (isThrowing && !Input.GetMouseButton(1))
        {
            if (Input.GetMouseButton(0))
            {
                CalculateVector();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                lr.enabled = false;
                ThrowObject();
                isThrowing = false;
                isGrabbing = false;
                grabbedObj = null;
            }
        }


    }

    void ToggleThrowing()
    {

        lr = grabbedObj.GetComponent<LineRenderer>();
        //simple switch statement
        isThrowing = !isThrowing;
        if(isThrowing) 
        {
            //set the position of the object to throwing position
            grabbedObj.transform.position = throwPos.position;
            Debug.Log("Ready to throw");
        }
        else
        {
            //disable the line-renderer (if any) and sets the position of the object back to grab position.
            // this also acts as a throw cancel
            lr.enabled = false;
            grabbedObj.transform.position = grabPos.position;
            Debug.Log("Returned to grab");
        }
        
    }

    void GrabObject()
    {
        //setting raycast from rayPos gameObject
        RaycastHit2D hitInfo = Physics2D.Raycast(rayPos.position, transform.right, rayDist);

        //check if raycast is colliding with a gameobject in the correct layer
        if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex)
        {
            //'grab' gameobject when hands are empty
            if (Input.GetKeyDown(KeyCode.E) && grabbedObj == null && !isGrabbing)
            {
                //get gameObj infront of collider
                grabbedObj = hitInfo.collider.gameObject;
                //set state of grabbed obj's rigidbody to kinematic
                grabbedObj.GetComponent<Rigidbody2D>().isKinematic = true;
                // set position of object in front of player object
                grabbedObj.transform.position = grabPos.position;
                //set grabbed obj as child to player
                grabbedObj.transform.SetParent(transform);
                isGrabbing = true;
                Debug.Log("Object grabbed");
            }
        }

    }

    void ReleaseObject()
    {
        //undo changes done when grabbed
        isGrabbing = false;
        isThrowing = false;
        grabbedObj.GetComponent<Rigidbody2D>().isKinematic = false;
        grabbedObj.transform.SetParent(null);
        grabbedObj = null;
        Debug.Log("Object dropped");
    }

    void CalculateVector()
    {
        // assigning the initial mouse position when left mouse button is held down
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //taking the distance between the mouse and its initial position
        Vector2 distance = mousePos - transform.position;

        // this block allows the player to throw at varying forces based on how far they pull back the mouse
        // preventing the player from pulling back too much by clamping the distance they can pull the mouse back
        float clampedDist = Mathf.Clamp01(distance.magnitude / maxMouseDist);
        // the throwforce is then calculated by multiplying the distance with a throwMultiplier
        float throwForce = throwMultiplier * clampedDist;

        // normalising the distance calculated and calculating the vector force to throw
        throwingVector = -distance.normalized * throwForce;

        DrawTrajectory();
    }

    void DrawTrajectory()
    {
        lr = grabbedObj.GetComponent<LineRenderer>();

        // makes an array of based on the number of points given
        // here, the numberOfPoints variable sort of acts like the total number of points for the entire vector i.e., the length
        Vector3[] trajectoryPoints = new Vector3[numberOfPoints];

        // using a for loop to iterate through the array in order to calculate the distance (in this case in time) between each point
        // similar to numberOfPoints var, timeBetweenPoints acts like the distance between each point measured in time
        for (int i = 0; i < numberOfPoints; i++)
        {
            // here the time value to be given to the CalculateTrajectoryPoint function is calculated by multiplying
            // i to the time between each point
            float time = timeBetweenPoints * i;
            // the next 2 lines will simply assign the Vector2 of each calculated point to each index of the array
            Vector2 trajectoryPoint = CalculateTrajectoryPoint(time);
            trajectoryPoints[i] = new Vector3(trajectoryPoint.x, trajectoryPoint.y, 0f);
        }
        
        //these next few lines then assign the position of each point using lineRenderer
        lr.positionCount = numberOfPoints;
        lr.SetPositions(trajectoryPoints);
        lr.enabled = true;
    }

    Vector2 CalculateTrajectoryPoint(float time)
    {
        Vector2 initPos = grabbedObj.transform.position;
        Vector2 velocity = throwingVector;
        Vector2 gravity = Physics2D.gravity;

        // with the help of some prior knowledge of physics and ChatGPT, I make use of the kinetic equation of motion for an object in free fall
        // after calculation, it simply returns the Vector2 for that specific point in flight.
        Vector2 pos = initPos + velocity * time + 0.5f * gravity * time * time;
        return pos;
    }

    void ThrowObject()
    {
        rb = grabbedObj.GetComponent<Rigidbody2D>();

        Debug.Log("Throwing");
        grabbedObj.GetComponent<Rigidbody2D>().isKinematic = false;
        grabbedObj.transform.SetParent(null);
        rb.velocity = throwingVector / rb.mass;
    }




}
