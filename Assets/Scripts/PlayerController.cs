using System.Collections;
using UnityEngine;

public class PlayerController : LivingEntity
{

    
    public enum State
    {
        Idle, Run, Attack, Jump, AttackRun, JumpAttack, Down, Anchor, Dash, JumpDash
        , Fall, Parrying, Up
    }


    [Header("Player")]
    [SerializeField] int hp = 3;
    [SerializeField] int mp = 0;
    [SerializeField] float axisH;
    [SerializeField] float axisV;
    [SerializeField] bool parry;
    [SerializeField] bool isShooting = false;
    [SerializeField] float parryRange = 1f;
    [SerializeField] bool parrySucess = false;
    [SerializeField] bool EXshooting = false;

    [Header("Component")]
    [SerializeField] new Rigidbody2D rigidbody;
    [SerializeField] new SpriteRenderer renderer;
    [SerializeField] Animator animator;

    [SerializeField] BulletSpawner bulletSpawner;
    [SerializeField] GameObject FootBoxCollider;
    [SerializeField] JumpEffectSpawn JumpEffectSpawn;
    [SerializeField] ParryCheck parryCheck;
    [SerializeField] DashEffectSpawn DashEffectSpawn;
    [SerializeField] Transform dashSpawn;
    [SerializeField] LayerMask parryMask;
    [SerializeField] GameObject superBar;
    [SerializeField] BarController barController;

    PooledObject bulletPrefab;
    PooledObject bulletSparkle;
    Transform spawnPos;

    [Header("Spec")]
    [SerializeField] float maxSpeed = 10.0f;
    [SerializeField] float accelPower = 13.0f;
    [SerializeField] float decelPower = 20.0f;
    [SerializeField] float jumpSpeed = 11.0f;
    [SerializeField] LayerMask groundCheckLayer;
    [SerializeField] Vector2 playerPos;
    [SerializeField] bool FootIsTrigger = false;
    [SerializeField] Vector2 bulletPos;
    [SerializeField] bool isOverlap = false;

    private Vector2 inputDir;
    private bool onGround;
    private int groundCount;
    private bool isJumping;
    private bool isParried;
    public bool downJump = false;


    private StateMachine stateMachine;

    string nowAnime = "";
    string oldAnime = "";

    void Awake()
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

