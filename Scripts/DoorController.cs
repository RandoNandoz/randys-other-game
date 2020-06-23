using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject door;
    public float doorOpenHeight;

    public bool isLocked = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isLocked)
        {
            door.transform.Translate(0.0f, doorOpenHeight, 0.0f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && !isLocked)
        {
            door.transform.Translate(0.0f, -doorOpenHeight, 0.0f);
        }
    }
}
