using UnityEditor.SceneManagement;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField] float speed = 35f;

    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PooledObject pooledObject;
    [SerializeField] PooledObject bulletCollision;
    [SerializeField] GameObject spawnPos;



    private void OnEnable()
    {
        
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
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet")
        {
            return;
        }

        pooledObject.Release(); //다른 장소에 부딪히면 파괴 --> 파괴 애니메이션 출력 필요
        Manager.Pool.GetPool(bulletCollision, transform.position, transform.rotation);
    }

}
