using System.Collections;
using UnityEngine;

public class PooledObject : MonoBehaviour //�̰� ��ӹ޴� ģ���鸸 ��� �ϰڴٴ� able���� ���ΰ�? 
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

    public void Release()  //�ᱹ ���� ��Ȳ�� ���ĸ� pool�� null�� �����ΰŰŵ� �ٴ� Pool�� null��... �� ?
    {
        
        
        if (pool != null)
        {
            
            pool.ReturnPool(this); //gameobject�� �ұ�? 
        }
        else
        {
            
            Destroy(gameObject);
        }
    }
}
