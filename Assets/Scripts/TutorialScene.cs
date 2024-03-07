using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScene : BaseScene
{
    [SerializeField] PooledObject bulletPrefab;
    [SerializeField] PooledObject SparklePrefab;
    [SerializeField] PooledObject ExplosionPrefab;

    [SerializeField] int size = 20;
    [SerializeField] int capacity = 30;

    void Start()
    {
        Manager.Pool.CreatePool(bulletPrefab, size, capacity);
        
    }

  
    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
}
