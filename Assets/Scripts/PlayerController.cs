
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //������ ���� Ŭ�������� �� ���� �����ϰ� � ��Ȳ�� �Ǹ� �ٸ� ���·� Ʈ������ ���� ���� 
    //�̵��� �׳� �����ý������� ó������ �װ� �ξ� ����� ����. 
    // groundCheck�� �ʿ��� ģ������ �Ǻ��� �������� �ΰ� ���� 
    // �� ����Ʈ�� ��� ���� + �� 2���� 

    public enum State
    {
        Idle, Run, Attack, Jump, AttackRun, JumpAttack, Down, Anchor, Dash, JumpDash
    , Fall, Parrying, Up
    }
    //��Ŀ cŰ ������ �̵� ����  8���� ���� ��ȯ ���� 

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
    [SerializeField] LayerMask groundCheckLayer; //���������� ���� ���� or �и� �������� �������� 
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

    //�̵� �ɱ� ��� ���� ���� ���� -->���¸ӽ� ���� 
    //�׶��� ������Ʈ ������ �׶���� �ǴܵǼ� ���� ��� ������°� ���ľ��� 

    void Awake() //���� �ÿ� ���¸� �߰� �ϰ� ���� (��ųʸ��� add�� )
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
        stateMachine.Update(); //������Ʈ���� ������Ʈ�ӽ��� ������Ʈ ��������� 
                               //curState�� update�� transition�� ����ϰ� �ִ� statemachine�� update �Լ�
    }


    private void FixedUpdate()
    {
        //�÷��̾ ������ٵ� ���� �Ƚ��������Ʈ�� �ʿ��� ��찡 ����
        stateMachine.FixedUpdate();
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundCheckLayer);




    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (groundCheckLayer.Contain(collision.gameObject.layer))
        {
            Debug.Log("����");
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
            //�ٷ� �ٿ� ���°� ���°� ������ Ű ���� ��ȯ��Ű���� �׳� �ٷ� donw ���°� ������.
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

    private class AttackState : PlayerState  //���¸� ������ �ƴϸ� ���� ������ �׳� ���� �ִϸ��̼� �߰�����?
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
            Debug.Log("�и�");
            player.isParried = true;
            player.gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, 0.81f);
            player.gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(1.3f, 1.56f);
            groundCount = 0;
            player.isJumping = true; //isjumping�� ���߿� parry��Ȳ üũ�� �� �������. 
        }

        public override void Update()
        {
            //�и� �� �и����� �ϴ� �� �ϴ� �׷� ��Ȳ�� �־��ֱ� 
            animator.Play("Parry");
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

        public override void Transition()
        {
            if (onGround && groundCount == 1)
            {
                Debug.Log("�и� �� ");
                player.gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, 1.16f);
                player.gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(1.56f, 2.26f);
                rigidbody.gravityScale = 1;
                groundCount = 0;
                player.isJumping = false;
                player.isParried = false;
                ChangeState(State.Idle);
            }
        }

    }  //�и� �߿��� ���� �Ǵ��� Ȯ���غ��� 

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
                ChangeState(State.Idle); //�ӽ÷� Ż��� 
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
                rigidbody.gravityScale = 1; //��Ȳ �̵��ÿ� �ٽ� ������1�� ��ȯ���ֱ� �߿�!
                rigidbody.velocity = Vector2.zero; //�̰� ������ ���ư��������� ���� ���·� ���� �ϱⰡ �����

                if (!onGround)
                {
                    ChangeState(State.Fall);
                }
                else
                {
                    ChangeState(State.Idle); //������ô� Fall ���·� ��ȯ������ 
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

        //�������� ��Ȳ������ ���� �ٸ� �ִϸ��̼��� �����Ƿ� (�����ִϸ��̼� �̹Ƿ�)

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
            if (onGround)
            {
                ChangeState(State.Idle);
            }

        }

    }

    private class DownState : PlayerState //���� ���¿����� ������ �Ұ� but �¿� ��ȯ������ 
    {
        public DownState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            player.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            player.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            rigidbody.velocity = Vector2.zero;

            // Idle Exit���� Down �ִϸ��̼� ������� 
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
        //�ڽ��� �θ� Ŭ������ �����ڸ� ������ ȣ�� Base �̿� 
        public IdleState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            animator.Play("Idle");

        }

        public override void Update() //��� ���ư��鼭 üũ ������Ʈ + Ʈ������ 
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
            if (axisH != 0) //�̵��� ������ ������ȯ 
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

            if (Input.GetKeyDown(KeyCode.C)) //��Ŀ ���� 
            {
                ChangeState(State.Anchor);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift)) //����Ʈ����Ʈ�� ��� ���� 
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

        //��Ŀ��Ȳ�� �Ǹ� ���� �ִϸ��̼��� ��������� 

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
                animator.Play("AimStraight");
                renderer.flipX = true; //���� ��ȯ 
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
            // c Ű�� ������ ���� ������ idle�� Ż�� 

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
                Debug.Log("���� ��");
                player.gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, 1.16f);
                player.gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(1.56f, 2.26f);
                rigidbody.gravityScale = 1;
                groundCount = 0;
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
                player.gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, 0.81f);
                player.gameObject.GetComponent<CapsuleCollider2D>().size = new Vector2(1.3f, 1.56f);
                Debug.Log("����");
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
                groundCount = 0;
                player.isJumping = true; //isjumping�� ���߿� parry��Ȳ üũ�� �� �������. 
            }


        }
    }



    private class RunState : PlayerState
    {
        public RunState(PlayerController player) : base(player) { }

        //������ ������ �ӵ� ���� �� �� �ֵ��� 

        public override void Enter()
        {
            animator.Play("Run");
        }

        public override void Update()
        {
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical"); //�밢�� ���� �޸��°� ����������� 
            

            if (axisH < 0.0f && rigidbody.velocity.x > -maxSpeed&&axisV==0.0f) //���� �̵�
            {

                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                //animator.Play("Run");
                renderer.flipX = true;  //�������� ��� �ٲ��ֱ�

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
            else if (axisH > 0.0f && rigidbody.velocity.x < maxSpeed&&axisV==0.0f) //������ �̵� �׻� ������ �ӵ� 
            {

                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                //animator.Play("Run");
                renderer.flipX = false;  //���������� (�������� ����Ʈ)

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

        public override void Transition() //Ʈ�����ǿ��� �޸��鼭 ��� �޸��鼭 ���� ��� ��ȯ���� 
        {
            if (rigidbody.velocity.x == 0.0f) //�ӵ��� 0 �� �� (�������� ���� �� idle�� ü���� ���ֱ� )
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
            

        }

    }




    public IEnumerator ShootCoroutine()
    {
        float coolTime = 0.1f; //�̰� ��� ������ ������ ��� �����ϱ� �װ� �����ؼ� ��Ÿ�� ��������� 
        if (isShooting == false)
        {
            isShooting = true;
            bulletSpawner.ObjectSpawn();
            yield return new WaitForSeconds(coolTime);
            isShooting = false;
        }

    }



}
