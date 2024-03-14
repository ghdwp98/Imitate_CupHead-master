using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPui : MonoBehaviour

    //체력 달기 구현 및
    // 플레이어의 hp : 0 --> dead 상태 
{
    [SerializeField] public Image hp3;
    [SerializeField] public Image hp2;
    [SerializeField] public Image hp1;
    [SerializeField] public Image dead;
    [SerializeField] PlayerController playerController;

    
    public bool takeHit=false; //무적상태 계산 


    void Start()
    {
        
    }

    void Update()
    {
        
    }


    // ui 바꾸기 구현 
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
        else if(playerController.hp==1) // 죽음 상태 진입 
        {
            hp1.gameObject.SetActive(false);

        }        
    }

    



}
