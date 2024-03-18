using UnityEditor.SceneManagement;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField] float speed = 35f;

    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PooledObject pooledObject;
    [SerializeField] PooledObject bulletCollision;
    [SerializeField] float bulletDamage = 2f;
    BarController barCard;
    [SerializeField] AudioClip shootAD;
   

    
    private void OnEnable()
    {
        audioSource=GetComponent<AudioSource>();
        GameObject card= GameObject.Find("Card");

        barCard=card.gameObject.GetComponent<BarController>(); //��¿ �� ����... ���߿� �ٲ㺸��.. 
  
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator.Play("WeaponShot");
        audioSource.clip = shootAD;
        audioSource.PlayOneShot(shootAD);
        

    }

    void Update()
    {
        rb.velocity = transform.right * speed; //x���� �ٶ󺸴� �������� �߻�ǵ��� 
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet" ||
            collision.gameObject.tag == "Checking"||collision.gameObject.tag=="Exit")
            
        {
            return;
        }
       

        IDamagable target=collision.GetComponent<IDamagable>();
        if(target != null)
        { 
            target.OnDamage(bulletDamage); //�������� �ҷ��� �ְ� 
            if (barCard != null)
            {
                barCard.AttackCardCharge();
            }
        }

        pooledObject.Release(); //�ٸ� ��ҿ� �ε����� �ı� --> �ı� �ִϸ��̼� ��� �ʿ�
        Manager.Pool.GetPool(bulletCollision, transform.position, transform.rotation);



    }

}
