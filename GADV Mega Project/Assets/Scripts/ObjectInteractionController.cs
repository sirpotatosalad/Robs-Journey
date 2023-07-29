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
        if (isGrabbing)
        {
            if (Input.GetKeyDown(KeyCode.R) && grabbedObj != null)
            {
                ToggleThrowing();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            { 
                ReleaseObject();
            }
              
        }
        else
        {
            if (Input.GetKey(KeyCode.E)) 
            {
                GrabObject();
            }
        }

        if (isThrowing)
        {
            if (Input.GetMouseButton(0))
            {
                if (Input.GetMouseButtonDown(1))
                {
                    lr.enabled = false;
                    Debug.Log("Cancelled Throw");
                    return;
                }

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

        isThrowing = !isThrowing;
        if(isThrowing) 
        {
            grabbedObj.transform.position = throwPos.position;
            Debug.Log("Ready to throw");
        }
        else
        {
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
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 distance = mousePos - transform.position;

        float normalisedDist = Mathf.Clamp01(distance.magnitude / maxMouseDist);
        float throwForce = throwMultiplier * normalisedDist;

        throwingVector = -distance.normalized * throwForce;

        DrawTrajectory();
    }

    void DrawTrajectory()
    {
        lr = grabbedObj.GetComponent<LineRenderer>();

        Vector3[] trajectoryPoints = new Vector3[numberOfPoints];

        for (int i = 0; i < numberOfPoints; i++)
        {
            float time = timeBetweenPoints * i;
            Vector2 trajectoryPoint = CalculateTrajectoryPoint(time);
            trajectoryPoints[i] = new Vector3(trajectoryPoint.x, trajectoryPoint.y, 0f);
        }

        lr.positionCount = numberOfPoints;
        lr.SetPositions(trajectoryPoints);
        lr.enabled = true;
    }

    Vector2 CalculateTrajectoryPoint(float time)
    {
        Vector2 initPos = grabbedObj.transform.position;
        Vector2 velocity = throwingVector;
        Vector2 gravity = Physics2D.gravity;

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
