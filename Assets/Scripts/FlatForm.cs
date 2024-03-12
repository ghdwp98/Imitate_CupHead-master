using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatForm : MonoBehaviour
{
    PlatformEffector2D plat;


    void Start()
    {
        plat= GetComponent<PlatformEffector2D>();   
    }

    
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            
            plat.useColliderMask = false;
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            

            plat.useColliderMask = false;
        }
    }
}
