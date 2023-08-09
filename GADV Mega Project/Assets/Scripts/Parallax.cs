using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length;
    private float startPos;
    public GameObject cam;
    public float parallaxEffect;

    
    // the parallax background obviously has seam lines and it is because the actual background that i am using is not tilealbe.
    // unfortunately i cannot fix this without having to re-do the background itself, and i don't have much time to do so

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        // this is the initial offset of the background layer relative to the camera
        float offset = (cam.transform.position.x * (1 - parallaxEffect));
        // this is the distance the background layer will move relative to the camera
        float dist = (cam.transform.position.x * parallaxEffect);
        // moves the background layer based on the distance set via the parallax effect
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        //this block checks if the background layer has already been offset past its intial bound
        // basically, it will move the background layer forward or back to allow it to continuously apply the parallax effect without it being left behind.
        if (offset > startPos + length)
        {
            startPos += length;
        }
        else if (offset < startPos - length)
        {
            startPos -= length;
        }
    }
}
