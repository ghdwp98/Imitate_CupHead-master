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
    [SerializeField] float bulletDamage = 20f; //필살기 데미지 --> IDamagerble에 데미지 줘야함..
    // 몬스터 스크립트와 연계해줘야함. 
    BarController barCard;
    [SerializeField]AudioClip clip;

    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        GameObject card = GameObject.Find("Card");
        barCard = card.gameObject.GetComponent<BarController>(); //어쩔 수 없다... 나중에 바꿔보자.. 
        animator = GetComponent<Animator>();
        rb= GetComponent<Rigidbody2D>();
        animator.Play("EXShoot");
        audioSource.clip = clip;
        audioSource.PlayOneShot(clip);
    }

    void Update()
    {
        rb.velocity = transform.right * EXbulletSpeed; //x축이 바라보는 방향으로 발사되도록 

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
            target.OnDamage(bulletDamage); //데미지는 불렛이 주고 
            if (barCard != null)
            {
                barCard.AttackCardCharge(); //그냥 필살기도 어택차지 해서 똑같이 채워주기 
            }
        }

     


        
    }
}
