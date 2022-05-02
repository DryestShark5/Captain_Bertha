using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Rigidbody bulletRigitbody;
    public float speed;
    float maxRange = 1f;

    private void Awake()
    {
        bulletRigitbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bulletRigitbody.velocity = transform.forward * speed;
    }

    private void Update()
    {
        maxRange -= Time.deltaTime;
        if (maxRange <= 0)
        {
            Destroy(gameObject);
        }
    }
}
