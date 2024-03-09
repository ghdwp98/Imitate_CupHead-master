using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSparkle : MonoBehaviour
{
    [SerializeField] PooledObject pooledObject;
    Animator animator;
    public float releaseTime = 0;


    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        animator.Play("WeaponSpakle");
    }

    void Update()
    {
        releaseTime += Time.deltaTime;

        if(releaseTime>0.3f)
        {
            pooledObject.Release();
            releaseTime= 0;
        }


    }
}