        stateMachine.InitState(State.Idle); //���� ���¸� Idle ���·� ���� 

    }


    private void Start()
    {
        nowAnime = "IdlePlayer";
        oldAnime = "IdlePlayer";
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        spawnPos = transform.Find("BulletSpawn");

        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        playerPos = transform.position;
    }

    private void Update()
    {
        stateMachine.Update();



    }


    private void FixedUpdate()
    {

        stateMachine.FixedUpdate();
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundCheckLayer);




    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (groundCheckLayer.Contain(collision.gameObject.layer))
        {

            groundCount = 1;
        }





    }

    private void OnCollisionStay2D(Collision2D collision)
    {

    }


    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Checking"
            || collision.tag == "Parry")

        {
            return;
        }

        FootIsTrigger = true;


    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Checking"
            || collision.tag == "Parry")

        {
            return;
        }

        FootIsTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Checking"
            || collision.tag == "Parry")

        {
            return;
        }
        FootIsTrigger = false;




    }

    private class PlayerState : BaseState //���̽�������Ʈ ����ؼ� ���밡 �� Ŭ���� 
    {
        protected PlayerController player; //Player�� �̸� ����ϴ� stateŬ�����鿡��
        // player.hp ������ �÷��̾��� ������ �̿��� �� �ֵ��� �Ѵ�. 
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

        protected bool parrySucess { get { return player.parrySucess; } set { player.parrySucess = value; } }

        protected PooledObject bulletPrefab => player.bulletPrefab;
        protected PooledObject bulletSparkle => player.bulletSparkle;
        protected BulletSpawner bulletSpawner => player.bulletSpawner;
        protected GameObject superBar { get { return player.superBar; } set { player.superBar = value; } }

        protected BarController barController { get { return player.barController; } set { player.barController = value; } }
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
            axisV = Input.GetAxisRaw("Vertical");

            if (renderer.flipX == true) //���� ���� ������ 
            {
                spawnPos.transform.localRotation = Quaternion.Euler(0f, 0, 90);
                spawnPos.transform.localPosition = new Vector2(-0.4f, 2.7f);
            }
            else //������ ���� ������ 
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
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f ||
            Input.GetKeyUp(KeyCode.X))
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

            //�ٷ� �ٿ� ���°� ���°� ������ Ű ���� ��ȯ��Ű���� �׳� �ٷ� donw ���°� ������.
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                ChangeState(State.Jump);
            }
            else if (axisH != 0.0f)
            {
                ChangeState(State.Run);
            }

            if (!onGround && player.FootIsTrigger == false)
            {
                ChangeState(State.Fall);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                ChangeState(State.Anchor);
            }
        }
    }

    private class AttackState : PlayerState  //���¸� ������ �ƴϸ� ���� ������ �׳� ���� �ִϸ��̼� �߰�����?
    {
        public AttackState(PlayerController player) : base(player) { }



    }

    private class ParryingState : PlayerState  //자넝ㄴ망.. 8���� ��� �����ؾߵ�... �Ф�
    {
        public ParryingState(PlayerController player) : base(player) { }

        public override void Enter()
        {

            player.isParried = true;
            player.gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, 0.81f);
            player.gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(1.3f, 1.56f);
            groundCount = 0;
            player.isJumping = true;
        }

        public override void Update()
        {
            animator.Play("Parry");
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical");
            if (renderer.flipX == true)
            {
                if (axisV == 0)
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 180);
                    spawnPos.transform.localPosition = new Vector2(-1.5f, 1.2f);
                }
                else if ((axisH == 0.0f && axisV == +1.0f))  //up
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    spawnPos.transform.localPosition = new Vector2(-0.4f, 2.7f);
                }
                else if (axisH == 0.0f && axisV == -1.0f) //down 
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    spawnPos.transform.localPosition = new Vector2(0.16f, 0.25f);
                }
                else if (axisH == -1.0f && axisV == 1.0f)
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 135);
                    spawnPos.transform.localPosition = new Vector2(-1.5f, 2.1f);

                }
                else if (axisH == -1.0f && axisV == -1.0f)
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 225);
                    spawnPos.transform.localPosition = new Vector2(-0.64f, 0.8f);
                }
            }
            else //������ 
            {
                if (axisV == 0)
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    spawnPos.transform.localPosition = new Vector2(0.9f, 1.2f);
                }
                else if ((axisH == 0.0f && axisV == +1.0f))  //up
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    spawnPos.transform.localPosition = new Vector2(0.4f, 2.7f);
                }
                else if (axisH == 0.0f && axisV == -1.0f) //down 
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    spawnPos.transform.localPosition = new Vector2(0.16f, 0.25f);
                }
                else if (axisH == 1.0f && axisV == 1.0f)
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 45);
                    spawnPos.transform.localPosition = new Vector2(1.5f, 2.1f);

                }
                else if (axisH == 1.0f && axisV == -1.0f)
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, -45);
                    spawnPos.transform.localPosition = new Vector2(0.57f, 0.7f);

                }
            }
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKey(KeyCode.X))
            {
                player.StartCoroutine(player.ShootCoroutine());
            }
            if (axisH < 0.0f && rigidbody.velocity.x > -maxSpeed)
            {

                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                renderer.flipX = true;

            }
            else if (axisH > 0.0f && rigidbody.velocity.x < maxSpeed)
            {

                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                renderer.flipX = false;

            }

            if (axisH == 0 && rigidbody.velocity.x > 0.1f)
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            }
            else if (axisH == 0 && rigidbody.velocity.x < -0.1f)
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            }
        }

        public override void FixedUpdate()
        {
            player.StartCoroutine(player.ParryCheckCoroutine());
            

        }
        public override void Transition()
        {
            if (onGround && groundCount == 1)
            {
                player.gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, 1.16f);
                player.gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(1.56f, 2.26f);
                player.JumpEffectSpawn.JumpEffect();
                rigidbody.gravityScale = 1;
                groundCount = 0;
                player.isJumping = false;
                player.isParried = false;
                ChangeState(State.Idle);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ChangeState(State.Dash);
            }
            if (parrySucess == true)
            {
                ChangeState(State.Jump);
            }



        }

    }
    private class DashState : PlayerState
    {
        public DashState(PlayerController player) : base(player) { }

        public int dashSpeed = 10;
        public override void Enter()
        {

            if (renderer.flipX == false)
            {
                animator.Play("Dash");
                player.dashSpawn.transform.localPosition = new Vector2(-1f, 0.3f);
                player.DashEffectSpawn.DashEffect();
            }
            else if (renderer.flipX == true)
            {
                renderer.flipX = true;
                animator.Play("Dash");

                player.dashSpawn.transform.localPosition = new Vector2(1f, 0.3f);
                player.DashEffectSpawn.DashEffect();

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
                if (!onGround)
                {
                    rigidbody.velocity = Vector2.zero;

                    ChangeState(State.Fall);
                }
                else
                {
                    rigidbody.velocity = Vector2.zero;
                    ChangeState(State.Idle);
                }


            }
        }
    }

    private class JumpDashState : PlayerState  //��� ���� �� ���� �� ���� + �������Ʈ �ߵ� 
    {
        public JumpDashState(PlayerController player) : base(player) { }

        public int jumpDashSpeed = 10;
        public override void Enter()
        {

            if (renderer.flipX == false)
            {
                animator.Play("JumpDash");
                player.dashSpawn.transform.localPosition = new Vector2(-1f, 0.3f);
                player.DashEffectSpawn.DashEffect();
            }
            else if (renderer.flipX == true)
            {
                renderer.flipX = true;
                animator.Play("JumpDash");
                player.dashSpawn.transform.localPosition = new Vector2(1f, 0.3f);
                player.DashEffectSpawn.DashEffect();

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
                rigidbody.gravityScale = 1; //��Ȳ �̵��ÿ� �ٽ� ������1�� ��ȯ���ֱ� �߿�!
                rigidbody.velocity = Vector2.zero; //�̰� ������ ���ư��������� ���� ���·� ���� �ϱⰡ �����

                if (!onGround)
                {
                    ChangeState(State.Fall);
                }
                else
                {
                    ChangeState(State.Idle);
                }
            }

        }
    }
    private class FallState : PlayerState
    {

        //idle 상태전환 조건에 velocity.y 조건 달았음. 문제생기면 확인해볼것 
        public FallState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            Debug.Log("Fall진입");
            animator.Play("Jump");
        }


        public override void Update()
        {
            if (renderer.flipX == true) //���� ���� ������ 
            {
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 180);
                spawnPos.transform.localPosition = new Vector2(-1.5f, 1.2f);
            }
            else //������ ���� ������ 
            {
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 0);
                spawnPos.transform.localPosition = new Vector2(0.9f, 1.2f);
            }
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKey(KeyCode.X))
            {
                player.StartCoroutine(player.ShootCoroutine());
            }

            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical"); //������ �ƴ϶� �� �Ʒ� ���� ��������?

            if (axisH < 0.0f && rigidbody.velocity.x > -maxSpeed) //���� �̵�
            {
                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                renderer.flipX = true;  //�������� ��� �ٲ��ֱ�

            }
            else if (axisH > 0.0f && rigidbody.velocity.x < maxSpeed) //������ �̵� �׻� ������ �ӵ� 
            {
                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                renderer.flipX = false;  //���������� (�������� ����Ʈ)

            }
            //���ӻ��� --> �����ӵ� ���� �� ������ �ٷ� ���ߵ��� 
            if (axisH == 0 && rigidbody.velocity.x > 0.1f) //���������� �̵����� ���¿��� ���߸� 
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            }
            else if (axisH == 0 && rigidbody.velocity.x < -0.1f) //���� �̵� �� ���� 
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);

            }
            // �������� ������ �¿� �̵��� ���� 

        }
        public override void Transition()
        {
            if (onGround && rigidbody.velocity.y > -0.5)
            {
                player.JumpEffectSpawn.JumpEffect();
                ChangeState(State.Idle);
            }
            //&& player.isJumping == true)
            if (player.parryCheck.isParryed == true &&
                Input.GetKeyDown(KeyCode.Z))
            {
                ChangeState(State.Parrying);
            }

        }

    }

    private class DownState : PlayerState
    {
        public DownState(PlayerController player) : base(player) { }

        public override void Enter()
        {

            player.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            player.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            rigidbody.velocity = Vector2.zero;


        }
        public override void Update()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Down") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                animator.Play("DownIdle");
            }
            if (renderer.flipX == true) //���� ���� ������ 
            {
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 180);
                spawnPos.transform.localPosition = new Vector2(-1.6f, 0.7f);
            }
            else //������ ���� ������ 
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

            if (Input.GetKeyDown(KeyCode.Z))
            {
                player.ChangeLayer();
                onGround = false;
                ChangeState(State.Fall);
            }

            if (axisV >= 0.0f && onGround)
            {
                player.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                player.gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
                ChangeState(State.Idle);
            }

        }

    }

    private class IdleState : PlayerState
    {

        public IdleState(PlayerController player) : base(player) { }

        public override void Enter()
        {

            animator.Play("Idle");
        }

        public override void Update()
        {
            if (renderer.flipX == true)
            {
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 180);
                spawnPos.transform.localPosition = new Vector2(-1.5f, 1.2f);
            }
            else
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

            if(Input.GetKeyDown(KeyCode.V))
            {
                player.ExShoot();
            }


        }

        public override void Transition()
        {
            if (axisH != 0)
            {
                ChangeState(State.Run);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                ChangeState(State.Jump);
            }

            if (axisV < 0.0f && axisH == 0)
            {
                animator.Play("Down");
                ChangeState(State.Down);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                ChangeState(State.Anchor);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ChangeState(State.Dash);
            }
            if (!onGround && player.FootIsTrigger == false)
            {
                ChangeState(State.Fall);
            }

            if (axisV == 1.0f && axisH == 0.0f)
            {
                animator.Play("AimUp");
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                ChangeState(State.Up);
            }

        }

    }

    private class AnchorState : PlayerState
    {
        public AnchorState(PlayerController player) : base(player) { }


        public override void Enter()
        {

            rigidbody.velocity = Vector2.zero;
        }


        public override void Update()
        {
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical");
            //�Է��� ���� �ִϸ��̼� ��ȯ �ʿ� 

            if (axisH == -1.0f && axisV == 0.0f) //���� ������ �������� �ִϸ��̼� ��ȯ 
            {
                renderer.flipX = true; //���� ��ȯ 
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, -180);
                spawnPos.transform.localPosition = new Vector2(-1.5f, 1.2f);

                if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.X))
                {
                    animator.Play("AimShootStraight");
                    player.StartCoroutine(player.ShootCoroutine());
                    return;
                }

                animator.Play("AimStraight");

            }
            else if (axisH == 0.0f && axisV == 0.0f) //�ƹ��͵� �ȴ��� �� �� flip���·� �⺻ ���·� ����
            {

                if (renderer.flipX == true)  //���� ���� �ִ� ���¸� 
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, -180);
                    spawnPos.transform.localPosition = new Vector2(-1.5f, 1.2f);
                    if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.X))
                    {
                        animator.Play("AimShootStraight");
                        player.StartCoroutine(player.ShootCoroutine());
                        return;
                    }
                    animator.Play("AimStraight");

                }
                else
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    spawnPos.transform.localPosition = new Vector2(0.9f, 1.2f);
                    if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.X))
                    {
                        animator.Play("AimShootStraight");
                        player.StartCoroutine(player.ShootCoroutine());
                        return;
                    }
                    animator.Play("AimStraight");

                }
            }
            else if (axisH == 1.0f && axisV == 0.0f) //������ 
            {
                renderer.flipX = false;
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 0);
                spawnPos.transform.localPosition = new Vector2(0.9f, 1.2f);
                if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.X))
                {
                    animator.Play("AimShootStraight");
                    player.StartCoroutine(player.ShootCoroutine());
                    return;
                }

                animator.Play("AimStraight");
            }
            else if (axisH == 0.0f && axisV == 1.0f) //���� 
            {
                if (renderer.flipX == true)
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    spawnPos.transform.localPosition = new Vector2(-0.4f, 2.7f);
                }
                else
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    spawnPos.transform.localPosition = new Vector2(0.4f, 2.7f);
                }

                if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.X))
                {
                    animator.Play("AnchorShootUp");
                    player.StartCoroutine(player.ShootCoroutine());
                    return;
                }
                animator.Play("AnchorAimUp");

            }
            else if (axisH == 0.0f && axisV == -1.0f)
            {
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, -90);
                spawnPos.transform.localPosition = new Vector2(0.16f, 0.25f);

                if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.X))
                {
                    animator.Play("ShootDown");
                    player.StartCoroutine(player.ShootCoroutine());
                    return;
                }
                animator.Play("AimDown");

            }
            else if (axisH == 1.0f && axisV == 1.0f) //up diagonal ������ �밢�� �� 
            {
                renderer.flipX = false;
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 45);
                spawnPos.transform.localPosition = new Vector2(1.5f, 2.1f);

                if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.X))
                {
                    animator.Play("ShootDiagUp");
                    player.StartCoroutine(player.ShootCoroutine());
                    return;
                }
                animator.Play("AimDiagonalUp");

            }

            else if (axisH == -1.0f && axisV == 1.0f) //���� �밢�� ��
            {

                renderer.flipX = true;
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 135);
                spawnPos.transform.localPosition = new Vector2(-1.5f, 2.1f);

                if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.X))
                {
                    animator.Play("ShootDiagUp");
                    player.StartCoroutine(player.ShootCoroutine());
                    return;
                }
                animator.Play("AimDiagonalUp");

            }
            else if (axisH == 1.0f && axisV == -1.0f)  //�밢�� �Ʒ� ������ 
            {
                renderer.flipX = false;
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, -45);
                spawnPos.transform.localPosition = new Vector2(0.57f, 0.7f);
                if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.X))
                {
                    animator.Play("ShootDiagDown");
                    player.StartCoroutine(player.ShootCoroutine());
                    return;
                }
                animator.Play("AimDiagonalDown");
            }
            else if (axisH == -1.0f && axisV == -1.0f) //�밢�� �Ʒ� ���� 
            {
                renderer.flipX = true;
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 225);
                spawnPos.transform.localPosition = new Vector2(-0.64f, 0.8f);
                if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.X))
                {
                    animator.Play("ShootDiagDown");
                    player.StartCoroutine(player.ShootCoroutine());
                    return;
                }
                animator.Play("AimDiagonalDown");
            }

        }

        public override void Transition()
        {
            // c Ű�� ������ ���� ������ idle�� Ż�� 

            if (Input.GetKeyUp(KeyCode.C))
            {
                ChangeState(State.Idle);
            }

            if (axisV == 1.0 && Input.GetKeyUp(KeyCode.C))
            {

                ChangeState(State.Up);
            }



        }
    }
    private class JumpState : PlayerState
    {
        public bool isLongJump = false;

        public JumpState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            if (parrySucess == true)
            {
                ParryJump();
            }
            else
            {
                Jump();
            }
        }

        public override void Update()
        {
            if (renderer.flipX == true) //���� 
            {
                if (axisV == 0)
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 180);
                    spawnPos.transform.localPosition = new Vector2(-1.5f, 1.2f);
                }
                else if ((axisH == 0.0f && axisV == +1.0f))  //up
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    spawnPos.transform.localPosition = new Vector2(-0.4f, 2.7f);
                }
                else if (axisH == 0.0f && axisV == -1.0f) //down 
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    spawnPos.transform.localPosition = new Vector2(0.16f, 0.25f);
                }
                else if (axisH == -1.0f && axisV == 1.0f)
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 135);
                    spawnPos.transform.localPosition = new Vector2(-1.5f, 2.1f);

                }
                else if (axisH == -1.0f && axisV == -1.0f)
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 225);
                    spawnPos.transform.localPosition = new Vector2(-0.64f, 0.8f);
                }
            }
            else //������ 
            {
                if (axisV == 0)
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    spawnPos.transform.localPosition = new Vector2(0.9f, 1.2f);
                }
                else if ((axisH == 0.0f && axisV == +1.0f))  //up
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 90);
                    spawnPos.transform.localPosition = new Vector2(0.4f, 2.7f);
                }
                else if (axisH == 0.0f && axisV == -1.0f) //down 
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    spawnPos.transform.localPosition = new Vector2(0.16f, 0.25f);
                }
                else if (axisH == 1.0f && axisV == 1.0f) //�밢�� ��� ������ 
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 45);
                    spawnPos.transform.localPosition = new Vector2(1.5f, 2.1f);

                }
                else if (axisH == 1.0f && axisV == -1.0f)  //�밢�� �ϴ� ������ 
                {
                    spawnPos.transform.localRotation = Quaternion.Euler(0, 0, -45);
                    spawnPos.transform.localPosition = new Vector2(0.57f, 0.7f);

                }
            }

            if (Input.GetKeyDown(KeyCode.X) || Input.GetKey(KeyCode.X))
            {
                player.StartCoroutine(player.ShootCoroutine());  //���� �߿� ������ �ִϸ��̼��� ���ξ��� 
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
            axisV = Input.GetAxisRaw("Vertical"); //������ �ƴ϶� �� �Ʒ� ���� ��������?

            if (axisH < 0.0f && rigidbody.velocity.x > -maxSpeed) //���� �̵�
            {
                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                renderer.flipX = true;  //�������� ��� �ٲ��ֱ�
            }
            else if (axisH > 0.0f && rigidbody.velocity.x < maxSpeed) //������ �̵� �׻� ������ �ӵ� 
            {
                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                renderer.flipX = false;  //���������� (�������� ����Ʈ)
            }
            //���ӻ��� --> �����ӵ� ���� �� ������ �ٷ� ���ߵ��� 
            if (axisH == 0 && rigidbody.velocity.x > 0.1f) //���������� �̵����� ���¿��� ���߸� 
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            }
            else if (axisH == 0 && rigidbody.velocity.x < -0.1f) //���� �̵� �� ���� 
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

                player.gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, 1.16f);
                player.gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(1.56f, 2.26f);
                rigidbody.gravityScale = 1;
                groundCount = 0;
                player.JumpEffectSpawn.JumpEffect();
                player.isJumping = false;
                player.isParried = false;
                ChangeState(State.Idle);
            }
            else if (player.isJumping == true && Input.GetKeyDown(KeyCode.LeftShift)) //����Ʈ����Ʈ�� ��� ���� 
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
                player.gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(-0.02f, 0.87f);
                player.gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(1.1f, 1.43f);

                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
                groundCount = 0;
                player.isJumping = true;
            }
        }

        public void ParryJump()
        {
            Debug.Log("패리점프 함수호출");
            
            animator.Play("ParrySuccess");
            player.gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(-0.02f, 0.87f);
            player.gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(1.1f, 1.43f);
            player.parryCheck.ParryAniPlay();
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
            groundCount = 0;
            player.isJumping = true;
            parrySucess = false;

            barController.ParryCardCharge();



        }
    }

    private class RunState : PlayerState
    {
        public RunState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            animator.Play("Run");
        }

        public override void Update()
        {
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical"); //�밢�� ���� �޸��°� ����������� 


            if (axisH < 0.0f && rigidbody.velocity.x > -maxSpeed) //���� �̵�
            {

                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                //animator.Play("Run");
                renderer.flipX = true;  //�������� ��� �ٲ��ֱ�

                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 180);
                spawnPos.transform.localPosition = new Vector2(-2.0f, 1.2f);

                if (Input.GetKeyDown(KeyCode.X) || Input.GetKey(KeyCode.X))
                {
                    animator.SetBool("RunShoot", true);
                    player.StartCoroutine(player.ShootCoroutine());
                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("RunShoot") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f ||
                Input.GetKeyUp(KeyCode.X))
                {
                    animator.SetBool("RunShoot", false);

                }
            }
            else if (axisH > 0.0f && rigidbody.velocity.x < maxSpeed) //������ �̵� �׻� ������ �ӵ� 
            {

                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                //animator.Play("Run");
                renderer.flipX = false;  //���������� (�������� ����Ʈ)

                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 0);
                spawnPos.transform.localPosition = new Vector2(1.3f, 1.2f);

                if (Input.GetKeyDown(KeyCode.X) || Input.GetKey(KeyCode.X))
                {
                    animator.SetBool("RunShoot", true);
                    player.StartCoroutine(player.ShootCoroutine());
                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("RunShoot") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f ||
                Input.GetKeyUp(KeyCode.X))
                {
                    animator.SetBool("RunShoot", false);
                }
            }

            //���ӻ��� --> �����ӵ� ���� �� ������ �ٷ� ���ߵ��� 
            if (axisH == 0 && rigidbody.velocity.x > 0.02f) //���������� �̵����� ���¿��� ���߸� 
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            }
            else if (axisH == 0 && rigidbody.velocity.x < -0.02f) //���� �̵� �� ���� 
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            }
        }

        public override void Exit()
        {
            animator.Play("Down");
        }

        public override void Transition() //Ʈ�����ǿ��� �޸��鼭 ��� �޸��鼭 ���� ��� ��ȯ���� 
        {
            if (axisH == 0 && axisV == 0) //�ӵ��� 0 �� �� (�������� ���� �� idle�� ü���� ���ֱ� )
            {
                ChangeState(State.Idle);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {

                ChangeState(State.Jump);
            }

            if (Input.GetKeyDown(KeyCode.C)) //��Ŀ ���� 
            {
                ChangeState(State.Anchor);
            }
            //�޸����� �ٿ� ���� ���� ���� ���� ���߰� ����..

            if (Input.GetKeyDown(KeyCode.LeftShift)) //����Ʈ����Ʈ�� ��� ���� 
            {
                ChangeState(State.Dash);
            }
            if (!onGround && player.FootIsTrigger == false)
            {
                ChangeState(State.Fall);
            }

            if (axisV == -1)
            {
                ChangeState(State.Down);

            }

            //�� + �밢�� ���� ���� --> ���ݻ��°� �ƴϸ� �׳� �� Ű ������ �� ���°� �����ǵ��� 
            if ((axisH != 0 && axisV == 1.0f) && (Input.GetKeyDown(KeyCode.X) || Input.GetKey(KeyCode.X)))
            {
                ChangeState(State.AttackRun);
            }

            if (axisH == 0 && axisV == 1.0f)
            {
                ChangeState(State.Up);
            }


        }

    }

    private class AttackRunState : PlayerState //�� + �밢 ��� + ���� 3���� ���°� �𿩾� �ִϸ��̼� + �ҷ����� ��ġ �̵� 
    {
        public AttackRunState(PlayerController player) : base(player) { }

        public override void Enter()
        {

            animator.Play("RunShootDiagUp"); //�ִϸ��̼� ���� �صΰ� 
        }

        public override void Update()
        {
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical"); //�밢�� ���� �޸��°� ����������� 

            // �� ��� + �ҷ� ������ ��ġ ���� + �ִϸ��̼� ��� -->�ִϴ� �̹� loop�� ������̴ϱ� 
            // �ִ� ���� �� ��� �۾� + �׿� �´ºҷ� �۾��� ����.
            player.StartCoroutine(player.ShootCoroutine());

            if (axisH < 0.0f && rigidbody.velocity.x > -maxSpeed) //���� �̵�
            {
                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                renderer.flipX = true;  //�������� ��� �ٲ��ֱ�
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 135);
                spawnPos.transform.localPosition = new Vector2(-2.2f, 2.4f);

            }
            else if (axisH > 0.0f && rigidbody.velocity.x < maxSpeed) //������ �̵� �׻� ������ �ӵ� 
            {
                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                renderer.flipX = false;  //���������� (�������� ����Ʈ)
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 45);
                spawnPos.transform.localPosition = new Vector2(1.6f, 2.4f);

            }

            //���� ���� �ʼ�! �̰� ���ϸ� �̲����� �Ф� 
            if (axisH == 0 && rigidbody.velocity.x > 0.02f) //���������� �̵����� ���¿��� ���߸� 
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            }
            else if (axisH == 0 && rigidbody.velocity.x < -0.02f) //���� �̵� �� ���� 
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            }
        }

        public override void Exit()
        {
            animator.Play("Down");
        }
        public override void Transition()
        {
            if (axisH == 0) //�ϴ� y�� ���� +1�� �ƴϸ� Ż���ؾ� �ϴϱ� �ϳ� �����
            {
                ChangeState(State.Idle);
            }

            if (axisH == 0 && axisV == -1)
            {
                ChangeState(State.Down);
            }

            if (axisH != 0 && axisV != 1.0f)
            {
                ChangeState(State.Run);
            }

            if (axisV == 1.0f && axisH == 0.0f)
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                ChangeState(State.Up);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {

                ChangeState(State.Jump);
            }

            if (!onGround && player.FootIsTrigger == false)
            {
                ChangeState(State.Fall);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                ChangeState(State.Anchor);
            }
        }

    }

    public IEnumerator ShootCoroutine()
    {
        float coolTime = 0.12f; //�̰� ��� ������ ������ ��� �����ϱ� �װ� �����ؼ� ��Ÿ�� ��������� 
        if (isShooting == false)
        {
            isShooting = true;
            bulletSpawner.ObjectSpawn(); 
            yield return new WaitForSeconds(coolTime);
            isShooting = false;
        }

    }

    private IEnumerator EXshootCoroutine()
    {
        float coolTime = 1.3f; //바꿔나가자. 
        if(EXshooting == false)
        {
            EXshooting = true;
            bulletSpawner.EXShootSpawn();
            barController.EXshoot();
            yield return new WaitForSeconds(coolTime);
            EXshooting=false;
        }
       

    }

    public void ExShoot()
    {
        StartCoroutine(EXshootCoroutine());
    }



    public void ChangeLayer()
    {
        StartCoroutine(ReturnLayer());

    }


    IEnumerator ReturnLayer()
    {
        if (downJump == false)

        {
            downJump = true;


            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
                        LayerMask.NameToLayer("Flat"), true);
            yield return new WaitForSeconds(2f);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),
                        LayerMask.NameToLayer("Flat"), false);
            downJump = false;

        }


    }


    public override void OnDamage(float damage)
    {
        //어차피 모든 몬스터는 무조건 데미지를 1을 줄거니까 

        if (!dead)
        {
            //피격 효과음 재생 
            base.OnDamage(damage); // if문 내부의 사망 처리 실행 가능

        }
        hp -= (int)damage; //자신의 hp에서 감소시키기. 

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 1, 0), parryRange);
    }

    IEnumerator ParryCheckCoroutine()
    {
        if (isOverlap == false) //폴스일 때만 실행되도록 
        {
            float overLapCollTime = 0.2f;
            isOverlap = true;
            Collider2D colliders = Physics2D.OverlapCircle(transform.position + new Vector3(0, 1, 0),
               parryRange, parryMask);
            if (colliders != null)
            {
                IParry iparry = colliders.GetComponent<IParry>();
                if (iparry != null)
                {
                    iparry.Parried();
                }
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
                parrySucess = true;
            }
            yield return new WaitForSecondsRealtime(overLapCollTime);
            isOverlap = false;
        }
    }




    
}


