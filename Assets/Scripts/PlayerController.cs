using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody myRB;

    // public GameObject shotSpot;
    // public GameObject projectilePrefab;

    public float moveSpeed = 5.0f;
    public float rotSpeed = 10.0f;
    private Vector3 lastMousePos = Vector3.zero;

    public float forwardVelLimit = 50.0f;
    public float backwardVelLimit = -25.0f;
    public float sidewaysVelLimit = 45.0f;

    public AudioClip hitAudioClip;
    private AudioSource myAudioSource;

    // public GameObject checkpointContainer;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
        health healthHealth = GetComponent<health>();
        if (healthHealth)
        {
            healthHealth.respawnCallBack = GameManager.instance.ResetLevel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckPerformMovement();
        // CheckPerformRotation_Keyboard();
        CheckPerformRotation_Mouse();
        // CheckPerformShoot();
        // CheckHealth();
        CheckPerformRotationMousePointer();
    }

    void CheckPerformMovement()
    {
        Vector3 inputVect = Vector3.zero;
        //bool moveForwards = false;
        //bool moveBackwards = false;
        // Check for user input
        if (Input.GetKey(KeyCode.W))
        {
            inputVect.z += 1.0f;
            //moveForwards = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVect.z -= 1.0f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVect.x -= 1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVect.x += 1.0f;
        }
        // Perform force-based movement accordingly
        Vector3 forwardsMovement = transform.forward * inputVect.z;
        Vector3 sidewaysMovement = transform.right * inputVect.x;
        Vector3 movement = forwardsMovement + sidewaysMovement;
        //inputVect.x *= transform.forward.x;
        //inputVect.z *= transform.forward.z;

        movement *= (moveSpeed * Time.deltaTime);

        myRB.AddForce(movement, ForceMode.Acceleration);

        // Clamping the velocity within a specific range
        // This prevents the player from constantly getting faster
        //Debug.Log("Velocity Z: " + myRB.velocity.z);
        float clampedForwards = 
            Mathf.Clamp(myRB.velocity.z, backwardVelLimit, forwardVelLimit);

        float clampedSideways = 
            Mathf.Clamp(myRB.velocity.x, -sidewaysVelLimit, sidewaysVelLimit);

        Vector3 clampedVelocity = 
            new Vector3(clampedSideways, myRB.velocity.y, clampedForwards);

        myRB.velocity = clampedVelocity;
    }

    void CheckPerformRotation_Keyboard()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(transform.up, -rotSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(transform.up, rotSpeed * Time.deltaTime);
        }
    }

    void CheckPerformRotation_Mouse()
    {
        if (lastMousePos == Vector3.zero)
        {
            lastMousePos = Input.mousePosition;
            return;
        }

        Vector3 mouseDelta = Input.mousePosition - lastMousePos;
        float rotAmt = mouseDelta.x * rotSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotAmt, Space.World);
        lastMousePos = Input.mousePosition;
    }

    void CheckPerformRotationMousePointer()
    {
        Vector3 inputPointVector3 = Input.mousePosition;
        // inputPointVector3.x = Camera.main.pixelWidth - inputPointVector3.x;
        inputPointVector3.z = 50;
        // inputPointVector3.y = Camera.main.pixelHeight - inputPointVector3.y;
        Vector3 worldPointVector3 = Camera.main.ScreenToWorldPoint(inputPointVector3);
        transform.LookAt(worldPointVector3);
        Vector3 anglesVector3 = transform.eulerAngles;
        anglesVector3.x = 0.0f;
        anglesVector3.z = 0.0f;
        transform.eulerAngles = anglesVector3;
    }

    public void PlayHitAudio()
    {
        // GameManager.instance.PlayPain(hitAudioClip);
        //if (!myAudioSource.isPlaying)
        //{
        //    myAudioSource.clip = hitAudioClip;
        //    myAudioSource.Play();
        //}
    }
    //void CheckPerformShoot()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        // Instantiate(projectilePrefab, shotSpot.transform.position, transform.rotation);
    //    }
    //}

     /*void CheckHealth()
    {
        if (health <= 0)
        {
            transform.position = respawnPos;
            health = maxHealth;
        }
    }

    void DoDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Oh no! The Player died!");
        }
    } */

    /* public void SetCheckpoint(Checkpoint checkpoint)
    {
        if (checkpoint.isInitial && respawnPos != Vector3.zero)
        {
            return;
        }

        respawnPos = checkpoint.spawnPos.position;
        checkpoint.isActive = true;

        Checkpoint[] checkpoints = 
            checkpointContainer.GetComponentsInChildren<Checkpoint>();

        for (int ii = 0; ii < checkpoints.Length; ii++)
        {
            if (checkpoints[ii] == checkpoint)
            {
                continue;
            }
            checkpoints[ii].isActive = false;
        }
    }*/

    ////////////////////////////////////////////////////////
    // UNITY EVENTS
    ////////////////////////////////////////////////////////
    //void OnCollisionEnter(Collision other)
    //{
    //    Hazard hazard = other.gameObject.GetComponent<Hazard>();
    //    if (hazard != null)
    //    {
    //        //health -= hazard.damage;
    //        DoDamage(hazard.damage);
    //        Destroy(hazard.gameObject);
    //    }
    //}

}
