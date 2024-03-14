using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    //�ڱ� �ڽ� 5���� ����Ʈ

    [SerializeField] public Image[] manaBarImage; //�迭 �Ʊ� 5�� 0~4 
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Sprite fullCard;
    [SerializeField] Sprite EmptyCard;

    [SerializeField] float manaRegen;

    private void Awake()
    {
        for (int i = 0; i < manaBarImage.Length; i++)
        {
            manaBarImage[i].fillAmount = 0; //���� �� �� �� 0 ���� ���� .
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            ParryCardCharge();
        }

        if (Input.GetKeyDown (KeyCode.Space))
        {
            EXshoot();
        }
        

    }


    public void AttackCardCharge() //���� �� ī�� ä���. 
    {
        //�����տ� �� ui�������� �Ҵ��ص� ���ӿ�����Ʈ ���̾��Ű�� �ִ�
        // ���ӿ�����Ʈ�� ��ȣ�ۿ��� ������ �ʴ´�. 
        // ���߿� �̺κ� ���ĺ���. 
        for (int i = 0; i < manaBarImage.Length; i++)
        {
            Debug.Log("��������");
            if (manaBarImage[i].fillAmount != 1f)
            {
                manaBarImage[i].fillAmount += 0.05f; 
                
                break;
            }

           
            
        }
    }

    public void ParryCardCharge() //�и� �� 1ĭ ä������. 
    {
        for (int i = 0; i < manaBarImage.Length; i++)
        {
            if (manaBarImage[i].fillAmount < 1f)
            {
                float fill = manaBarImage[i].fillAmount; //�ڽ��� �� ���Ʈ��ŭ ���� ������ ++ 
                manaBarImage[i].fillAmount = 1f;
                if(i+1< manaBarImage.Length)
                {
                    manaBarImage[i + 1].fillAmount = fill;
                }  
                break;
            }

        }
    }

    public void EXshoot()  //�ʻ�� ���� �� exshoot �ߵ� --> 1ĭ�� ����. 
    {
        for (int i = 4; i >= 1; i--) //4�� ���� ����� �ϴϱ� ��������Ʈ�� ���� �ڱⲬ�� �����ָ� ��. 
        {
            if (manaBarImage[0].fillAmount < 1f)  //0��°�� ���� ������ �ȳ����� �ϴµ� ��°��?? 
            {
                break; 
            }
            else if (manaBarImage[i].fillAmount == 1f)
            {
                manaBarImage[i].fillAmount = 0f;
                manaBarImage[i].sprite = EmptyCard;
                break;
            }
            else if (manaBarImage[i].fillAmount >= 0.01f) //�� �� ���� ������ 
            {
                float fill = 1f - manaBarImage[i].fillAmount; //�� ��ŭ���� �� ������ �����������
                manaBarImage[i].fillAmount = 0f;
                manaBarImage[i].sprite = EmptyCard;
                manaBarImage[i - 1].fillAmount -= fill;
                manaBarImage[i - 1].sprite = EmptyCard;
                break;

            }
        }
    }

    public void FixedUpdate()
    {
        for (int i = 0; i < manaBarImage.Length; i++)
        {
            if (manaBarImage[i].fillAmount != 1f)
            {
                manaBarImage[i].fillAmount += (Time.deltaTime / 30);
                break;
            }
            else
            {
                manaBarImage[i].sprite = fullCard;
            }



           
        }


        
    }



}
