using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryCheck : MonoBehaviour
{
    public bool isParryed=false;
    Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Checking" ||
            collision.tag == "Parry")
        {
            return; //do nothing
        }

        isParryed = true;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Checking"
            || collision.tag == "Parry")
        {
            return; //do nothing
        }

        isParryed = false;

    }

    public void ParryAniPlay()
    {
        animator.Play("ParrySpark");
    }


    private void Update()
    {
        
    }


}
