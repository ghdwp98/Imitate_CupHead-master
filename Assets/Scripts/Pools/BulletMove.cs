using UnityEditor.SceneManagement;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField] float speed = 35f;

    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PooledObject pooledObject;
    [SerializeField] PooledObject bulletCollision;
    [SerializeField] float bulletDamage = 2f;
    BarController barCard;
    
   

    
    private void OnEnable()
    {
        GameObject card= GameObject.Find("Card");
        barCard=card.gameObject.GetComponent<BarController>(); //어쩔 수 없다... 나중에 바꿔보자.. 
  
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator.Play("WeaponShot");
    }

    void Update()
    {
        rb.velocity = transform.right * speed; //x축이 바라보는 방향으로 발사되도록 
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
            target.OnDamage(bulletDamage); //데미지는 불렛이 주고 
            if (barCard != null)
            {
                barCard.AttackCardCharge();
            }
        }

        


        pooledObject.Release(); //다른 장소에 부딪히면 파괴 --> 파괴 애니메이션 출력 필요
        Manager.Pool.GetPool(bulletCollision, transform.position, transform.rotation);
    }

}
