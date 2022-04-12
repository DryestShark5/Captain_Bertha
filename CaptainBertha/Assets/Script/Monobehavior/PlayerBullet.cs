using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Rigidbody bulletRigitbody;
    public float speed;
    Player bertha;
    EnemyAI enemy;

    private void Awake()
    {
        bulletRigitbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bulletRigitbody.velocity = transform.forward * speed;
        bertha = GameObject.Find("Bertha").GetComponent<Player>();
        enemy = GameObject.Find("Enemy").GetComponent<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Enemy"))
        {
            enemy.health -= bertha.damage;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
