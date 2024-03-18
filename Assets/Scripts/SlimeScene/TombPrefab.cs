using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombPrefab : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject slime;
    public bool isCollid;
    [SerializeField] bool onGround;
    [SerializeField] LayerMask groundCheckLayer;




    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        isCollid = false;
        slime = GameObject.Find("Slime");
    }

    void Update()
    {
        //만약 태그 바꿔줘야 하면 그라운드에 닿은 이후에 

        
    }

    private void FixedUpdate()
    {
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundCheckLayer);

        if (onGround==true)
        {
            rb.velocity= Vector2.zero;
            
        }
        else
        {
            rb.AddForce(Vector2.up * -30);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            SlimeBoss slimeBoss = slime.gameObject.GetComponent<SlimeBoss>();

            if (slimeBoss != null)
            {
                slimeBoss.TombCollid = true;

            }

        }
    }



}
