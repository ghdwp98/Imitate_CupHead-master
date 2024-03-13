using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExBulletCollision : MonoBehaviour
{
    Animator animator;
    PooledObject pooledObject;
    void OnEnable()
    {
        animator= GetComponent<Animator>();
        animator.Play("EXShootCollision");
    }

    void Update()
    {
        
    }
}
