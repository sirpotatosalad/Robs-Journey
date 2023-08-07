using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float lookAheadDist;
    [SerializeField] private float camFollowSpeed;
    [SerializeField] private float camCentreSpeed;
    [SerializeField] private float maxVerticalHeight;
    private float lookAhead;
    private float defaultCamHeight;

    void Start()
    {
        defaultCamHeight = transform.position.y;
    }



    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {

            float playerSpeed = player.GetComponent<Rigidbody2D>().velocity.x;

            if (Mathf.Abs(playerSpeed) > 0f)
            {
                // use Mathf.Lerp to linearly pan the camera slightly ahead of the player
                lookAhead = Mathf.Lerp(lookAhead, (lookAheadDist * player.localScale.x), Time.deltaTime * camFollowSpeed);
                transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);

            }
            else
            {
                // same principles as before, but this time to center to the player when they are not moving
                float lookAheadCentred = 0f;
                lookAhead = Mathf.Lerp(lookAhead, lookAheadCentred, Time.deltaTime * camCentreSpeed);
                transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
            }

            float playerHeight = player.position.y;
            float camHeight = transform.position.y;

            // this next block follows the same principles as before, this time to follow the player vertically once they
            // reach a certain transform.position.y

            if (playerHeight > maxVerticalHeight)
            {
                float targetCamHeight = Mathf.Lerp(camHeight, playerHeight, Time.deltaTime * camFollowSpeed);
                transform.position = new Vector3(transform.position.x, targetCamHeight, transform.position.z);
            }
            else
            {
                float targetCamHeight = Mathf.Lerp(camHeight, defaultCamHeight, Time.deltaTime * camFollowSpeed);
                transform.position = new Vector3(player.position.x, targetCamHeight, transform.position.z);
            }


        }
       
    }

}
