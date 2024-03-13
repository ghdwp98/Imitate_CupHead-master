using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoSlime : MonoBehaviour
{
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("충돌중");
        if (collision.gameObject.tag == "Player")
        {
            //나가기 팝업창 띄워주고

            if (Input.GetKeyDown(KeyCode.A))
            {
                Manager.Scene.LoadScene("SlimeScene");
            }
        }
    }
}
