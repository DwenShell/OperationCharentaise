using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class S_playerChaser : MonoBehaviour
{
    public bool isPlayerVisible;  
    public bool isPlayerSoundable; 

    public Transform player;     
    private Vector3 playerPosition;
    public NavMeshAgent agent;

    public float stopChasingDistance = 2f;  
    public float slowDownDuration = 20f;    
    public float slowSpeedMultiplier = 0.5f;

    public bool isSlowingDown = false;      
    private float originalSpeed;

    private bool isChasing = false;         

    public bool hasFixedPosition = false;   
    public Vector3 fixedPosition;           
    public Vector3 wanderingCenter;         
    public float wanderingRadius = 10f;     
    public float idleWaitTime = 5f;         

    private Coroutine wanderingCoroutine;   

    void Start()
    {
        originalSpeed = agent.speed; 

        if (hasFixedPosition)
        {
            wanderingCenter = fixedPosition;
        }
    }

    void Update()
    {
        if (player != null && isPlayerSoundable)
        {
            playerPosition = player.position;
            float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

            if (distanceToPlayer > stopChasingDistance)
            {
                agent.SetDestination(playerPosition); 
                isChasing = true;

                if (wanderingCoroutine != null)
                {
                    StopCoroutine(wanderingCoroutine);
                    wanderingCoroutine = null;
                }
            }
            else if (!isSlowingDown)
            {
                StartCoroutine(SlowDownBeforeStopping());
            }
        }
        else if (!isSlowingDown)
        {
            StartCoroutine(SlowDownBeforeStopping());
        }
    }

    private IEnumerator SlowDownBeforeStopping()
    {
        isSlowingDown = true;

        agent.speed = originalSpeed * slowSpeedMultiplier;

        yield return new WaitForSeconds(slowDownDuration);

        agent.speed = originalSpeed;
        agent.ResetPath();

        isSlowingDown = false;
        isChasing = false;

        if (wanderingCoroutine == null)
        {
            wanderingCoroutine = StartCoroutine(Wander());
        }
    }

    private IEnumerator Wander()
    {
        while (!isChasing) 
        {
            Vector3 destination;

            if (hasFixedPosition)
            {
                destination = fixedPosition;
            }
            else
            {
                destination = wanderingCenter + Random.insideUnitSphere * wanderingRadius;
                destination.y = transform.position.y;
            }

            agent.SetDestination(destination); 

            yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);

            yield return new WaitForSeconds(idleWaitTime);
        }
    }
}
