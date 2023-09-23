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
    List<GameObject> unphasables; //objects the player cannot phase through

    Vector3 screenPosition;
    Vector3 worldPosition;
    Vector3 holdPosition;

    float minX = 0f;
    float maxX = 0f;
    float minZ = 0f;
    float maxZ = 0f;

    public Camera mainCam;
    public GameObject wisp;
    int maxWisps = 3;

    LayerMask ground;

    GameObject heldObj = null;

    public string message = "Idle";



    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        direction = transform.forward;
        holdPosition = transform.GetChild(0).transform.position; 
        speed = 0.01f;
        tangible = true;
        glow = false;
        floating = false;

        //Find and record all interactable items in the environment
        /*interactables = new List<GameObject>();
        GameObject[] elements = GameObject.FindGameObjectsWithTag("Interactable");
        foreach (GameObject element in elements)
        {
            interactables.Add(element);
        }*/

        //Find and record all interactable items in the environment
        /*collectibles = new List<GameObject>();
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Collectible");
        foreach (GameObject pickup in pickups)
        {
            interactables.Add(pickup);
        }*/

        //Find and record all movable items in the environment
        movables = new List<GameObject>();
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Movable");
        foreach (GameObject projectile in projectiles)
        {
            movables.Add(projectile);
        }

        //Find and record all unphasable objects in the environment
        unphasables = new List<GameObject>();
        GameObject[] boundaries = GameObject.FindGameObjectsWithTag("Unphasable");
        foreach(GameObject boundary in boundaries)
        {
            unphasables.Add(boundary);
        }

        foreach (GameObject u in unphasables)
        {
            if (u.transform.position.x < minX)
            {
                minX = u.transform.position.x;
            }
            if(u.transform.position.x > maxX)
            {
                maxX = u.transform.position.x;
            }
            if(u.transform.position.z < minZ)
            {
                minZ = u.transform.position.z;
            }
            if(u.transform.position.z > maxZ)
            {
                maxZ = u.transform.position.z;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ground = LayerMask.GetMask("Ground");

        //Movement input
        if (Input.GetKey(KeyCode.W))
        {
            position.z += speed;
            holdPosition.z += speed;
            direction.z = 1f;
            message = "Moving";
        }
        if (Input.GetKey(KeyCode.S))
        {
            position.z -= speed;
            holdPosition.z -= speed;
            direction.z = -1f; ;
            message = "Moving";
        }
        if (Input.GetKey(KeyCode.D))
        {
            position.x += speed;
            holdPosition.x += speed;
            direction.x = 1f;
            message = "Moving";
        }
        if (Input.GetKey(KeyCode.A))
        {
            position.x -= speed;
            holdPosition.x -= speed;
            direction.x = -1f;
            message = "Moving";
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

        //Leave wisp
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            GameObject[] wisps = GameObject.FindGameObjectsWithTag("Wisp");
            if(wisps != null)
            {
                if (wisps.Length > maxWisps - 1)
                {
                    foreach (GameObject w in wisps)
                    {
                        GameObject.Destroy(w);
                    }
                }
            }

            GameObject.Instantiate(wisp, transform.position, Quaternion.identity);
            message = "Lighting";
        }

        //Check for boundaries
        if(transform.position.x < minX)
        {
            position.x = minX;
        }
        if(transform.position.x > maxX)
        {
            position.x = maxX;
        }
        if(transform.position.z < minZ)
        {
            position.z = minZ;
        }
        if(transform.position.z > maxZ)
        {
            position.z = maxZ;
        }

        Float();
        Glow();
        Levitate();

        transform.forward = direction;
        transform.position = position;
        transform.Rotate(direction);
    }

    void Float() //Lets player slowly rise into the air
    {
        if (floating)
        {
            position.y += speed;
            holdPosition.y += speed;
            message = "Floating";
        }
        else
        {
            while(position.y > 1.5)
            {
                position.y -= speed;
                holdPosition.y -= speed;
            }
        }

    }

    void Levitate() //Allows player to lift up movable objects they click on
    {
        //Mouse position logic
        screenPosition = Input.mousePosition;

        Ray ray = mainCam.ScreenPointToRay(screenPosition);
        RaycastHit hitData;

        if (Physics.Raycast(ray, out hitData, Mathf.Infinity) && Input.GetMouseButton(0))
        {
            worldPosition = hitData.point;

            foreach (GameObject m in movables)
            {
                if (m == hitData.transform.gameObject || m.transform.position == holdPosition)
                {
                    heldObj = m;
                }

            }
        }
        else
        {
            heldObj = null;
        }

        if(heldObj != null)
        {
            heldObj.transform.position = holdPosition;
            message = "Levitating";
        }
    }

    void Glow() //Allows player to illuminate themselves
    {
        if (glow)
        {
            this.GetComponent<Light>().enabled = true;
            message = "Glowing";
        }
        else
        {
            this.GetComponent<Light>().enabled = false;
        }
    }
}
