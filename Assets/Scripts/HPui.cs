using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPui : MonoBehaviour

    //ü�� �ޱ� ���� ��
    // �÷��̾��� hp : 0 --> dead ���� 
{
    [SerializeField] public Image hp3;
    [SerializeField] public Image hp2;
    [SerializeField] public Image hp1;
    [SerializeField] public Image dead;
    [SerializeField] PlayerController playerController;

    
    public bool takeHit=false; //�������� ��� 


    void Start()
    {
        
    }

    void Update()
    {
        
    }


    // ui �ٲٱ� ���� 
    public void HpChange()
    {
        if(playerController.hp==3)
        {
            hp3.gameObject.SetActive(false);
        }
        else if(playerController.hp==2)
        {
            hp2.gameObject.SetActive(false);

        }
        else if(playerController.hp==1) // ���� ���� ���� 
        {
            hp1.gameObject.SetActive(false);

        }        
    }

    



}
