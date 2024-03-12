using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targets : LivingEntity
{
    float hp = 10f;

    private void Start()
    {
        health = hp;
    }


    public override void OnDamage(float damage) //버추얼함수 상속 
    { 
        base.OnDamage(damage);
        health-=(float)damage;
    }


    private void OnTriggerEnter2D(Collider2D collision) //타겟은 트리거를 가지자. 
    {
        if(!dead) 
        {// 파괴 애니메이션 재생 +효과음 + 자신의 부모와 자식 들 게임 오브젝트 파괴 + 다른 게임오브젝트 활성화 

            Debug.Log(health);

        }


        

    }
}
