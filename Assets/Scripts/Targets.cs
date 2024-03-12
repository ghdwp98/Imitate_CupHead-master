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

    public override void OnDamage(float damage) //���߾��Լ� ��� 
    {
        base.OnDamage(damage);
        health -= (float)damage;
    }

    private void OnTriggerEnter2D(Collider2D collision) //Ÿ���� Ʈ���Ÿ� ������. 
    {
        if (!dead)
        {
            //�ǰ�ȿ����
            if (collision.gameObject.tag == "Bullet")
            {
                StartCoroutine(OnHit()); //�ǰ� �� ���� ȿ��
                //�ǰ� �ִϸ��̼� �ʿ� 

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
        Debug.Log("�ڷ�ƾ");
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer parentSprite = transform.parent.GetComponent<SpriteRenderer>();
        parentSprite.color = new Color(87/255f, 100/255f, 100/255f); 
        spriteRenderer.color = new Color(87 / 255f, 100 / 255f, 100 / 255f);
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = new Color(1, 1, 1, 1f); 
        parentSprite.color = new Color(1, 1, 1, 1f); 


    }

}
