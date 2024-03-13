using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator= GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animator.Play("CoinDeathAni");
            //게임매니저? 에 코인 숫자 추가..
            CoinManager();
            Destroy(gameObject, 1f);


        }
           
    }

    private void CoinManager()
    {
        Manager.Game.SetScore(1);
        Manager.Game.Test();
    }



}
