using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryCheck : MonoBehaviour
{
    public bool isParryed=false;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag=="Player"||collision.tag=="Checking")
        {
            return; //do nothing
        }

        isParryed = true;
       
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Checking")
        {
            return; //do nothing
        }

        isParryed = false;
        
    }

    
}
