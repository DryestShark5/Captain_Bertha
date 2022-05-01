using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Rigidbody bulletRigitbody;
    public float speed;

    private void Awake()
    {
        bulletRigitbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bulletRigitbody.velocity = transform.forward * speed;
    }
}
