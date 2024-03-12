using System.Collections;
using UnityEngine;

public class Targets : LivingEntity
{
    float hp = 10f;
    [SerializeField] GameObject parent;
     Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
        health = hp;
    }

    public override void OnDamage(float damage) //버추얼함수 상속 
    {
        base.OnDamage(damage);
        health -= (float)damage;
    }

    private void OnTriggerEnter2D(Collider2D collision) //타겟은 트리거를 가지자. 
    {
        if (!dead)
        {
            //피격효과음
            if (collision.gameObject.tag == "Bullet")
            {
                StartCoroutine(OnHit()); //피격 색 변경 효과
                //피격 애니메이션 필요 

            }
            
        }

    }
    public override void Die()
    {
        base.Die();


        if (parent != null)
        {

            parent.SetActive(false);
        }
    }

    IEnumerator OnHit()
    {
        Debug.Log("코루틴");
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer parentSprite = transform.parent.GetComponent<SpriteRenderer>();
        parentSprite.color = new Color(87/255f, 100/255f, 100/255f); 
        spriteRenderer.color = new Color(87 / 255f, 100 / 255f, 100 / 255f);
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = new Color(1, 1, 1, 1f); 
        parentSprite.color = new Color(1, 1, 1, 1f); 


    }

}
