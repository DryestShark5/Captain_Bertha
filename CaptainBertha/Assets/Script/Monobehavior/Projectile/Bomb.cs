using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float delay = 2f;
    public float radius = 5f;
    public float force = 700f;

    public float damage;

    public GameObject explotionEffect;

    float countdown;
    bool hasExploded = false;

    EnemyAI enemy;

    private void Start()
    {
        countdown = delay;
        enemy = GameObject.Find("Enemy").GetComponent<EnemyAI>();
    }

    private void Update()
    {
        damage = enemy.damage;

        countdown -= Time.deltaTime;
        if (countdown <= 0 && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        //show effect
        Instantiate(explotionEffect, transform.position, transform.rotation);

        //Get nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders)
        {
            //Add force
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }

            //Damage
            EnemyAI AI = nearbyObject.GetComponent<EnemyAI>();
            if (AI != null)
            {
                AI.BombDmg();
            }

            Player Bertha =  nearbyObject.GetComponent<Player>();
            if (Bertha != null)
            {
                Bertha.health -= damage;
            }
        }

        //Remove bomb
        Destroy(gameObject);
    }
}
