using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryCircle : IParry  //¿Ã∫•∆Æ? 
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
        
        if (transform.localPosition.x == -4.5f)
        {
           Transform sphere = GameObject.Find("pink2").transform.Find("tutorial_pink_sphere_2");
           sphere.gameObject.SetActive(true);
           gameObject.SetActive(false);
           Time.timeScale = 1.0f;

        }

        if (transform.localPosition.x == -1.72f)
        {
            Transform sphere = GameObject.Find("pink3").transform.Find("tutorial_pink_sphere_3");
            sphere.gameObject.SetActive(true);
            gameObject.SetActive(false);
            Time.timeScale = 1.0f;

        }

        if (transform.localPosition.x == 0.87f)
        {
            Transform sphere = GameObject.Find("pink1").transform.Find("tutorial_pink_sphere_1");
            sphere.gameObject.SetActive(true);
            gameObject.SetActive(false);
            Time.timeScale = 1.0f;

        }
    }

    
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

}
