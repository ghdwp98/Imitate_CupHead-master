using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamagable
{
    public float startingHealth = 100f;
    public float health { get; set; }
    public bool dead { get; protected set; }

    public event Action onDeath; //사망 시 발동할 이벤트 



    public virtual void OnDamage(float damage)
    {

        if (health <= 0 && !dead) //죽지 않았을 때 체력이 감소하면 사망 처리 실행
        {
            Die();
            
        }
    }

    void Start() //딱히 생성하는게 아니라 처음부터 있는거니까 그냥 start 로 가자.
    {
        dead = false;
        health = startingHealth;
    }

    public virtual void Die()
    {
        Debug.Log("사망처리");
        if (onDeath != null) //등록된 이벤트가 있으면 사망 시 이벤트 실행 .
        {
            onDeath(); //튜토리얼 타겟은 작은 폭발 과 활성화/비활성화
            // 보스라면 보스형폭발과 게임클리어 애니메이션 등 관련 작업 진행 

        }
        dead = true;
    }
}
