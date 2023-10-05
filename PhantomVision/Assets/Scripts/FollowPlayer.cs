using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float followSharpness = 0.1f;

    Vector3 _followOffset;

    float dragSpeed = 0.5f;
    float speedMod = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        _followOffset = transform.position - player.position;

        transform.LookAt(player.position);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = player.position + _followOffset;

        float yDiff = transform.position.y - player.position.y;
        //targetPosition.y = transform.position.y;

        transform.position += (targetPosition - transform.position) * followSharpness;

        if (Input.GetMouseButton(1)) //With right-mouse button, rotate camera around player
        {

            Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * dragSpeed, Vector3.up);

            _followOffset = camTurnAngle * _followOffset;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f) //Scroll up to zoom in
        {
            if(Camera.main.fieldOfView > 7.5)
            {
                Camera.main.fieldOfView = (Camera.main.fieldOfView / 2f);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) //Scroll down to zoom out
        {
            if(Camera.main.fieldOfView < 89.5) 
            {
                Camera.main.fieldOfView = (Camera.main.fieldOfView * 2f);
            }
        }
        transform.LookAt(player.position);
    }
}
