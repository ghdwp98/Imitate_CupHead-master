using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryCircle : IParry  //�̺�Ʈ? 
{
    
    

    private void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
    }


    public override void Parried()
    {
        base.Parried();
        Debug.Log(transform.position);

        if (transform.localPosition.x == -4.5f)
        {
           Transform sphere = GameObject.Find("pink2").transform.Find("tutorial_pink_sphere_2");
           sphere.gameObject.SetActive(true);
           gameObject.SetActive(false);

        }

        if (transform.localPosition.x == -1.72f)
        {
            Transform sphere = GameObject.Find("pink3").transform.Find("tutorial_pink_sphere_3");
            sphere.gameObject.SetActive(true);
            gameObject.SetActive(false);

        }

        if (transform.localPosition.x == 0.87f)
        {
            Transform sphere = GameObject.Find("pink1").transform.Find("tutorial_pink_sphere_1");
            sphere.gameObject.SetActive(true);
            gameObject.SetActive(false);

        }
    }

    
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

}
