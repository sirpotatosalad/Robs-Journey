using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwables : MonoBehaviour
{
    public float throwForce = 10.0f;

    private Vector3 throwingVector;
    private Rigidbody2D rb;
    private LineRenderer lr;
    private GameObject throwableObj;

    // Start is called before the first frame update
    void Start()
    {
        throwableObj = this.gameObject;
        rb = this.GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && throwableObj.transform.parent != null)
        {
            CalculateVector();
            UpdateArrow();
        }
      
    }

    private void OnMouseDown()
    {
        if (throwableObj.transform.parent != null)
        {
            SetArrow();
            Debug.Log("Down");
        }
       
    }

    private void OnMouseUp()
    {
        if(throwableObj.transform.parent != null)
        {
            RemoveArrow();
            Throw();
            Debug.Log("Up");
        }
        
    }

    void CalculateVector()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 distance = mousePos - transform.position;
        throwingVector = -distance.normalized * throwForce;
        Debug.Log("Throwing Vector: " + throwingVector);
    }

    void Throw()
    {
        Debug.Log("Throwing");
        throwableObj.GetComponent<Rigidbody2D>().isKinematic = false;
        throwableObj.transform.SetParent(null);
        rb.velocity = throwingVector / rb.mass;
    } 

    void SetArrow()
    {
        lr.positionCount = 2;
        lr.SetPosition(0, throwableObj.transform.position);
        lr.SetPosition(1, throwableObj.transform.position + throwingVector.normalized);
        lr.enabled = true;
    }

    void UpdateArrow()
    {
        if (lr.enabled)
        {
            lr.SetPosition(0, throwableObj.transform.position);
            lr.SetPosition(1, throwableObj.transform.position + throwingVector.normalized * 2);
        }
    }

    void RemoveArrow()
    {
        lr.enabled = false;
    }
}
