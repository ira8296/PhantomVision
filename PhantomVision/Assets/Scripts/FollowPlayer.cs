using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public float followSharpness = 0.1f;

    Vector3 _followOffset;

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
        transform.LookAt(player.position);
    }
}
