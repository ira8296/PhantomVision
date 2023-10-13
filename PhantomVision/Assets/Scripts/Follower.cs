using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public GameObject player;

    public float followSharpness = 0.1f;

    Vector3 _followOffset;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        _followOffset = transform.position - player.transform.position;

        transform.LookAt(player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = player.transform.position + _followOffset;

        float yDiff = transform.position.y - player.transform.position.y;
        //targetPosition.y = transform.position.y;

        transform.position += (targetPosition - transform.position) * followSharpness;

        transform.LookAt(player.transform.position);
    }
}
