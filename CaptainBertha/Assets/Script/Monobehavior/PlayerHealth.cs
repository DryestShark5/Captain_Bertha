using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealt;
    public float maxHealth;
    public Animator anim;

    public Slider healthBar;
    // Start is called before the first frame update
    void Start()
    {
        currentHealt = maxHealth;
        healthBar.value = currentHealt;
        healthBar.maxValue = maxHealth;

        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //float hit = anim.GetFloat("hit");

        //if(hit > 0)
        {
          //  hit -= Time.deltaTime * 3;
            //anim.SetFloat("hit", hit);
        }
        //if(currentHealt < 1)
        {
            //anim.SetBool("death", true);
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            SendDamage(Random.Range(10, 20));
        }
    }
    public void SendDamage (float damageValue)
    {
        currentHealt -= damageValue;
        healthBar.value = currentHealt;
            //anim.SetFloat("hit", 1);
    }
}
