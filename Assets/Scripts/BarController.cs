using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TBarController : MonoBehaviour
{
    //�ڱ� �ڽ� 5���� ����Ʈ

    [SerializeField] Image[] manaBarImage; //�迭 �Ʊ� 5�� 0~4 
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Sprite fullCard;
    [SerializeField] Sprite EmptyCard;
    
    [SerializeField] float manaRegen;

    private void Awake()
    {
        for(int i=0;i<manaBarImage.Length;i++)
        {
            manaBarImage[i].fillAmount= 0; //���� �� �� �� 0 ���� ���� .
        }    
    }


    public void Update()  //�÷��̾�� ȣ���� �Լ� �� 
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
        for(int i=4;i>=0;i--) //4�� ���� ����� �ϴϱ� ��������Ʈ�� ���� �ڱⲬ�� �����ָ� ��. 
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
