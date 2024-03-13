using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpwaner : MonoBehaviour
{
    [SerializeField]GameObject ghostPrefab;
    [SerializeField] bool spawn = false;
    void Start()
    {
        
    }

    void Update()
    {
        
        
    }


    IEnumerator GhostSpawn()
    {
        if(spawn==false)
        {
            Debug.Log("코루틴 작동");
            Instantiate(ghostPrefab, transform.position, transform.rotation);
            spawn = true;
            yield return new WaitForSecondsRealtime(8f);
            spawn = false;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        StartCoroutine(GhostSpawn());
    }



}
