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


    public override void OnDamage(float damage) //���߾��Լ� ��� 
    { 
        base.OnDamage(damage);
        health-=(float)damage;
    }


    private void OnTriggerEnter2D(Collider2D collision) //Ÿ���� Ʈ���Ÿ� ������. 
    {
        if(!dead) 
        {
            //�ǰ�ȿ���� + �ǰ� �ִϸ��̼� 

            

        }

    }

    public override void Die()
    {
        base.Die();
        //�ı� �ִϸ��̼� ��� �� �ڽ��� ������Ʈ�ı�
        Destroy(gameObject.transform.parent);
        

    }

}
