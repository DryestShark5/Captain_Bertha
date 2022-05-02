using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeProjectile : MonoBehaviour
{
    Player healthScript;
    EnemyAI enemy;

    private void Start()
    {
        healthScript = GameObject.Find("Bertha").GetComponent<Player>();
        enemy = GameObject.Find("Enemy Melee").GetComponent<EnemyAI>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

        if (collision.gameObject.tag == ("Player"))
        {
            healthScript.health -= enemy.damage;
            Destroy(gameObject);
        }
    }
}
