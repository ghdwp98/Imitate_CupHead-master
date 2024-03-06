using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //각각의 상태 클래스에서 뭘 할지 결정하고 어떤 상황이 되면 다른 상태로 트랜지션 할지 결정 
    //이동은 그냥 고전시스템으로 처리하자 그게 훨씬 쉬울것 같다. 
    // groundCheck가 필요한 친구들은 피봇을 바텀으로 두고 쓰자 

    public enum State { Idle, Run, Attack, Jump, attackRun, JumpAttack, Down }
    //앵커 c키 누르면 이동 없이  8방향 조준 전환 가능 

    [Header("Player")]
    [SerializeField] int hp = 3;
    [SerializeField] int mp = 0;
    [SerializeField] float axisH;
    [SerializeField] float axisV;

    [Header("Component")]
    [SerializeField] new Rigidbody2D rigidbody;
    [SerializeField] new SpriteRenderer renderer;
    [SerializeField] Animator animator;

    [Header("Spec")]
    [SerializeField] float maxSpeed = 5.0f;
    [SerializeField] float accelPower = 10.0f;
    [SerializeField] float decelPower = 20.0f;
    [SerializeField] float jumpSpeed = 15.0f;
    [SerializeField] LayerMask groundCheckLayer; //땅위에서만 점프 가능 or 패리 위에서만 점프가능 
    [SerializeField] Vector2 playerPos;

    private Vector2 inputDir;
    private bool onGround;
    private int groundCount;


    private StateMachine stateMachine;

    string nowAnime = "";
    string oldAnime = "";

    //이동 앉기 대시 점프 조준 슈팅 -->상태머신 구현 

    void Awake() //시작 시에 상태를 추가 하고 시작 (딕셔너리에 add함 )
    {
        stateMachine = gameObject.AddComponent<StateMachine>();
        stateMachine.AddState(State.Idle, new IdleState(this));
        stateMachine.AddState(State.Run, new RunState(this));
        stateMachine.AddState(State.Attack, new AttackState(this));
        stateMachine.AddState(State.Jump, new JumpState(this));
        stateMachine.AddState(State.Down, new DownState(this));

        stateMachine.InitState(State.Idle); //최초 상태를 Idle 상태로 시작 

    }
    private void Start()
    {
        nowAnime = "IdlePlayer";
        oldAnime = "IdlePlayer";
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        playerPos = transform.position;

    }

    private void Update()
    {
        stateMachine.Update(); //업데이트마다 스테이트머신을 업데이트 시켜줘야함 
                               //curState의 update와 transition을 담당하고 있는 statemachine의 update 함수 

        

    }

    private void FixedUpdate()
    {
        //플레이어가 리지드바디를 쓰면 픽스드업데이트가 필요할 경우가 있음
        stateMachine.FixedUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(groundCheckLayer.Contain(collision.gameObject.layer))
        {
            groundCount = 1;
        }
    }


    private class PlayerState : BaseState //베이스스테이트 상속해서 뼈대가 될 클래스 
    {
        protected PlayerController player; //Player로 이를 상속하는 state클래스들에서
        // player.hp 등으로 플레이어의 변수를 이용할 수 있도록 한다. 
        protected Transform transform => player.transform;
        protected int hp { get { return player.hp; } set { player.hp = value; } }
        protected int mp { get { return player.mp; } set { player.mp = value; } }
        protected float axisH { get { return player.axisH; } set { player.axisH = value; } }
        protected float axisV { get { return player.axisV; } set { player.axisV = value; } }
        protected Rigidbody2D rigidbody => player.rigidbody;
        protected SpriteRenderer renderer => player.renderer;
        protected Animator animator => player.animator;
        protected float maxSpeed => player.maxSpeed;
        protected float decelPower => player.decelPower;
        protected float accelPower => player.accelPower;
        protected float jumpSpeed => player.jumpSpeed;
        protected LayerMask groundCheckLayer => player.groundCheckLayer;
        protected Vector2 playerPos => player.playerPos;
        protected bool onGround { get { return player.onGround; } set { player.onGround = value; } }
        protected int groundCount { get { return player.groundCount; } set { player.groundCount = value; } }

        public PlayerState(PlayerController player)
        {
            this.player = player;
        }

    }


    private class DownState : PlayerState //숙인 상태에서는 움직임 불가 but 좌우 전환은가능 
    {
        public DownState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            animator.Play("DownIdlePlayer");
        }

        public override void Update()
        {
            animator.Play("DownPlayer");
        }

        public override void Transition()
        {
            if(axisV>=0.0f)
            {
                ChangeState(State.Idle);
            }
        }

    }



    private class IdleState : PlayerState  //이러면 필수매개변수 player가 없다고 나온다. 위에서 생성자를 만들면 
    {
        //자식이 부모 클래스의 생성자를 강제로 호출 Base 이용 
        public IdleState(PlayerController player) : base(player) { }

        public override void Update() //계속 돌아가면서 체크 업데이트 + 트랜지션 
        {
            animator.Play("IdlePlayer");
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical"); //점프가 아니라 위 아래 보는 느낌으로?


        }


        public override void Transition()
        {
            if (axisH != 0) //이동이 있으면 상태전환 
            {

                ChangeState(State.Run);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                ChangeState(State.Jump);
            }
            
            if (axisV<0.0f)
            {
                ChangeState(State.Down);
            }


        }
    }

    

    private class JumpState : PlayerState
    {
        public JumpState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            Jump();
        }

        public override void Update()
        {

            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical"); //점프가 아니라 위 아래 보는 느낌으로?

            if (axisH < 0.0f && rigidbody.velocity.x > -maxSpeed) //왼쪽 이동
            {

                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                renderer.flipX = true;  //왼쪽으로 모습 바꿔주기

            }
            else if (axisH > 0.0f && rigidbody.velocity.x < maxSpeed) //오른쪽 이동 항상 일정한 속도 
            {

                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                renderer.flipX = false;  //오른쪽으로 (오른쪽이 디폴트)

            }
            //감속상태 --> 일정속도 유지 및 정지시 바로 멈추도록 
            if (axisH == 0 && rigidbody.velocity.x > 0.1f) //오른쪽으로 이동중인 상태에서 멈추면 
            {
                

                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            }
            else if (axisH == 0 && rigidbody.velocity.x < -0.1f) //왼쪽 이동 중 정지 
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                
            }
        }

        public override void FixedUpdate()
        {
            onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundCheckLayer);

        }
        public override void Transition()
        {
           if(onGround&&groundCount==1)
            {
                ChangeState(State.Idle);
            }
        }

        public void Jump()
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
            animator.Play("JumpPlayer");
            groundCount = 0;
        }
    }

    private class AttackState : PlayerState
    {
        public AttackState(PlayerController player) : base(player) { }

    }

    private class RunState : PlayerState
    {
        public RunState(PlayerController player) : base(player) { }

        //엑셀로 일정한 속도 유지 할 수 있도록 
        public override void Update()
        {
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical"); //점프가 아니라 위 아래 보는 느낌으로?

            if (axisH < 0.0f && rigidbody.velocity.x > -maxSpeed) //왼쪽 이동
            {

                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                animator.Play("RunPlayer");
                renderer.flipX = true;  //왼쪽으로 모습 바꿔주기

            }
            else if (axisH > 0.0f && rigidbody.velocity.x < maxSpeed) //오른쪽 이동 항상 일정한 속도 
            {


                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                animator.Play("RunPlayer");
                renderer.flipX = false;  //오른쪽으로 (오른쪽이 디폴트)

            }
            //감속상태 --> 일정속도 유지 및 정지시 바로 멈추도록 
            if (axisH == 0 && rigidbody.velocity.x > 0.1f) //오른쪽으로 이동중인 상태에서 멈추면 
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            }
            else if (axisH == 0 && rigidbody.velocity.x < -0.1f) //왼쪽 이동 중 정지 
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            }
        }
        public override void Transition() //트랜지션에서 달리면서 쏘기 달리면서 점프 등등 전환구현 
        {
            if (rigidbody.velocity.x == 0.0f) //속도가 0 일 때 (움직임이 없을 때 idle로 체인지 해주기 )
            {
                ChangeState(State.Idle);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {

                ChangeState(State.Jump);
            }
        }

    }




}
