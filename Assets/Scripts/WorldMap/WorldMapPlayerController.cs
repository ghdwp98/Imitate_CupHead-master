using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapPlayerController : MonoBehaviour
{
    //8방향 이동과 그에 맞는 애니메이션 구현 

    private Rigidbody2D playerRb;
    private Animator myAnim;
    [SerializeField] float playerMoveSpeed;
    [SerializeField] float breakPower;
    [SerializeField] float accelPower;
    
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        playerRb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized*
            playerMoveSpeed*Time.deltaTime;

        myAnim.SetFloat("MoveX", playerRb.velocity.x);
        myAnim.SetFloat("MoveY", playerRb.velocity.y);

        if(Input.GetAxisRaw("Horizontal")==1||Input.GetAxisRaw("Horizontal")==-1
            || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            myAnim.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));
            myAnim.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
        }


    }
}
