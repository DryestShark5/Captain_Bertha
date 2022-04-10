using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //Enemy
    [Tooltip("Drag in the enemy's Nav Mesh")]
    public NavMeshAgent agent;
    [Space(5)]

    //Layers
    [Tooltip("Drag in the player character")]
    public Transform player;
    [Header("Layers")]
    [Tooltip("Select the ground layer")]
    public LayerMask whatIsGround; 
    [Tooltip("Select the player layer")]
    public LayerMask whatIsPlayer;

    //Patroling
    Vector3 walkPiont;
    bool walkPointSet;
    [Header("AI movement")]
    [Tooltip("Set max walk distance before turning")]
    public float walkPointRange;

    //States
    [Header("States")]
    [Tooltip("Max range the AI can follow the player")]
    public float sightRange;
    [Tooltip("Max range the AI can attack the player")]
    public float attackRange;
    bool playerInSightRange, playerInAttackRange;
    
    //Attacking
    [Header("Attack")]
    [Tooltip("Set attackspeed")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;



    private void Awake()
    {
        player = GameObject.Find("Bertha").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //Setting state
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPiont);

        Vector3 distanceToWalkPoint = transform.position - walkPiont;

        //walk point reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    void SearchWalkPoint()
    {
        //Calcualte random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPiont = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.y + randomZ);

        if (Physics.Raycast(walkPiont, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack code here



            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
