using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAnimationTest : MonoBehaviour
{
    // Start is called before the first frame update
    Animator ani;
    void Start()
    {
        ani = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ani.SetBool("isAttacking", false);
            ani.SetBool("isWalking", true);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ani.SetBool("isAttacking", true);
            ani.SetBool("isWalking", false);
        }
    }
        
}
