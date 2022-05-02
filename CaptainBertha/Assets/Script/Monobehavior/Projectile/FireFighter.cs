using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFighter : MonoBehaviour
{
    float timeToBurn = 10f;

    private void Start()
    {
        Destroy(gameObject, timeToBurn);
    }

}
