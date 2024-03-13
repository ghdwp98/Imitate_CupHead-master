using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMove : IParry
{
    Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Destroy(gameObject, 8f);
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Ghost") == true)
        {
            rb.velocity = Vector2.up * Time.deltaTime * 220f;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
            
        

    }

    public override void Parried()
    {
        Debug.Log("ÆÐ¸®µå ");
        base.Parried();
        animator.Play("Revive");
        Destroy(gameObject, 1.2f);

    }
}
