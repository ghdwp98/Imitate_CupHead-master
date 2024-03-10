using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : MonoBehaviour
{
    Animator animator;
    [SerializeField] PooledObject dashEffect;
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        animator.Play("Dash");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
