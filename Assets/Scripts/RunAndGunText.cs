using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAndGunText : MonoBehaviour
{
    Animator animator;
    AudioSource audioSource;
    [SerializeField]AudioClip Ready;
    [SerializeField]AudioClip go;
    void Start()
    {
        animator= GetComponent<Animator>();
        audioSource= GetComponent<AudioSource>();
        audioSource.PlayOneShot(Ready);
        StartCoroutine(SoundCoroutine());
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("RunAndGun") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(gameObject);
        }

        
    }

    IEnumerator SoundCoroutine()
    {
        
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(go);
    }
}
