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
        for (int i = 0; i < manaBarImage.Length; i++)
        {
            manaBarImage[i].fillAmount = 0; //시작 할 때 다 0 으로 시작 .
        }
    }


    public void Update()  //플레이어에서 호출할 함수 들 
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
        for (int i = 4; i >= 1; i--) //4번 부터 빼줘야 하니까 스프라이트도 원래 자기껄로 돌려주면 됨. 
        {
            if (manaBarImage[i].fillAmount == 1f)
            {
                manaBarImage[i].fillAmount = 0f;
                manaBarImage[i].sprite = EmptyCard;
                break;
            }
            else if (manaBarImage[i].fillAmount>=0.01f) //꽉 차 있지 않으면 
            {
                float fill = 1f - manaBarImage[i].fillAmount; //이 만큼으로 그 전꺼도 변경해줘야함
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
