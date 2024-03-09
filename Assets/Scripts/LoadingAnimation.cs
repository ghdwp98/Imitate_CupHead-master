using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void AnimationPlay()
    {
        animator.Play("");
    }

}
