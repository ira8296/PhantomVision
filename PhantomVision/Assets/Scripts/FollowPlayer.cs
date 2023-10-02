using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float followSharpness = 0.1f;

    Vector3 _followOffset;

    float X;
    float Y;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        _followOffset = transform.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = player.position + _followOffset;

        float yDiff = transform.position.y - player.position.y;
        targetPosition.y = transform.position.y;

        transform.position += (targetPosition - transform.position) * followSharpness;

        if (Input.GetMouseButton(1))
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * 1f, -Input.GetAxis("Mouse X") * 1f, 0));
            X = transform.rotation.eulerAngles.x;
            Y = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(X, Y, 0);
            //transform.LookAt(-worldPosition);
        }
        else
        {
            transform.LookAt(player.position);
        }
    }
}
