using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //������ ���� Ŭ�������� �� ���� �����ϰ� � ��Ȳ�� �Ǹ� �ٸ� ���·� Ʈ������ ���� ���� 
    //�̵��� �׳� �����ý������� ó������ �װ� �ξ� ����� ����. 
    // groundCheck�� �ʿ��� ģ������ �Ǻ��� �������� �ΰ� ���� 
    // �� ����Ʈ�� ��� ���� + �� 2���� 

    public enum State { Idle, Run, Attack, Jump, attackRun, JumpAttack, Down,Anchor }
    //��Ŀ cŰ ������ �̵� ����  8���� ���� ��ȯ ���� 

    [Header("Player")]
    [SerializeField] int hp = 3;
    [SerializeField] int mp = 0;
    [SerializeField] float axisH;
    [SerializeField] float axisV;
    [SerializeField] bool parry; 

    [Header("Component")]
    [SerializeField] new Rigidbody2D rigidbody;
    [SerializeField] new SpriteRenderer renderer;
    [SerializeField] Animator animator;

    [Header("Spec")]
    [SerializeField] float maxSpeed = 5.0f;
    [SerializeField] float accelPower = 10.0f;
    [SerializeField] float decelPower = 20.0f;
    [SerializeField] float jumpSpeed = 10.0f ;
    [SerializeField] LayerMask groundCheckLayer; //���������� ���� ���� or �и� �������� �������� 
    [SerializeField] Vector2 playerPos;

    private Vector2 inputDir;
    private bool onGround;
    private int groundCount;


    private StateMachine stateMachine;

    string nowAnime = "";
    string oldAnime = "";

    //�̵� �ɱ� ��� ���� ���� ���� -->���¸ӽ� ���� 

    void Awake() //���� �ÿ� ���¸� �߰� �ϰ� ���� (��ųʸ��� add�� )
    {
        stateMachine = gameObject.AddComponent<StateMachine>();
        stateMachine.AddState(State.Idle, new IdleState(this));
        stateMachine.AddState(State.Run, new RunState(this));
        stateMachine.AddState(State.Attack, new AttackState(this));
        stateMachine.AddState(State.Jump, new JumpState(this));
        stateMachine.AddState(State.Down, new DownState(this));
        stateMachine.AddState(State.Anchor, new AnchorState(this));

        stateMachine.InitState(State.Idle); //���� ���¸� Idle ���·� ���� 

    }
    private void Start()
    {
        nowAnime = "IdlePlayer";
        oldAnime = "IdlePlayer";
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;

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
            groundCount = 1;
        }
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

        public PlayerState(PlayerController player)
        {
            this.player = player;
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


            //�ݶ��̴� ĸ�� ���� �ڽ� �Ѽ� �浹 �ٲ��ֱ� 
            animator.Play("DownPlayer");
        }

        public override void Update()
        {
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

            animator.Play("DownIdlePlayer");
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

        public override void Update() //��� ���ư��鼭 üũ ������Ʈ + Ʈ������ 
        {
            animator.Play("IdlePlayer");
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical"); //������ �ƴ϶� �� �Ʒ� ���� ��������?


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

            if(Input.GetKeyDown(KeyCode.C)) //��Ŀ ���� 
            {
                ChangeState(State.Anchor);
            }

        }
    }

    private class AnchorState:PlayerState
    {
        public AnchorState(PlayerController player) : base(player) { }

        //��Ŀ��Ȳ�� �Ǹ� ���� �ִϸ��̼��� ��������� 

        public override void Enter()
        {
            rigidbody.velocity = Vector2.zero;

        }


        public override void Update()
        {
            Debug.Log("��Ŀ");
        }

        public override void Transition()
        {
            // c Ű�� ������ ���� ������ idle�� Ż�� 

            if(Input.GetKeyUp(KeyCode.C))
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
            if(Input.GetKey(KeyCode.Z))
            {
                isLongJump= true;
            }
            else if(Input.GetKeyUp(KeyCode.Z))
            {
                isLongJump= false;
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
            if(isLongJump)
            {
                rigidbody.gravityScale = 0.9f;
            }
            else
            {
                rigidbody.gravityScale = 2f;
            }
        }

        public override void Transition()
        {
            if (onGround && groundCount == 1)
            {
                player.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                //�̰� �̺κ� ���ľ� ���� ����� �� ���� ĸ���� ũ�⸦ �ٲٴ°�?? 
                player.gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
                rigidbody.gravityScale = 1;
                ChangeState(State.Idle);
            }
            

        }

        public void Jump()
        {

            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
            animator.Play("JumpPlayer");
            player.gameObject.GetComponent<CircleCollider2D>().enabled = true;
            player.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
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

        //������ ������ �ӵ� ���� �� �� �ֵ��� 
        public override void Update()
        {
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical"); //������ �ƴ϶� �� �Ʒ� ���� ��������?

            if (axisH < 0.0f && rigidbody.velocity.x > -maxSpeed) //���� �̵�
            {

                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                animator.Play("RunPlayer");
                renderer.flipX = true;  //�������� ��� �ٲ��ֱ�

            }
            else if (axisH > 0.0f && rigidbody.velocity.x < maxSpeed) //������ �̵� �׻� ������ �ӵ� 
            {


                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                animator.Play("RunPlayer");
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
            if (axisV < 0.0f)
            {
                ChangeState(State.Down);
            }

            if (Input.GetKeyDown(KeyCode.C)) //��Ŀ ���� 
            {
                ChangeState(State.Anchor);
            }

        }

    }




}
