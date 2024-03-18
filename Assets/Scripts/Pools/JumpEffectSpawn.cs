using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEffectSpawn : MonoBehaviour
{
    [SerializeField] PooledObject jumpEffectPrefab;
    /*[SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;*/

    int size = 3;
    int capacity = 3;
    private void Start()
    {
        Manager.Pool.CreatePool(jumpEffectPrefab, size, capacity);

    }

    public void JumpEffect()
    {
        Manager.Pool.GetPool(jumpEffectPrefab,transform.position,transform.rotation);
    }


}
