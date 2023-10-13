using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    public Vector3 position;
    public Vector3 direction;
    public float speed;
    public Vector3 velocity;

    Vector3 shakeDir;
    float amplitude;
    float frequency;
    public Rigidbody rb;

    public bool tangible;
    public bool glow;

    List<GameObject> interactables; //objects which the player can interact with
    List<GameObject> collectibles; //objects the player can collect
    List<GameObject> movables; //objects the player can telekinetically move
    List<GameObject> unphasables; //objects the player can't phase through

    Vector3 screenPosition;
    Vector3 worldPosition;
    Vector3 holdPosition;

    float maxHeight;
    float maxSpeed;

    public Camera mainCam;
    Transform camTrans;
    public GameObject wisp;
    int maxWisps = 3;

    LayerMask ground;

    GameObject heldObj = null;

    public string message = "Idle";
    public bool moving = false;

    public enum Direction { Forward, Backward, Left, Right, Up, Down };

    public Direction vector;
    public Direction lastVec;



    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        direction = mainCam.transform.forward;
        shakeDir = transform.forward;
        holdPosition = transform.GetChild(0).transform.position; 
        camTrans = mainCam.transform;
        speed = 0.0f;
        amplitude = 0.1f;
        frequency = 1f;
        vector = Direction.Forward;
        tangible = true;
        glow = false;
        maxHeight = 0.0f;
        maxSpeed = 1f;

        rb = GetComponent<Rigidbody>();
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        

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

        //Find and record all unphasable objects in environment
        unphasables = new List<GameObject>();
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Unphasable");
        foreach(GameObject obstacle in obstacles)
        {
            unphasables.Add(obstacle);
        }

        //Find max height
        foreach(GameObject obstacle in unphasables)
        {
            if(obstacle.transform.position.y > maxHeight)
            {
                maxHeight = obstacle.transform.position.y;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ground = LayerMask.GetMask("Ground");

        //Inputs

        /*Vector3 movement = Vector3.zero;

        float horInput = Input.GetAxis("Horizontal") * speed;
        float verInput = Input.GetAxis("Vertical") * speed;
        
        //Camera direction
        Vector3 camForward;
        Vector3 camRight;

        camForward.y = 0;
        camRight.y = 0;

        //Creating relative camera direction
        if(horInput != 0 || verInput != 0)
        {
            camRight = camTrans.right;
            camForward = Vector3.Cross(camRight, Vector3.up);
            movement = (camRight * horInput) + (camForward * verInput);
            movement *= speed;
            movement = Vector3.ClampMagnitude(movement, speed);

            direction = movement;
        }

        movement *= Time.deltaTime;*/


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

        //Leave wisp - can only leave 3 at a time
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
        /*if(transform.position.x < minX)
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
        }*/

        Float();
        Glow();
        Levitate();

        transform.forward = direction;
        //rb.MovePosition(position);
        Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        rb.AddForce(rb.velocity);
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void Float() //Lets player slowly rise into the air
    {
        //Movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 forward = mainCam.transform.forward;
        Vector3 right = mainCam.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 forwardRelativeVerticalInput = verticalInput * forward;
        Vector3 rightRelativeVerticalInput = horizontalInput * right;

        Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeVerticalInput;

        //Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);

        if (Input.GetKey(KeyCode.W))
        {
            speed = 0.1f;
            position.z += speed;
            holdPosition.z += speed;
            direction.z = 1f;
            message = "Moving";
            moving = true;
            vector = Direction.Forward;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            speed = 0.1f;
            position.z -= speed;
            holdPosition.z -= speed;
            direction.z = -1f; ;
            message = "Moving";
            moving = true;
            vector = Direction.Backward;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            speed = 0.1f;
            position.x += speed;
            holdPosition.x += speed;
            direction.x = 1f;
            message = "Moving";
            moving = true;
            vector = Direction.Right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            speed = 0.1f;
            position.x -= speed;
            holdPosition.x -= speed;
            direction.x = -1f;
            message = "Moving";
            moving = true;
            vector = Direction.Left;
        }
        else if (Input.GetKey(KeyCode.T))
        {
            if(position.y < maxHeight)
            {
                speed = 0.1f;
                position.y += speed;
                holdPosition.y += speed;
                //direction.y = 1f;
                message = "Floating";
                moving = true;
                vector = Direction.Up;
            }
        }
        else if(Input.GetKey(KeyCode.G))
        {
            if(position.y > 1.5)
            {
                speed = 0.1f;
                position.y -= speed;
                holdPosition.y -= speed;
                //direction.y = -1f;
                message = "Descending";
                moving = true;
                vector = Direction.Down;
            }
        }
        else
        {
            moving = false;
            speed = 0.0f;
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            lastVec = vector;
        }

        if (message == "Moving")
        {
            transform.Translate(cameraRelativeMovement * (speed * 2), Space.World);
            //rb.MovePosition(position);
        }
        else if (message == "Floating")
        {
            transform.Translate(new Vector3(0, speed, 0));
        }
        else if(message == "Descending")
        {
            transform.Translate(new Vector3(0, -speed, 0));
        }
    }

    void Levitate() //Allows player to lift up movable objects they click on
    {
        //Mouse position logic
        screenPosition = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCam.ScreenPointToRay(screenPosition);
            RaycastHit hitData;

            if (Physics.Raycast(ray, out hitData, Mathf.Infinity))
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

            if (heldObj != null)
            {
                heldObj.transform.position = holdPosition;
                message = "Levitating";
            }
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

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        if(other.gameObject.tag != "AreaTrigger")
        {
            if(other.gameObject.tag != "Unphasable" && other.gameObject.tag != "Wisp")
            {
                speed = speed * 0.5f;
                transform.position = position + shakeDir * Mathf.Sin(frequency * Time.fixedDeltaTime) * amplitude;
                GetComponent<AudioSource>().Play();
            }
            else if(other.gameObject.tag == "Unphasable")
            {
                
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        speed = 0.1f;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Unphasable")
        {
            rb.Sleep();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        
    }
}
