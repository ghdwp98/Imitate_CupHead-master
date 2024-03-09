using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    //스포너의 생성 위치를 플레이어가 결정하는게 더 좋은 방법 
    //그래도 render 같은 상태는 여기서 판단하는게?? 

    [SerializeField] PlayerController player;
    [SerializeField] PooledObject bulletPrefab;
    [SerializeField] PooledObject SparklePrefab;
    [SerializeField] PooledObject ExplosionPrefab;

    [SerializeField] int size = 20;
    [SerializeField] int capacity = 30;
    

    void Start()
    {

        Manager.Pool.CreatePool(bulletPrefab, size, capacity);
        Manager.Pool.CreatePool(SparklePrefab, size, capacity);
        Manager.Pool.CreatePool(ExplosionPrefab, size, capacity);
    }
    void Update()
    { 
        
    }

    public void ObjectSpawn() //소환하는 위치가 자신의 위치니까 플레이어의 상태에 따라서 그 위치를 바꿔주자.
    {
        Manager.Pool.GetPool(bulletPrefab, transform.position, transform.rotation);
        Manager.Pool.GetPool(SparklePrefab, transform.position, transform.rotation);
    }


}
