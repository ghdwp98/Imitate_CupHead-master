using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IParry :MonoBehaviour //�и� ������ ��ü�鿡 �ٿ��ֱ�. 
    //�и� �ÿ� ��� �ð� �����ֱ�
{
    public virtual void Parried()
    {
        Debug.Log("�и��� ����");
        StartCoroutine(parryCoroutine());
    }

    IEnumerator parryCoroutine()
    {
        Time.timeScale = 0.8f;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1.0f;
        
        
    }
}
