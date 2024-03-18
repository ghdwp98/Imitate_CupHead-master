using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrySpark : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Destroy(gameObject,0.25f);
        audioSource.clip = clip;
        audioSource.PlayOneShot(clip);
    }

    
    void Update()
    {
        
    }
}
