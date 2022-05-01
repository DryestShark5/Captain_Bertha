using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPLookAtPlayer : MonoBehaviour
{
    public Transform cam;

    private void LateUpdate()
    {
        transform.LookAt(cam);
    }
}
