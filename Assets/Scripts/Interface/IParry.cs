using System.Collections;
using UnityEngine;

public class IParry : MonoBehaviour //�и� ������ ��ü�鿡 �ٿ��ֱ�. 
                                    //�и� �ÿ� ��� �ð� �����ֱ�
{
    public bool isparrying = false;

    private void Start()
    {

    }

    private void Update()
    {
        
    }

    public virtual void Parried()
    {
        
        StartCoroutine(parryCoroutine());

    }

    IEnumerator parryCoroutine()
    {

        if (isparrying == false)
        {
            Time.timeScale = 0.7f;
            isparrying = true;
            yield return new WaitForSecondsRealtime(0.5f);
            Time.timeScale = 1.0f;
            isparrying = false;
            
        }
    }
    




}
