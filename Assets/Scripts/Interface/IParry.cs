using System.Collections;
using UnityEngine;

public class IParry : MonoBehaviour //패리 가능한 물체들에 붙여주기. 
                                    //패리 시에 잠깐 시간 늦춰주기
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
