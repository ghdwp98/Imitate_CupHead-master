using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoSlime : MonoBehaviour
{
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("�浹��");
        if (collision.gameObject.tag == "Player")
        {
            //������ �˾�â ����ְ�

            if (Input.GetKeyDown(KeyCode.A))
            {
                Manager.Scene.LoadScene("SlimeScene");
            }
        }
    }
}
