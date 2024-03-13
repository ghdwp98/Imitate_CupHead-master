using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TBarController : MonoBehaviour
{
    //자기 자식 5개를 리스트

    [SerializeField] Image[] manaBarImage; //배열 아기 5개 0~4 
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Sprite fullCard;
    [SerializeField] Sprite EmptyCard;
    
    [SerializeField] float manaRegen;

    private void Awake()
    {
        for(int i=0;i<manaBarImage.Length;i++)
        {
            manaBarImage[i].fillAmount= 0; //시작 할 때 다 0 으로 시작 .
        }    
    }


    public void Update()  //플레이어에서 호출할 함수 들 
    {
        if(Input.GetKey(KeyCode.Space)) 
        {
            CardCharge();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            EXshoot();
        }
    }

    public void CardCharge()
    {
        for(int i=0;i<manaBarImage.Length;i++)
        {
            if (manaBarImage[i].fillAmount!=1f)
            {
                manaBarImage[i].fillAmount += 0.05f;
                break;
            }
            else
            {
                manaBarImage[i].sprite = fullCard;
            }
        }
    }

    public void EXshoot()
    {
        for(int i=4;i>=0;i--) //4번 부터 빼줘야 하니까 스프라이트도 원래 자기껄로 돌려주면 됨. 
        {
            if (manaBarImage[i].fillAmount == 1f)
            {

            }
            else
            {

            }
        }
    }

    public void FixedUpdate()
    {
        for (int i = 0; i < manaBarImage.Length; i++)
        {
            if (manaBarImage[i].fillAmount != 1f)
            {
                manaBarImage[i].fillAmount += (Time.deltaTime/30);
                break;
            }
            else
            {
                manaBarImage[i].sprite = fullCard;
            }
        }
    }



}
