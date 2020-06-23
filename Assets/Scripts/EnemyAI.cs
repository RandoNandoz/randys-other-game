using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;

enum EnemyState
{
    Invalid = -1,
    Idle = 0,
    Patrol = 1,
    Triggered = 2,
    Attacking = 3
}

public class EnemyAI : MonoBehaviour
{
    private Vector3 startPosVector3;
    private Quaternion startingRotQuaternion;
    public Transform patrolPointRootTransform;
    private List<Transform> patrolList = new List<Transform>();
    private Transform currentTransformPatrolPointTransform;
    private float reachedDist = 2f;
    public float AwarenessRange = 10f;
    public float AttackRange = 5f;
    public float attackTime = 2;
    private float attackTimer = 0;
   
    public AudioClip hitAudioClip;
    [HideInInspector]
    public Transform targetTransform;

    public float rotationSpeed;

    public int damage = 1;  

    public float movementSpeed = 10f;

    // leaks the juicy secrets of the private variable to the world ;)
    [SerializeField]
    private EnemyState myEnemyState = EnemyState.Invalid;
    // Start is called before the first frame update
    void Start()
    {
        FindPatrolPoints();
        FindPlayerTarget();
        ChangeState(EnemyState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        switch (myEnemyState)
        {
            case EnemyState.Invalid:
                break;
            case EnemyState.Idle:
                IdleUpdate();
                break;
            case EnemyState.Patrol:
                PatrolUpdate();
                break;
            case EnemyState.Triggered:
                TriggeredUpdate();
                break;
            case EnemyState.Attacking:
                AttackingUpdate();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        //if (!targetTransform)
        //{
        //    FindPlayerTarget();
        //}
        //else
        //{
        //    MoveTowardsTarget();
        //}se
    }

    void FindPlayerTarget()
    {
        targetTransform = FindObjectOfType<PlayerController>().transform;
    }

    void FindPatrolPoints()
    {
        if (!patrolPointRootTransform)
        {
         return;   
        }
        for (int i = 0; i < patrolPointRootTransform.childCount; i++)
        {
            Transform childTransform = patrolPointRootTransform.GetChild(i);
            patrolList.Add(childTransform);
        }

    }

    void MoveTowardsTarget()
    {

        // Rotate the enemy at the player.
        // transform.LookAt(targetTransform);
        Vector3 directionVector3 = targetTransform.position - transform.position;
        directionVector3.Normalize();
        float dot = Vector3.Dot(transform.right, directionVector3);
        bool isLeft = (dot < 0);
        float rotation = rotationSpeed * Time.deltaTime;
        if (isLeft)
        {
            rotation = -rotation;
        }

        if (Vector3.Distance(directionVector3,transform.forward) > 0.1f)
        {
            transform.Rotate(0, rotation,0);
        }
        else
        {
            transform.LookAt(targetTransform);
        }

        // Move towards the target. (MOVE FORWARDS YA DUMMY)
        Vector3 movementVector3 = transform.forward * movementSpeed * Time.deltaTime;
        transform.Translate(movementVector3, Space.World);
    }

    void RotateTowards(Transform pointTransform)
    {
        Vector3 directionVector3 = pointTransform.position - transform.position;
        directionVector3.Normalize();
        float dot = Vector3.Dot(transform.right, directionVector3);
        bool isLeft = (dot < 0);
        float rotation = rotationSpeed * Time.deltaTime;
        if (isLeft)
        {
            rotation = -rotation;
        }

        if (Vector3.Distance(directionVector3, transform.forward) > 0.1f)
        {
            transform.Rotate(0, rotation, 0);
        }
        else
        {
            transform.LookAt(pointTransform);
        }

        // Move towards the target. (MOVE FORWARDS YA DUMMY)
        Vector3 movementVector3 = transform.forward * movementSpeed * Time.deltaTime;
        transform.Translate(movementVector3, Space.World);
        // Ensure rot on only y axis
        Vector3 anglesVector3 = transform.eulerAngles;
        anglesVector3.x = 0;
        anglesVector3.z = 0;
        transform.eulerAngles = anglesVector3;
    }
    // State Machine handling


    private void ChangeState(EnemyState newState)
    {
        Debug.Log("new state: " + newState.ToString());
        ExitState(myEnemyState);
        myEnemyState = newState;
        EnterState(myEnemyState);
    }

    private void EnterState(EnemyState newState)
    {
        switch (newState)
        {
            case EnemyState.Invalid:
                break;
            case EnemyState.Idle:
                break;
            case EnemyState.Patrol:
                break;
            case EnemyState.Triggered:
                break;
            case EnemyState.Attacking:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    private void ExitState(EnemyState oldEnemyState)
    {
        switch (oldEnemyState)
        {
            case EnemyState.Invalid:
                break;
            case EnemyState.Idle:
                break;
            case EnemyState.Patrol:
                break;
            case EnemyState.Triggered:
                break;
            case EnemyState.Attacking:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(oldEnemyState), oldEnemyState, null);
        }
    }

    private void IdleUpdate()
    {
        if (IsTargetWithinRange(AwarenessRange) && IsTargetVisible())
        {
            ChangeState(EnemyState.Triggered);
        }
        else if (patrolList.Count > 0)
        {
            ChangeState(EnemyState.Patrol);
        }
    }

    private void PatrolUpdate()
    { 
        if (IsTargetWithinRange(AwarenessRange) && IsTargetVisible())
        {
            ChangeState(EnemyState.Triggered);
        }
        else
        {
            MoveTowardsPatrolPoint();
        }
    }

    private void TriggeredUpdate()
    {
        MoveTowardsTarget();
        if (IsTargetWithinRange(AttackRange))
        {
            ChangeState(EnemyState.Attacking);
        }
        else if (!IsTargetWithinRange(AwarenessRange + 1.0f) || !IsTargetVisible())
        {
         ChangeState(EnemyState.Idle);   
        }
        
    }
    private void AttackingUpdate()
    {
        // implement logic ya morgol
        attackTimer = Time.deltaTime;
        if (attackTimer >= attackTime)
        {
            attackTimer = 0;
            health targetHealth = targetTransform.GetComponent<health>();
            if (targetHealth)
            {
                targetHealth.actualHealth -= damage;
            }
        }
        RotateTowards(targetTransform);
        if (!IsTargetWithinRange(AttackRange + 1.0f) || !IsTargetVisible())
        {
            ChangeState(EnemyState.Triggered);
        }
    }

    private bool IsTargetWithinRange(float range)
    {
        if (Vector3.Distance(transform.position, targetTransform.position) < range)
        {
            return true;
        }
        return false;
    }

    private bool IsTargetVisible()
    {
        RaycastHit hit;
        Vector3 directionVector3 = targetTransform.position - transform.position;
        directionVector3.Normalize();
        if (Physics.Raycast(transform.position, directionVector3, out hit, AwarenessRange))
        {
            if (hit.transform == targetTransform)
            {
                return true;
            }
        }
        return false;
    }

    void MoveTowardsPatrolPoint()
    {
        if (!currentTransformPatrolPointTransform)
        {
            currentTransformPatrolPointTransform = patrolList[0];
        }
        RotateTowards(currentTransformPatrolPointTransform);
        Vector3 movementVector3 = transform.forward * movementSpeed * Time.deltaTime;
        transform.Translate(movementVector3, Space.World);
        RotateTowards(patrolPointRootTransform);
        if (Vector3.Distance(transform.position, currentTransformPatrolPointTransform.position) < reachedDist)
        {
            int index = patrolList.IndexOf((currentTransformPatrolPointTransform));
            index = (index + 1) % patrolList.Count;
            currentTransformPatrolPointTransform = patrolList[index];
        }
    }

    public void ThanosSnap()
    {
        transform.position = startPosVector3;
        transform.rotation = startingRotQuaternion;
        startPosVector3 = transform.position;
        startingRotQuaternion = transform.rotation;
        ChangeState(EnemyState.Idle);
    }

    void OnCollisionEnter(Collision otherCollision)
    {
        health health = otherCollision.gameObject.GetComponent<health>();
        if (health)
        {
            health.actualHealth -= damage;
            GameManager.instance.PlayPain(hitAudioClip);
        }

        PlayerController playerController = otherCollision.gameObject.GetComponent<PlayerController>();
        if (playerController)
        {
            playerController.PlayHitAudio();
            Destroy(gameObject);
        }
        
        //EnemyAI otherEnemyAi = otherCollision.gameObject.GetComponent<EnemyAI>();
        //if (otherEnemyAi)
        //{
        //    Destroy(gameObject);
        //    return;
        //}


    }

}
