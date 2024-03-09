using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEffect : MonoBehaviour
{
    Animator animator;
    [SerializeField] PooledObject jumpEffect;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        animator.Play("Jump");
    }


    void Update()
    {
        
    }
}
