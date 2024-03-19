using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GoSlime : MonoBehaviour
{
    [SerializeField] GameObject DoorKeyPrefab;
    GameObject popUp;
    [SerializeField] GameObject pos;
    [SerializeField] PopUpUI popUpPrefab;
    [SerializeField] GameObject flagPrefab;
    bool SlimeflagOn = false;

    private void Awake()
    {
        if(Manager.Game.slimeDie==true)
        {
            Instantiate(flagPrefab,transform.localPosition,Quaternion.identity);

            SlimeflagOn=true; //���߿� ���忡 �� �뵵 --> true ���·� ���� ����. 
            Manager.Game.slimeDie = false;
        }
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) //escŰ�� ���� �ִ� �˾� ui ���� �� ����. 
        {
            Manager.UI.ClosePopUpUI();
        }
 
    }

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
            Manager.UI.ShowPopUpUI<PopUpUI>(popUpPrefab);


        }
    }

       
}
