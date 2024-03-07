using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScene : BaseScene
{
    [SerializeField] PooledObject bulletPrefab;

    void Start()
    {
        Manager.Pool.CreatePool(bulletPrefab, 20, 30);
    }

  
    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
}
