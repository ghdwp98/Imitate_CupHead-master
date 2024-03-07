using System.Collections;
using UnityEngine;

public class PooledObject : MonoBehaviour //이거 상속받는 친구들만 상속 하겠다는 able같은 거인가? 
{
    [SerializeField] bool autoRelease;
    [SerializeField] float releaseTime;

    private ObjectPool pool;
    public ObjectPool Pool { get { return pool; } set { pool = value; } }

    private void OnEnable()
    {
        if (autoRelease)
        {
            StartCoroutine(ReleaseRoutine());
        }
    }

    IEnumerator ReleaseRoutine()
    {
        yield return new WaitForSeconds(releaseTime);
        Release();
    }

    public void Release()  //결국 지금 상황이 뭐냐면 pool이 null인 상태인거거든 근대 Pool도 null임... 왜 ?
    {
        
        
        if (pool != null)
        {
            
            pool.ReturnPool(this); //gameobject로 할까? 
        }
        else
        {
            
            Destroy(gameObject);
        }
    }
}
