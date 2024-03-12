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
        {
            //피격효과음 + 피격 애니메이션 

            

        }

    }

    public override void Die()
    {
        base.Die();
        //파괴 애니메이션 재생 및 자신의 오브젝트파괴
        Destroy(gameObject.transform.parent);
        

    }

}
