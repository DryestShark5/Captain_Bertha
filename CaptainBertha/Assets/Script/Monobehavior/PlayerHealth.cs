using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float damage;


    private void Update()
    {
        if (health <= 0) Destroy(gameObject);
    }
}
