using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorExit : MonoBehaviour
{
    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //나가기 팝업창 띄워주고

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Manager.Scene.LoadScene("WorldMapScene");
            }
        }
    }
}
