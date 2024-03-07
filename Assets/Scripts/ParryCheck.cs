using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryCheck : MonoBehaviour
{
    public bool isParryed=false;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            return; //do nothing
        }

        isParryed = true;
        Debug.Log("자식 탈출");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            return; //do nothing
        }

        isParryed = false;
        Debug.Log("자식 붙음");
    }

    
}
