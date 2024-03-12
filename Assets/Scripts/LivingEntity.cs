using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamagable
{
    public float startingHealth = 100f;
    public float health { get; set; }
    public bool dead { get; protected set; }

    public event Action onDeath; //��� �� �ߵ��� �̺�Ʈ 



    public virtual void OnDamage(float damage)
    {

        if (health <= 0 && !dead) //���� �ʾ��� �� ü���� �����ϸ� ��� ó�� ����
        {
            Die();
            
        }
    }

    void Start() //���� �����ϴ°� �ƴ϶� ó������ �ִ°Ŵϱ� �׳� start �� ����.
    {
        dead = false;
        health = startingHealth;
    }

    public virtual void Die()
    {
        Debug.Log("���ó��");
        if (onDeath != null) //��ϵ� �̺�Ʈ�� ������ ��� �� �̺�Ʈ ���� .
        {
            onDeath(); //Ʃ�丮�� Ÿ���� ���� ���� �� Ȱ��ȭ/��Ȱ��ȭ
            // ������� ���������߰� ����Ŭ���� �ִϸ��̼� �� ���� �۾� ���� 

        }
        dead = true;
    }
}
