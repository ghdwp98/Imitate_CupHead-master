using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : MonoBehaviour
{
    Animator animator;
    [SerializeField] PooledObject dashEffect;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioSource source;
    private void OnEnable()
    {
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        animator.Play("Dash");
        source.clip = clip;
        source.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
