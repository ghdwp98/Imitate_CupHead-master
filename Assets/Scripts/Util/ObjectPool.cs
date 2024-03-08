using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] PooledObject prefab; //pooled 상속한 프리팹들만 오브젝트풀링 하겠다는 의미
    [SerializeField] int size;
    [SerializeField] int capacity;

    private Stack<PooledObject> objectPool;

    public void CreatePool(PooledObject prefab, int size, int capacity)
    {
        this.prefab = prefab; //오브젝트 풀로 생성할 프리팹 
        this.size = size; 
        this.capacity = capacity;

        objectPool = new Stack<PooledObject>(capacity);
        for (int i = 0; i < size; i++)  //지정한 size 만큼 객체를 생성해서 스택에 저장해둔다. 
        {
            PooledObject instance = Instantiate(prefab);
            instance.gameObject.SetActive(false); //보이지 않아야 하므로 비활성화한다.
            instance.Pool = this;
            instance.transform.parent = transform;
            objectPool.Push(instance); //instance를 생성 후 push해서 스택에 저장 
        }
    }

    public PooledObject GetPool(Vector3 position, Quaternion rotation)
    {
        if (objectPool.Count > 0) //풀에서 인스턴스를 가지고 온다. 
        {
            PooledObject instance = objectPool.Pop();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.gameObject.SetActive(true);
            return instance;
        }
        else //풀이 비어있으면 생성해서 가지고 온다. 어차피 똑같이 return instance임 
        {
            PooledObject instance = Instantiate(prefab);
            instance.Pool = this;
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            return instance;
        }
    }

    public void ReturnPool(PooledObject instance) //풀에 돌려준다. 비활성화를 통하여 
    {
        
        if (objectPool.Count < capacity)
        {
            
            instance.gameObject.SetActive(false);
            instance.transform.parent = transform;
            objectPool.Push(instance); //스택에 다시 푸쉬한다. 
        }
        else
        {
            Destroy(instance.gameObject); //스택의 크기를 넘어 생성된 친구는 삭제한다. 
        }
    }
}
