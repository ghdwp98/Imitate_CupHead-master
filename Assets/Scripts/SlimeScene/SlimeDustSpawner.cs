using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDustSpawner : MonoBehaviour
{
    [SerializeField] PooledObject dustPrefab;

    [SerializeField] GameObject TombFallDust;
    [SerializeField] GameObject TombAttackDust;
    
    [SerializeField] int size = 3;
    [SerializeField] int capacity = 3;

    void Start()
    {
        Manager.Pool.CreatePool(dustPrefab, size, capacity);
        
    }

    void Update()
    {
        
    }


    public void BigSlimeDustSpawn()
    {
        Manager.Pool.GetPool(dustPrefab, transform.position, transform.rotation);
    }

    
    public void TombFallSpawn()
    {
        GameObject fallDust= Instantiate(TombFallDust,transform.position, Quaternion.identity);

        Destroy(fallDust, 0.5f);
    }

    public void TombAttackSpawn()
    {

    }

    public void TombMoveSpawn()

    {

    }

}
