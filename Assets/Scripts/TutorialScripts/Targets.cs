using System.Collections;
using UnityEngine;

public class Targets : LivingEntity
{
    public float hp = 10f;
    [SerializeField] GameObject parent;
    Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
        health = hp;
    }

    private void Update()
    {
        
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
            }
            
        }

    }
    public override void Die()
    {
        
        StartCoroutine(Destorys());
        base.Die();

    }

    IEnumerator OnHit()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer parentSprite = transform.parent.GetComponent<SpriteRenderer>();
        parentSprite.color = new Color(87/255f, 100/255f, 100/255f); 
        spriteRenderer.color = new Color(87 / 255f, 100 / 255f, 100 / 255f);
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = new Color(1, 1, 1, 1f); 
        parentSprite.color = new Color(1, 1, 1, 1f); 

    }

    IEnumerator Destorys()
    {
        Animator parentAnimator=transform.parent.GetComponent<Animator>();
        animator.Play("Explosion"); //�ı� �ִϸ��̼� 
        parentAnimator.Play("Explosion2");
        yield return new WaitForSeconds(1f);
        if (parent != null)
        {
            parent.SetActive(false);
        }
    }

}
