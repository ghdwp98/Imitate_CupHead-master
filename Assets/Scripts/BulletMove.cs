using UnityEditor.SceneManagement;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField] float speed = 35f;

    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PooledObject pooledObject;
    [SerializeField] PooledObject bulletCollision;
    [SerializeField] Transform spawnPos;



    private void OnEnable()
    {

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator.Play("WeaponShot");
    }

    void Update()
    {
        
        rb.velocity = Vector2.right * speed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet")
        {
            return;
        }

        pooledObject.Release(); //�ٸ� ��ҿ� �ε����� �ı� --> �ı� �ִϸ��̼� ��� �ʿ�
        Manager.Pool.GetPool(bulletCollision, transform.position, transform.rotation);
    }

}
