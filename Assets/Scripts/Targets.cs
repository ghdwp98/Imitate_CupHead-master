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
        {// �ı� �ִϸ��̼� ��� +ȿ���� + �ڽ��� �θ�� �ڽ� �� ���� ������Ʈ �ı� + �ٸ� ���ӿ�����Ʈ Ȱ��ȭ 

            Debug.Log(health);

        }


        

    }
}
