using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField] PooledObject pooledObject;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Animator animator;
    [SerializeField] float releaseTime = 0f;
    [SerializeField] AudioClip clip;

    private void OnEnable()
    {
        audioSource=GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        animator.Play("WeaponCollision");
       /* audioSource.clip = clip;
        audioSource.Play();*/
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
