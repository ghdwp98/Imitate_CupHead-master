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

    public void Release()
    {
        if (pool != null)
        {
            pool.ReturnPool(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
