using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCAI : MonoBehaviour
{
    public Vector3 position;
    public Vector3 direction;
    public float speed;
    public float maxSpeed;
    public Vector3 velocity;

    public GameObject player;
    public List<GameObject> gateways;
    public Rigidbody rb;

    private enum Actions { Idle, Venturing, Defending}

    Actions current;
    Player ghost;
    
    public bool inContact = false;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        direction = transform.forward;
        velocity = direction;
        velocity.Normalize();
        speed = 0.0f;
        maxSpeed = 0.05f;
        current = Actions.Idle;
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");
        ghost = player.GetComponent<Player>();

        //Find any and all doors for the player to go to
        gateways = new List<GameObject>();
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        foreach(GameObject go in doors)
        {
            gateways.Add(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Constantly check for wisps
        GameObject[] wisps = GameObject.FindGameObjectsWithTag("Wisp");

        if (current == Actions.Idle)
        {
            speed = 0.0f;

            //If the ghost is glowing, that means it wants the AI to follow them
            if (Vector3.Distance(position, player.transform.position) <= 3f)
            {
                if(ghost.message == "Glowing")
                {
                    //Seek(player);
                    inContact = true;
                }
                else
                {
                    current = Actions.Idle;
                }
            }

            //Wisps mark a path for the AI to follow
            if (wisps.Length > 0)
            {
                float closest = Vector3.Distance(position, wisps[0].transform.position);
                List<GameObject> points = new List<GameObject>();
                foreach (GameObject go in wisps)
                {
                    float dist = Vector3.Distance(position, go.transform.position);
                    if (dist < closest)
                    {
                        closest = dist;
                        points.Add(go);
                    }
                }

                if(points.Count > 0)
                {
                    int index = 0;
                    if (Vector3.Distance(points[index].transform.position, position) <= 1f)
                    {
                        Seek(points[index]);
                        if(Vector3.Distance(position, points[index].transform.position) <= 1f)
                        {
                            if(index != points.Count - 1)
                            {
                                index++;
                            }
                        }
                    }
                }
            }

            //When the player is led near a door, then the AI should enter it
            foreach(GameObject gate in gateways)
            {
                if(Vector3.Distance(position, gate.transform.position) <= 1f)
                {
                    Seek(gate);
                }
            }
        }

        if(current == Actions.Venturing)
        {
            speed = 0.03f;
            position += velocity * speed;
            transform.position = position;
            Vector3.ClampMagnitude(velocity, maxSpeed);
            Quaternion toRotation = Quaternion.LookRotation(direction);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, speed * Time.deltaTime);
        }

        if (current == Actions.Defending)
        {

        }
    }

    void Seek(GameObject target)
    {
        if (target != null)
        {
            current = Actions.Venturing;

            velocity = target.transform.position - position;

            velocity.Normalize();
            velocity *= maxSpeed;

            //transform.rotation = Quaternion.LookRotation(transform.forward, velocity.normalized * Time.deltaTime);

            if(Vector3.Distance(position, target.transform.position) <= 0.5f)
            {
                current = Actions.Idle;
                speed = 0.0f;
                return;
            }
        }
    }
}
