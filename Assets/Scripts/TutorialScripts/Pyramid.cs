using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyramid : MonoBehaviour
{
    [SerializeField] Targets targets;
    Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Bullet")
        {
            Collider2D collider = GetComponent<BoxCollider2D>();
            collider.enabled = false;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            Collider2D collider = GetComponent<BoxCollider2D>();
            collider.enabled = false;

        }
    }

    private void Update()
    {
        
    }


}
