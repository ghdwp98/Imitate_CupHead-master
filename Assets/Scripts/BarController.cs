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
        for (int i = 0; i < manaBarImage.Length; i++)
        {
            manaBarImage[i].fillAmount = 0; //���� �� �� �� 0 ���� ���� .
        }
    }


    public void Update()  //�÷��̾�� ȣ���� �Լ� �� 
    {
        if (Input.GetKey(KeyCode.Space))
        {
            CardCharge();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            EXshoot();
        }
    }

    public void CardCharge()
    {
        for (int i = 0; i < manaBarImage.Length; i++)
        {
            if (manaBarImage[i].fillAmount != 1f)
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
        for (int i = 4; i >= 1; i--) //4�� ���� ����� �ϴϱ� ��������Ʈ�� ���� �ڱⲬ�� �����ָ� ��. 
        {
            if (manaBarImage[i].fillAmount == 1f)
            {
                manaBarImage[i].fillAmount = 0f;
                manaBarImage[i].sprite = EmptyCard;
                break;
            }
            else if (manaBarImage[i].fillAmount>=0.01f) //�� �� ���� ������ 
            {
                float fill = 1f - manaBarImage[i].fillAmount; //�� ��ŭ���� �� ������ �����������
                manaBarImage[i].fillAmount = 0f;
                manaBarImage[i].sprite = EmptyCard;
                manaBarImage[i - 1].fillAmount -=fill;
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
