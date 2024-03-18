using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExBulletMove : MonoBehaviour
{

    [SerializeField] float EXbulletSpeed = 15;
    Animator animator;
    [SerializeField]AudioSource audioSource;
    Rigidbody2D rb;
    [SerializeField]PooledObject pooledObject;
    [SerializeField]PooledObject ExBulletCollision;
    [SerializeField] float bulletDamage = 20f; //�ʻ�� ������ --> IDamagerble�� ������ �����..
    // ���� ��ũ��Ʈ�� �����������. 
    BarController barCard;
    [SerializeField]AudioClip clip;

    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        GameObject card = GameObject.Find("Card");
        barCard = card.gameObject.GetComponent<BarController>(); //��¿ �� ����... ���߿� �ٲ㺸��.. 
        animator = GetComponent<Animator>();
        rb= GetComponent<Rigidbody2D>();
        animator.Play("EXShoot");
        audioSource.clip = clip;
        audioSource.PlayOneShot(clip);
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
            pooledObject.Release();
            Manager.Pool.GetPool(ExBulletCollision, transform.position, transform.rotation);
            target.OnDamage(bulletDamage); //�������� �ҷ��� �ְ� 
            if (barCard != null)
            {
                barCard.AttackCardCharge(); //�׳� �ʻ�⵵ �������� �ؼ� �Ȱ��� ä���ֱ� 
            }
        }

     


        
    }
}
