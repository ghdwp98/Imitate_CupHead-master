using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoSlime : MonoBehaviour
{
    [SerializeField] GameObject DoorKeyPrefab;
    GameObject popUp;
    [SerializeField] GameObject pos;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {

            popUp = Instantiate(DoorKeyPrefab, new Vector2(-0.4f,9.37f), Quaternion.identity);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(popUp);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("ÆË¾÷Ã¢ ¶ç¿ì±â");
        }
    }
}
