using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffectSpawn : MonoBehaviour
{
    [SerializeField] PooledObject dashEffectPrefab;
    
    
    int size = 3;
    int capacity = 3;
    void Start()
    {
        Manager.Pool.CreatePool(dashEffectPrefab, size, capacity);
    }

    public void DashEffect()
    {
        Manager.Pool.GetPool(dashEffectPrefab, transform.position, transform.rotation);
    }
}
