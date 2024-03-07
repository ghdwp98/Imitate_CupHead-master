using UnityEngine;

public class BulletMove : PooledObject
{
    public float speed = 35f;
    
    Animator animator;
    Rigidbody2D rb;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator.Play("WeaponShot");
        
    }

    void Update()
    {

        /*if(this.transform.position.x>20)
        {
            Release();
        }*/
        rb.velocity=new Vector2(30*speed*Time.deltaTime,rb.velocity.y);

        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player"||collision.gameObject.tag=="Bullet")
        {
            return;
        }

        Release();
    }




}
