using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEffect : MonoBehaviour
{
    Animator animator;
    [SerializeField] PooledObject jumpEffect;
    [SerializeField] AudioSource jumpSource;
    [SerializeField] AudioClip jumpClip;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        animator.Play("Jump");
        jumpSource= GetComponent<AudioSource>();
        jumpSource.clip = jumpClip;
        jumpSource.PlayOneShot(jumpClip);
    }


    void Update()
    {
        
    }
}
