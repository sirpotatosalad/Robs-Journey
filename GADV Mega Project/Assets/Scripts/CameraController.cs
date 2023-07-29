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
    private float lookAhead;


    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {

            float playerSpeed = Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.x);

            if (Mathf.Abs(playerSpeed) > 0f)
            {
                lookAhead = Mathf.Lerp(lookAhead, (lookAheadDist * player.localScale.x), Time.deltaTime * camFollowSpeed);
                transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);

            }
            else
            {
                float lookAheadCentred = 0f;
                lookAhead = Mathf.Lerp(lookAhead, lookAheadCentred, Time.deltaTime * camCentreSpeed);
                transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
            }
        }
       
    }

}
