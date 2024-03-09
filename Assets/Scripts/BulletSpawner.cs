using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    //�������� ���� ��ġ�� �÷��̾ �����ϴ°� �� ���� ��� 
    //�׷��� render ���� ���´� ���⼭ �Ǵ��ϴ°�?? 

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

    public void ObjectSpawn() //��ȯ�ϴ� ��ġ�� �ڽ��� ��ġ�ϱ� �÷��̾��� ���¿� ���� �� ��ġ�� �ٲ�����.
    {
        Manager.Pool.GetPool(bulletPrefab, transform.position, transform.rotation);
        Manager.Pool.GetPool(SparklePrefab, transform.position, transform.rotation);
    }


}
