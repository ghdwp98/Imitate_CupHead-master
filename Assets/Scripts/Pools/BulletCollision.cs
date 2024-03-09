using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField] PooledObject pooledObject;
    [SerializeField] Animator animator;
    [SerializeField] float releaseTime = 0f;

    private void OnEnable()
    {
        animator=GetComponent<Animator>();
        animator.Play("WeaponCollision");
    }

    void Update()
    {
        releaseTime += Time.deltaTime;

        if (releaseTime > 0.3f)
        {
            pooledObject.Release();
            releaseTime = 0;
        }
    }
}
