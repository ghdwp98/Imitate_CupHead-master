using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    Animator animator;
    bool isTrigger = false;
    void Start()
    {
        animator=GetComponent<Animator>();
    }

    void Update()
    {
        if (isTrigger==false&&animator.GetCurrentAnimatorStateInfo(0).IsName("FlagSpawn") &&
        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            animator.Play("FlagIdle");
            isTrigger = true;
        }


    }
}
