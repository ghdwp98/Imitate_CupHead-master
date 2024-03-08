
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //각각의 상태 클래스에서 뭘 할지 결정하고 어떤 상황이 되면 다른 상태로 트랜지션 할지 결정 
    //이동은 그냥 고전시스템으로 처리하자 그게 훨씬 쉬울것 같다. 
    // groundCheck가 필요한 친구들은 피봇을 바텀으로 두고 쓰자 
    // 좌 쉬프트로 대시 공중 + 땅 2가지 

    public enum State
    {
        Idle, Run, Attack, Jump, AttackRun, JumpAttack, Down, Anchor, Dash, JumpDash
    , Fall, Parrying, Up
    }
    //앵커 c키 누르면 이동 없이  8방향 조준 전환 가능 

    [Header("Player")]
    [SerializeField] int hp = 3;
    [SerializeField] int mp = 0;
    [SerializeField] float axisH;
    [SerializeField] float axisV;
    [SerializeField] bool parry;
    [SerializeField] bool isShooting = false;

    [Header("Component")]
    [SerializeField] new Rigidbody2D rigidbody;
    [SerializeField] new SpriteRenderer renderer;
    [SerializeField] Animator animator;

    [SerializeField] BulletSpawner bulletSpawner;
    [SerializeField] GameObject FootBoxCollider;
    [SerializeField] ParryCheck parryCheck;

    [SerializeField] PooledObject bulletPrefab;
    [SerializeField] PooledObject bulletSparkle;
    [SerializeField] Transform spawnPos;

    [Header("Spec")]
    [SerializeField] float maxSpeed = 5.0f;
    [SerializeField] float accelPower = 10.0f;
    [SerializeField] float decelPower = 20.0f;
    [SerializeField] float jumpSpeed = 10.0f;
    [SerializeField] LayerMask groundCheckLayer; //땅위에서만 점프 가능 or 패리 위에서만 점프가능 
    [SerializeField] Vector2 playerPos;
    [SerializeField] bool FootIsTrigger = false;
    [SerializeField] Vector2 bulletPos;


    private Vector2 inputDir;
    private bool onGround;
    private int groundCount;
    private bool isJumping;
    private bool isParried;


    private StateMachine stateMachine;

    string nowAnime = "";
    string oldAnime = "";

    //이동 앉기 대시 점프 조준 슈팅 -->상태머신 구현 
    //그라운드 오브젝트 옆에서 그라운드로 판단되서 점프 모션 사라지는거 고쳐야함 

    void Awake() //시작 시에 상태를 추가 하고 시작 (딕셔너리에 add함 )
    {
        stateMachine = gameObject.AddComponent<StateMachine>();
        stateMachine.AddState(State.Idle, new IdleState(this));
        stateMachine.AddState(State.Run, new RunState(this));
        stateMachine.AddState(State.Attack, new AttackState(this));
        stateMachine.AddState(State.AttackRun, new AttackRunState(this));
        stateMachine.AddState(State.Jump, new JumpState(this));
        stateMachine.AddState(State.Down, new DownState(this));
        stateMachine.AddState(State.Anchor, new AnchorState(this));
        stateMachine.AddState(State.Dash, new DashState(this));
        stateMachine.AddState(State.JumpDash, new JumpDashState(this));
        stateMachine.AddState(State.Fall, new FallState(this));
        stateMachine.AddState(State.Parrying, new ParryingState(this));
        stateMachine.AddState(State.Up, new UpState(this));

        stateMachine.InitState(State.Idle); //최초 상태를 Idle 상태로 시작 

    }

    [System.Obsolete]
    private void Start()
    {
        nowAnime = "IdlePlayer";
        oldAnime = "IdlePlayer";
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        spawnPos = transform.FindChild("BulletSpawn");
        Debug.Log(spawnPos);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
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
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundCheckLayer);




    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (groundCheckLayer.Contain(collision.gameObject.layer))
        {
            Debug.Log("접촉");
            groundCount = 1;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FootIsTrigger = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        FootIsTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        FootIsTrigger = false;
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

        protected bool parry { get { return player.parry; } set { player.parry = value; } }
        protected Transform spawnPos { get { return player.spawnPos; } set { player.spawnPos = value; } }

        protected PooledObject bulletPrefab => player.bulletPrefab;
        protected PooledObject bulletSparkle => player.bulletSparkle;
        protected BulletSpawner bulletSpawner => player.bulletSpawner;


        public PlayerState(PlayerController player)
        {
            this.player = player;
        }

    }

    private class UpState : PlayerState
    {
        public UpState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            animator.Play("AimUp");
        }
        public override void Update()
        {
            axisH = Input.GetAxisRaw("Horizontal");

            if (renderer.flipX == true) //왼쪽 보고 있으면 
            {
                spawnPos.transform.localRotation = Quaternion.Euler(0f, 0, 90);
                spawnPos.transform.localPosition = new Vector2(-0.4f, 2.7f);
            }
            else //오른쪽 보고 있으면 
            {
                spawnPos.transform.localRotation = Quaternion.Euler(0f, 0, 90);
                spawnPos.transform.localPosition = new Vector2(0.4f, 2.7f);
            }

            


            if (Input.GetKeyDown(KeyCode.X) || Input.GetKey(KeyCode.X))
            {
                animator.SetBool("ShootUp", true);
                player.StartCoroutine(player.ShootCoroutine());
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("ShootUp") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                animator.SetBool("ShootUp", false);
            }
        }
        public override void Transition()
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                ChangeState(State.Idle);
            }
            //바로 다운 가는건 없는게 나은듯 키 떼고 전환시키도록 그냥 바로 donw 가는건 없애자.
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                ChangeState(State.Jump);
            }
            else if (!onGround && player.FootIsTrigger == false)
            {
                ChangeState(State.Fall);
            }
            else if (axisH != 0.0f)
            {
                ChangeState(State.Run);
            }
        }
    }

    private class AttackState : PlayerState  //상태를 만들지 아니면 상태 내에서 그냥 어택 애니메이션 추가할지?
    {
        public AttackState(PlayerController player) : base(player) { }



    }

    private class AttackRunState : PlayerState
    {
        public AttackRunState(PlayerController player) : base(player) { }



    }

    private class ParryingState : PlayerState
    {
        public ParryingState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            Debug.Log("패링");
            player.isParried = true;
            player.gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, 0.81f);
            player.gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(1.3f, 1.56f);
            groundCount = 0;
            player.isJumping = true; //isjumping은 나중에 parry상황 체크할 때 사용하자. 
        }

        public override void Update()
        {
            //패리 중 패리성공 하던 뭐 하던 그런 상황들 넣어주기 
            animator.Play("Parry");
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

        public override void Transition()
        {
            if (onGround && groundCount == 1)
            {
                Debug.Log("패리 끝 ");
                player.gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, 1.16f);
                player.gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(1.56f, 2.26f);
                rigidbody.gravityScale = 1;
                groundCount = 0;
                player.isJumping = false;
                player.isParried = false;
                ChangeState(State.Idle);
            }
        }

    }  //패링 중에서 공격 되는지 확인해보기 

    private class DashState : PlayerState
    {
        public DashState(PlayerController player) : base(player) { }

        public int dashSpeed = 10;
        public override void Enter()
        {

            if (renderer.flipX == false)
            {
                animator.Play("Dash");
            }
            else if (renderer.flipX == true)
            {
                renderer.flipX = true;
                animator.Play("Dash");

            }
        }

        public override void FixedUpdate()
        {
            rigidbody.velocity = new Vector2(dashSpeed, 0);

            if (renderer.flipX == true)
            {
                rigidbody.velocity = new Vector2(-dashSpeed, 0);

            }
        }

        public override void Transition()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dash") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                rigidbody.velocity = Vector2.zero;
                ChangeState(State.Idle); //임시로 탈출용 
            }
        }
    }

    private class JumpDashState : PlayerState
    {
        public JumpDashState(PlayerController player) : base(player) { }

        public int jumpDashSpeed = 10;
        public override void Enter()
        {
            if (renderer.flipX == false)
            {
                animator.Play("JumpDash");
            }
            else if (renderer.flipX == true)
            {
                renderer.flipX = true;
                animator.Play("JumpDash");

            }

        }

        public override void FixedUpdate()
        {
            rigidbody.velocity = new Vector2(jumpDashSpeed, 0);
            rigidbody.gravityScale = 0;
            if (renderer.flipX == true)
            {
                rigidbody.velocity = new Vector2(-jumpDashSpeed, 0);

            }
        }

        public override void Transition()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("JumpDash") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                rigidbody.gravityScale = 1; //상황 이동시에 다시 스케일1로 변환해주기 중요!
                rigidbody.velocity = Vector2.zero; //이게 없으면 날아가버리지만 폴링 상태로 진입 하기가 힘드네

                if (!onGround)
                {
                    ChangeState(State.Fall);
                }
                else
                {
                    ChangeState(State.Idle); //점프대시는 Fall 상태로 전환해주자 
                }
            }

        }
    }

    private class FallState : PlayerState
    {
        public FallState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            animator.Play("Jump");
        }

        //떨어지는 상황에서도 딱히 다른 애니메이션이 없으므로 (점프애니메이션 이므로)

        public override void Update()
        {
            if (renderer.flipX == true) //왼쪽 보고 있으면 
            {
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 180);
                spawnPos.transform.localPosition = new Vector2(-1.5f, 1.2f);
            }
            else //오른쪽 보고 있으면 
            {
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 0);
                spawnPos.transform.localPosition = new Vector2(0.9f, 1.2f);
            }
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKey(KeyCode.X))
            {
                player.StartCoroutine(player.ShootCoroutine());
            }

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
            // 떨어지는 동안은 좌우 이동만 가능 

        }
        public override void Transition()
        {
            if (onGround)
            {
                ChangeState(State.Idle);
            }

        }

    }

    private class DownState : PlayerState //숙인 상태에서는 움직임 불가 but 좌우 전환은가능 
    {
        public DownState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            player.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            player.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            rigidbody.velocity = Vector2.zero;

            // Idle Exit에서 Down 애니메이션 재생했음 
        }
        public override void Update()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Down") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                animator.Play("DownIdle");
            }
            if (renderer.flipX == true) //왼쪽 보고 있으면 
            {
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 180);
                spawnPos.transform.localPosition = new Vector2(-1.6f, 0.7f);
            }
            else //오른쪽 보고 있으면 
            {
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 0);
                spawnPos.transform.localPosition = new Vector2(1.3f, 0.7f);
            }

            if (Input.GetKeyDown(KeyCode.X) || Input.GetKey(KeyCode.X))
            {
                animator.SetBool("ShootDuck", true);
                player.StartCoroutine(player.ShootCoroutine());
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("DuckShoot") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                animator.SetBool("ShootDuck", false);
            }

            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical");

            if (axisH < 0.0f)
            {
                renderer.flipX = true;
            }
            else if (axisH > 0.0f)
            {
                renderer.flipX = false;

            }
        }

        public override void Transition()
        {
            if (axisV >= 0.0f)
            {
                player.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                player.gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
                ChangeState(State.Idle);
            }
        }

    }

    private class IdleState : PlayerState
    {
        //자식이 부모 클래스의 생성자를 강제로 호출 Base 이용 
        public IdleState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            animator.Play("Idle");

        }

        public override void Update() //계속 돌아가면서 체크 업데이트 + 트랜지션 
        {
            if (renderer.flipX == true) //왼쪽 보고 있으면 
            {
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 180);
                spawnPos.transform.localPosition = new Vector2(-1.5f, 1.2f);
            }
            else //오른쪽 보고 있으면 
            {
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 0);
                spawnPos.transform.localPosition = new Vector2(0.9f, 1.2f);
            }

            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.X) || Input.GetKey(KeyCode.X))
            {
                animator.SetBool("ShootStraight", true);
                player.StartCoroutine(player.ShootCoroutine());
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("ShootStraight") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                animator.SetBool("ShootStraight", false);
            }
        }

        public override void Exit()
        {
            animator.Play("Down");
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

            if (axisV < 0.0f)
            {

                ChangeState(State.Down);
            }

            if (Input.GetKeyDown(KeyCode.C)) //앵커 상태 
            {
                ChangeState(State.Anchor);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift)) //레프트쉬프트로 대시 구현 
            {
                ChangeState(State.Dash);
            }
            if (!onGround && player.FootIsTrigger == false)
            {
                ChangeState(State.Fall);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                ChangeState(State.Up);
            }
        }

    }

    private class AnchorState : PlayerState
    {
        public AnchorState(PlayerController player) : base(player) { }

        //앵커상황이 되면 조준 애니메이션이 나와줘야함 

        public override void Enter()
        {
            rigidbody.velocity = Vector2.zero;


        }

        public override void Update()
        {
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical");
            //입력을 통해 애니메이션 전환 필요 

            if (axisH == -1.0f && axisV == 0.0f) //왼쪽 누르면 왼쪽으로 애니메이션 전환 
            {
                animator.Play("AimStraight");
                renderer.flipX = true; //방향 전환 
            }
            else if (axisH == 0.0f && axisV == 0.0f)
            {
                animator.Play("AimStraight");
            }
            else if (axisH == 1.0f && axisV == 0.0f)
            {
                animator.Play("AimStraight");
                renderer.flipX = false;
            }
            else if (axisH == 0.0f && axisV == 1.0f)
            {
                animator.Play("AimUp");
            }
            else if (axisH == 0.0f && axisV == -1.0f)
            {
                animator.Play("AimDown");
            }
            else if (axisH == 1.0f && axisV == 1.0f) //up diagonal 
            {
                animator.Play("AimDiagonalUp");
                renderer.flipX = false;
            }
            else if (axisH == -1.0f && axisV == 1.0f) // -1 1
            {
                animator.Play("AimDiagonalUp");
                renderer.flipX = true;

            }
            else if (axisH == 1.0f && axisV == -1.0f)
            {
                renderer.flipX = false;
                animator.Play("AimDiagonalDown");
            }
            else if (axisH == -1.0f && axisV == -1.0f)
            {
                renderer.flipX = true;
                animator.Play("AimDiagonalDown");
            }
        }

        public override void Transition()
        {
            // c 키를 누르고 있지 않으면 idle로 탈출 

            if (Input.GetKeyUp(KeyCode.C))
            {
                ChangeState(State.Idle);
            }


        }
    }
    private class JumpState : PlayerState
    {
        public bool isLongJump = false;

        public JumpState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            Jump();
        }

        public override void Update()
        {
            if (renderer.flipX == true) //왼쪽 보고 있으면 
            {
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 180);
                spawnPos.transform.localPosition = new Vector2(-1.5f, 1.2f);
            }
            else //오른쪽 보고 있으면 
            {
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 0);
                spawnPos.transform.localPosition = new Vector2(0.9f, 1.2f);
            }
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKey(KeyCode.X))
            {
                player.StartCoroutine(player.ShootCoroutine());  //점프 중에 슈팅은 애니메이션이 따로없음 
            }

            if (Input.GetKey(KeyCode.Z))
            {
                isLongJump = true;
            }
            else if (Input.GetKeyUp(KeyCode.Z))
            {
                isLongJump = false;
            }

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
            if (isLongJump)
            {
                rigidbody.gravityScale = 1.0f;
            }
            else
            {
                rigidbody.gravityScale = 1.7f;
            }
        }
        public override void Transition()
        {
            if (onGround && groundCount == 1)
            {
                Debug.Log("점프 끝");
                player.gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, 1.16f);
                player.gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(1.56f, 2.26f);
                rigidbody.gravityScale = 1;
                groundCount = 0;
                player.isJumping = false;
                player.isParried = false;
                ChangeState(State.Idle);
            }
            else if (player.isJumping == true && Input.GetKeyDown(KeyCode.LeftShift)) //레프트쉬프트로 대시 구현 
            {
                ChangeState(State.JumpDash);
            }
            else if (player.parryCheck.isParryed == true &&
                Input.GetKeyDown(KeyCode.Z) &&
                player.isJumping == true)
            {
                ChangeState(State.Parrying);
            }
        }

        public void Jump()
        {
            //if (player.isJumping == false)
            {
                animator.Play("Jump");
                player.gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, 0.81f);
                player.gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(1.3f, 1.56f);
                Debug.Log("점프");
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
                groundCount = 0;
                player.isJumping = true; //isjumping은 나중에 parry상황 체크할 때 사용하자. 
            }


        }
    }



    private class RunState : PlayerState
    {
        public RunState(PlayerController player) : base(player) { }

        //엑셀로 일정한 속도 유지 할 수 있도록 

        public override void Enter()
        {
            animator.Play("Run");
        }

        public override void Update()
        {
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical"); //대각선 위로 달리는거 구현해줘야함 
            

            if (axisH < 0.0f && rigidbody.velocity.x > -maxSpeed&&axisV==0.0f) //왼쪽 이동
            {

                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                //animator.Play("Run");
                renderer.flipX = true;  //왼쪽으로 모습 바꿔주기

                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 180);
                spawnPos.transform.localPosition = new Vector2(-1.8f, 1.2f);

                if (Input.GetKeyDown(KeyCode.X) || Input.GetKey(KeyCode.X))
                {
                    animator.SetBool("RunShoot", true);
                    player.StartCoroutine(player.ShootCoroutine());
                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("RunShoot") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f||
                Input.GetKeyUp(KeyCode.X))
                {
                    animator.SetBool("RunShoot", false);
                    
                }
            }
            else if (axisH > 0.0f && rigidbody.velocity.x < maxSpeed&&axisV==0.0f) //오른쪽 이동 항상 일정한 속도 
            {

                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                //animator.Play("Run");
                renderer.flipX = false;  //오른쪽으로 (오른쪽이 디폴트)

                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 0);
                spawnPos.transform.localPosition = new Vector2(1.1f, 1.2f);

                if (Input.GetKeyDown(KeyCode.X) || Input.GetKey(KeyCode.X))
                {
                    animator.SetBool("RunShoot", true);
                    player.StartCoroutine(player.ShootCoroutine());
                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("RunShoot") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f||
                Input.GetKeyUp(KeyCode.X))
                {
                    animator.SetBool("RunShoot", false);
                }
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

            if (Input.GetKeyDown(KeyCode.C)) //앵커 상태 
            {
                ChangeState(State.Anchor);
            }
            //달리는중 다운 상태 구현 하지 말자 멈추고 하자..

            if (Input.GetKeyDown(KeyCode.LeftShift)) //레프트쉬프트로 대시 구현 
            {
                ChangeState(State.Dash);
            }
            if (!onGround && player.FootIsTrigger == false)
            {
                ChangeState(State.Fall);
            }
            

        }

    }




    public IEnumerator ShootCoroutine()
    {
        float coolTime = 0.1f; //이거 계속 누르고 있으면 계속 나가니까 그거 생각해서 쿨타임 정해줘야함 
        if (isShooting == false)
        {
            isShooting = true;
            bulletSpawner.ObjectSpawn();
            yield return new WaitForSeconds(coolTime);
            isShooting = false;
        }

    }



}
