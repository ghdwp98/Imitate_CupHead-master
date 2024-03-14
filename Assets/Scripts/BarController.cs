using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    //자기 자식 5개를 리스트

    [SerializeField] public Image[] manaBarImage; //배열 아기 5개 0~4 
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


    public void AttackCardCharge() //공격 시 카드 채우기. 
    {
        //프리팹에 이 ui프리팹을 할당해도 게임오브젝트 하이어라키에 있는
        // 게임오브젝트와 상호작용이 되지는 않는다. 
        // 나중에 이부분 고쳐보자. 
        for (int i = 0; i < manaBarImage.Length; i++)
        {
            Debug.Log("어택차지");
            if (manaBarImage[i].fillAmount != 1f)
            {
                manaBarImage[i].fillAmount += 0.05f; 
                
                break;
            }

           
            
        }
    }

    public void ParryCardCharge() //패리 시 1칸 채워야함. 
    {
        for (int i = 0; i < manaBarImage.Length; i++)
        {
            if (manaBarImage[i].fillAmount < 1f)
            {
                float fill = manaBarImage[i].fillAmount; //자신의 필 어마운트만큼 다음 놈한테 ++ 
                manaBarImage[i].fillAmount = 1f;
                if(i+1< manaBarImage.Length)
                {
                    manaBarImage[i + 1].fillAmount = fill;
                }  
                break;
            }

        }
    }

    public void EXshoot()  //필살기 시전 시 exshoot 발동 --> 1칸씩 빠짐. 
    {
        for (int i = 4; i >= 1; i--) //4번 부터 빼줘야 하니까 스프라이트도 원래 자기껄로 돌려주면 됨. 
        {
            if (manaBarImage[0].fillAmount < 1f)  //0번째가 안차 있으면 안나가야 하는데 어째서?? 
            {
                break; 
            }
            else if (manaBarImage[i].fillAmount == 1f)
            {
                manaBarImage[i].fillAmount = 0f;
                manaBarImage[i].sprite = EmptyCard;
                break;
            }
            else if (manaBarImage[i].fillAmount >= 0.01f) //꽉 차 있지 않으면 
            {
                float fill = 1f - manaBarImage[i].fillAmount; //이 만큼으로 그 전꺼도 변경해줘야함
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
