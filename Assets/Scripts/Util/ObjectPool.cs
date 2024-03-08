using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] PooledObject prefab; //pooled ����� �����յ鸸 ������ƮǮ�� �ϰڴٴ� �ǹ�
    [SerializeField] int size;
    [SerializeField] int capacity;

    private Stack<PooledObject> objectPool;

    public void CreatePool(PooledObject prefab, int size, int capacity)
    {
        this.prefab = prefab; //������Ʈ Ǯ�� ������ ������ 
        this.size = size; 
        this.capacity = capacity;

        objectPool = new Stack<PooledObject>(capacity);
        for (int i = 0; i < size; i++)  //������ size ��ŭ ��ü�� �����ؼ� ���ÿ� �����صд�. 
        {
            PooledObject instance = Instantiate(prefab);
            instance.gameObject.SetActive(false); //������ �ʾƾ� �ϹǷ� ��Ȱ��ȭ�Ѵ�.
            instance.Pool = this;
            instance.transform.parent = transform;
            objectPool.Push(instance); //instance�� ���� �� push�ؼ� ���ÿ� ���� 
        }
    }

    public PooledObject GetPool(Vector3 position, Quaternion rotation)
    {
        if (objectPool.Count > 0) //Ǯ���� �ν��Ͻ��� ������ �´�. 
        {
            PooledObject instance = objectPool.Pop();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.gameObject.SetActive(true);
            return instance;
        }
        else //Ǯ�� ��������� �����ؼ� ������ �´�. ������ �Ȱ��� return instance�� 
        {
            PooledObject instance = Instantiate(prefab);
            instance.Pool = this;
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            return instance;
        }
    }

    public void ReturnPool(PooledObject instance) //Ǯ�� �����ش�. ��Ȱ��ȭ�� ���Ͽ� 
    {
        
        if (objectPool.Count < capacity)
        {
            
            instance.gameObject.SetActive(false);
            instance.transform.parent = transform;
            objectPool.Push(instance); //���ÿ� �ٽ� Ǫ���Ѵ�. 
        }
        else
        {
            Destroy(instance.gameObject); //������ ũ�⸦ �Ѿ� ������ ģ���� �����Ѵ�. 
        }
    }
}
