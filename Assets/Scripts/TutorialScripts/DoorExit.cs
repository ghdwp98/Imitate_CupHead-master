using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorExit : MonoBehaviour
{
    GameObject DoorKeyPrefab;
    GameObject player;


    private void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(DoorKeyPrefab);
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                Manager.Scene.LoadScene("WorldMapScene");
            }
        }
    }
}
