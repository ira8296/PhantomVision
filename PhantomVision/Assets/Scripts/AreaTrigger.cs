using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public string area;
    public string message;

    public int playerCount;

    // Start is called before the first frame update
    void Start()
    {
        message = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            message = "Player has entered";
        }
    }
}
