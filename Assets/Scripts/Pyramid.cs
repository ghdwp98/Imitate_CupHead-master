using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyramid : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Bullet")
        {
            Collider2D collider = GetComponent<BoxCollider2D>();
            collider.enabled = false;

        }
    }


}
