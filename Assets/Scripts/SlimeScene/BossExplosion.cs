using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExplosion : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        Destroy(gameObject, 0.5f);
    }

    void Update()
    {
        


    }
}
