using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExBulletMove : MonoBehaviour
{

    [SerializeField] float EXbulletSpeed = 15;
    Animator animator;
    Rigidbody2D rb;
    [SerializeField]PooledObject pooledObject;
    [SerializeField]PooledObject ExBulletCollision;
    [SerializeField] float bulletDamage = 20f; //�ʻ�� ������ --> IDamagerble�� ������ �����..
    // ���� ��ũ��Ʈ�� �����������. 


    void OnEnable()
    {
        animator= GetComponent<Animator>();
        rb= GetComponent<Rigidbody2D>();
        animator.Play("EXShoot");
    }

    void Update()
    {
        rb.velocity = transform.right * EXbulletSpeed; //x���� �ٶ󺸴� �������� �߻�ǵ��� 

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet" ||
            collision.gameObject.tag == "Checking")
        {
            return;
        }

        IDamagable target=collision.GetComponent<IDamagable>();
        if(target != null) 
        {
            target.OnDamage(bulletDamage); //�������� �ҷ��� �ְ� 
        }


        pooledObject.Release();
        Manager.Pool.GetPool(ExBulletCollision,transform.position,transform.rotation);
    }
}
