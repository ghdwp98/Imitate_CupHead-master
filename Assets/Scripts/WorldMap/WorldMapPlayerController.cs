using UnityEngine;

public class WorldMapPlayerController : MonoBehaviour
{
    //8방향 이동과 그에 맞는 애니메이션 구현 

    private Rigidbody2D playerRb;
    private Animator myAnim;
    [SerializeField] float playerMoveSpeed;
    [SerializeField] float breakPower;
    [SerializeField] float accelPower;

    float delay = 0f;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

        //여기서 할 때 클리어 했으면 승리 애니메이션을 진행해보자. 

        if (Manager.Game.playerWin == true)
        {
            myAnim.Play("Win");

            Manager.Game.playerWin = false;
        }



    }

    private void FixedUpdate()
    {
        delay += Time.deltaTime;
        if (delay > 3f)
        {
            myAnim.SetTrigger("WinTrigger");
        }




        playerRb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized *
            playerMoveSpeed * Time.deltaTime;

        myAnim.SetFloat("MoveX", playerRb.velocity.x);
        myAnim.SetFloat("MoveY", playerRb.velocity.y);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1
            || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            myAnim.SetFloat("LastMoveX", Input.GetAxisRaw("Horizontal"));
            myAnim.SetFloat("LastMoveY", Input.GetAxisRaw("Vertical"));
        }


    }
}
