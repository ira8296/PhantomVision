using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 position;
    public Vector3 direction;
    public float speed;

    public bool tangible;
    public bool glow;
    public bool floating;

    List<GameObject> interactables; //objects which the player can interact with
    List<GameObject> collectibles; //objects the player can collect
    List<GameObject> movables; //objects the player can telekinetically move



    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        direction = transform.forward;
        speed = 0.01f;
        tangible = true;
        glow = false;
        floating = false;

        //Find and record all interactable items in the environment
        interactables = new List<GameObject>();
        GameObject[] elements = GameObject.FindGameObjectsWithTag("Interactable");
        foreach (GameObject element in elements)
        {
            interactables.Add(element);
        }

        //Find and record all interactable items in the environment
        collectibles = new List<GameObject>();
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Collectible");
        foreach (GameObject pickup in pickups)
        {
            interactables.Add(pickup);
        }

        //Find and record all movable items in the environment
        movables = new List<GameObject>();
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Movable");
        foreach (GameObject projectile in projectiles)
        {
            movables.Add(projectile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Movement input
        if (Input.GetKey(KeyCode.W))
        {
            position.z += speed;
            direction.z = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            position.z -= speed;
            direction.z = -1f; ;
        }
        if (Input.GetKey(KeyCode.D))
        {
            position.x += speed;
            direction.x = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            position.x -= speed;
            direction.x = -1f;
        }

        //Floating logic
        if (Input.GetKey(KeyCode.Space))
        {
            floating = true;
        }
        else
        {
            floating = false;
        }

        //Illumination
        if (Input.GetKeyDown(KeyCode.X))
        {
            if(glow == false)
            {
                glow = true;
            }
            else
            {
                glow = false;
            }
        }

        Float();
        Glow();

        transform.forward = direction;
        transform.position = position;
    }

    /*void Move(string input)
    {
        if(input == "W")
        {
            position.z += speed;
            direction.z = 1f;
        }
        if(input == "S")
        {
            position.z -= speed;
            direction.z = -1f;
        }
        if(input == "A")
        {
            position.x -= speed;
            direction.x = -1f;
        }
        if(input == "D")
        {
            position.x += speed;
            direction.x = 1f;
        }

    }*/

    void Float()
    {
        if (floating)
        {
            position.y += speed;
        }
        else
        {
            while(position.y > 1.5)
            {
                position.y -= speed;
            }
        }

    }

    void Levitate()
    {

    }

    void Phase()
    {

    }

    void Glow()
    {
        if (glow)
        {
            this.GetComponent<Light>().enabled = true;
        }
        else
        {
            this.GetComponent<Light>().enabled = false;
        }
    }
}
