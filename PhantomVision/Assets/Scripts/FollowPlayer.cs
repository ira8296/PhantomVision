using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float xDiff = player.position.x - transform.position.x;
        float yDiff = player.position.y - transform.position.y;
        float zDiff = player.position.z - transform.position.z;

        //transform.position = player.position + new Vector3(xDiff, yDiff, zDiff);
    }
}
