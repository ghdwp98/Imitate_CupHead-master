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
        StartCoroutine(GhostSpawn());
        
    }


    IEnumerator GhostSpawn()
    {
        if(spawn==false)
        {
            Instantiate(ghostPrefab, transform.position, transform.rotation);
            spawn = true;
            yield return new WaitForSecondsRealtime(8f);
            spawn = false;
        }
    }
}
