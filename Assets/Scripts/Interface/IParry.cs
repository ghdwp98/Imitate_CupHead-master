using System.Collections;
using UnityEngine;

public class IParry : MonoBehaviour //�и� ������ ��ü�鿡 �ٿ��ֱ�. 
                                    //�и� �ÿ� ��� �ð� �����ֱ�
{
    bool isparrying = false;

    private void Start()
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
            Time.timeScale = 0.8f;
            isparrying = true;
            yield return new WaitForSecondsRealtime(1f);
            Time.timeScale = 1.0f;
            isparrying = false;
        }


    }




}
