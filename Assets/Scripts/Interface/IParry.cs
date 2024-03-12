using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IParry :MonoBehaviour //패리 가능한 물체들에 붙여주기. 
    //패리 시에 잠깐 시간 늦춰주기
{
    public virtual void Parried()
    {
        Debug.Log("패리드 실행");
        StartCoroutine(parryCoroutine());
    }

    IEnumerator parryCoroutine()
    {
        Time.timeScale = 0.8f;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1.0f;
        
        
    }
}
