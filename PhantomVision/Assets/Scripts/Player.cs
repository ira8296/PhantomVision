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

    Vector3 screenPosition;
    Vector3 worldPosition;
    Vector3 holdPosition;

    public Camera mainCam;
    public GameObject wisp;
    int maxWisps = 3;

    LayerMask ground;

    GameObject heldObj = null;



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
        }
        if (Input.GetKey(KeyCode.S))
        {
            position.z -= speed;
            holdPosition.z -= speed;
            direction.z = -1f; ;
        }
        if (Input.GetKey(KeyCode.D))
        {
            position.x += speed;
            holdPosition.x += speed;
            direction.x = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            position.x -= speed;
            holdPosition.x -= speed;
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

        //Leave wisp
        if (Input.GetKeyDown(KeyCode.X) && Input.GetKeyDown(KeyCode.LeftShift))
        {
            GameObject[] wisps = GameObject.FindGameObjectsWithTag("Wisp");
            if(wisps.Length > maxWisps)
            {
                foreach(GameObject w in wisps)
                {
                    GameObject.Destroy(w);
                }
            }

            GameObject.Instantiate(wisp, transform.position, Quaternion.identity);
        }

        Float();
        Glow();
        Levitate();

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

    void Float() //Lets player slowly rise into the air
    {
        if (floating)
        {
            position.y += speed;
            holdPosition.y += speed;
        }
        else
        {
            while(position.y > 1.5)
            {
                position.y -= (speed * -10);
                holdPosition.y -= (speed * -10);
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
        }
    }

    void Glow() //Allows player to illuminate themselves
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
