using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //Enemy
    [Tooltip("Add a Nav Mesh agent component to your enemy, and drag it in here")]
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
    Vector3 walkPoint;
    bool walkPointSet;
    Vector3 startPos;
    [Header("AI movement")]
    [Tooltip("Set max walk distance before turning")]
    public float walkPointRange;
    [Tooltip("Set speed when in patrol state")]
    public float patrolSpeed;
    [Tooltip("Set speed when in chase state")]
    public float chaseSpeed;
    [Tooltip("Changes patrol state to idle state")]
    public bool idle;
    float timer = 10f;

    //States
    [Header("States")]
    [Tooltip("Max range the AI can follow the player")]
    public float sightRange;
    [Tooltip("Max range the AI can attack the player, if attackRange < 2 the AI is melee")]
    public float attackRange;
    bool playerInSightRange, playerInAttackRange;

    [Header("Health")]
    [Tooltip("Set max health of enemy")]
    public float health;

    //Attacking
    [Header("Attack")]
    [Tooltip("Set attackspeed")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    [Tooltip("Drag in a prefab to call on when attacking")]
    public GameObject projectile;
    [Tooltip("Drag in a gameobject without collider where you want bullet to spawn")]
    public GameObject firePoint;
    [Tooltip("Set speed for prefab when shot")]
    public float bulletForce;
    [Tooltip("Set damage the enemy deals")]
    public float damage;
    
    [Tooltip("Set height for the shot")]
    public float upForce;

    private void Awake()
    {
        player = GameObject.Find("Bertha").transform;
        agent = GetComponent<NavMeshAgent>();
        startPos = this.transform.position;
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

        if (health <= 0) Destroy(gameObject);


    }

    void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (idle)
            agent.SetDestination(startPos);
        else if (!idle)
            agent.SetDestination(walkPoint);
            agent.speed = patrolSpeed;


        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walk point reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            timer = 10;
        }
        
        if (distanceToWalkPoint.magnitude > 1f)
        {
            timer -= Time.deltaTime;
            if(timer < 0f)
            {
                walkPointSet = false;
                timer = 10f;
            }
        }

    }

    void SearchWalkPoint()
    {
        //Calcualte random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
            Debug.Log("Check");
        }
            
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        agent.speed = chaseSpeed;
    }

    void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack code
            Rigidbody rb = Instantiate(projectile, firePoint.transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * bulletForce, ForceMode.Impulse);
            rb.AddForce(transform.up * upForce, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
