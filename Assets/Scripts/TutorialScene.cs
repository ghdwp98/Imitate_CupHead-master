using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScene : BaseScene
{
    [SerializeField] PooledObject bulletPrefab;
    [SerializeField] PooledObject SparklePrefab;
    [SerializeField] PooledObject ExplosionPrefab;
    [SerializeField] PooledObject jumpEffectPrefab;
    [SerializeField] int size = 20;
    [SerializeField] int capacity = 30;
    [SerializeField] int jumpSize = 3;
    [SerializeField] int jumpCapacity = 3;

    private void Update()
    {
        // 
    }




    public override IEnumerator LoadingRoutine()
    {
        /*Manager.Pool.CreatePool(bulletPrefab, size, capacity);
        Manager.Pool.CreatePool(SparklePrefab, size, capacity);
        Manager.Pool.CreatePool(ExplosionPrefab, size, capacity);
        Manager.Pool.CreatePool(jumpEffectPrefab, jumpSize, jumpCapacity);*/

        // 잠깐 꺼두기 
        yield return null;


    }
}
