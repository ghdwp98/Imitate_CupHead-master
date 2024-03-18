using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : LivingEntity
{


    public enum State
    {
        Idle, Run, Attack, Jump, AttackRun, JumpAttack, Down, Anchor, Dash, JumpDash
        , Fall, Parrying, Up, Ex, Dead, Hit, GameOver
    }


    [Header("Player")]
    [SerializeField] public int hp = 3;
    [SerializeField] float axisH;
    [SerializeField] float axisV;
    [SerializeField] bool parry;
    [SerializeField] bool isShooting = false;
    [SerializeField] float parryRange = 1f;
    [SerializeField] bool parrySucess = false;
    [SerializeField] bool EXshooting = false;
    [SerializeField] GameObject GhostPrefab;

    [Header("Component")]
    [SerializeField] new Rigidbody2D rigidbody;
    [SerializeField] new SpriteRenderer renderer;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource playerAudio;


    [SerializeField] BulletSpawner bulletSpawner;
    [SerializeField] GameObject FootBoxCollider;
    [SerializeField] JumpEffectSpawn JumpEffectSpawn;
    [SerializeField] ParryCheck parryCheck;
    [SerializeField] DashEffectSpawn DashEffectSpawn;
    [SerializeField] Transform dashSpawn;
    [SerializeField] LayerMask parryMask;
    [SerializeField] GameObject superBar;
    [SerializeField] BarController barController;
    [SerializeField] HPui hpui;
    

    PooledObject bulletPrefab;
    PooledObject bulletSparkle;
    Transform spawnPos;

    [Header("Spec")]
    [SerializeField] float maxSpeed = 13.0f;
    [SerializeField] float accelPower = 13.0f;
    [SerializeField] float decelPower = 20.0f;
    [SerializeField] float jumpSpeed = 11.0f;
    [SerializeField] LayerMask groundCheckLayer;
    [SerializeField] Vector2 playerPos;
    [SerializeField] bool FootIsTrigger = false;
    [SerializeField] Vector2 bulletPos;
    [SerializeField] bool isOverlap = false;
    [SerializeField] bool takeHit = false;
    [SerializeField] bool invincible = false;

    private Vector2 inputDir;
    private bool onGround;
    private int groundCount;
    private bool isJumping;
    private bool isParried;
    public bool downJump = false;

    private StateMachine stateMachine;

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
        stateMachine.AddState(State.Ex, new ExState(this));
        stateMachine.AddState(State.Dead, new DeadState(this));
        stateMachine.AddState(State.Hit, new HitState(this));
        stateMachine.AddState(State.GameOver, new GameOverState(this));


        stateMachine.InitState(State.Idle);

    }


    //공격 불가능 + 이동 불가능한 intro 상태를 만들어줘서 씬로딩 중에서 bool변수나 함수 같은거를 가져와서
    // 실행시켜주고 update 자체에서 그거를 받아들이면 intro 씬에서 애니메이션을 재생해주고
    // 애니메이션이 끝나면 idel 상태로 전환 init을 인트로 상태로 둬 줘 야함. 
    private void Start()
    {      
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio= GetComponent<AudioSource>();
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

        if (collision.gameObject.layer == 13) //몬스터에 피격되면. 13번 레이어 몬스터 
        {
            TakeHit();

        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Checking"
            || collision.tag == "Parry")

        {
            return;
        }

        FootIsTrigger = true;

        if (collision.gameObject.layer==13) //몬스터에 피격되면. 
        {
            TakeHit();
        }
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

    private class PlayerState : BaseState
    {
        protected PlayerController player;

        protected Transform transform => player.transform;
        protected int hp { get { return player.hp; } set { player.hp = value; } }
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
        protected HPui hpUI { get { return player.hpui; } set { player.hpui = value; } }
        public PlayerState(PlayerController player)
        {
            this.player = player;
        }



    }

    

    private class ExState : PlayerState
    {
        public ExState(PlayerController player) : base(player) { }
        public bool ExitEx;
        public bool animPlaying;
        public override void Enter()
        {
            Debug.Log("ex상태진입");
            ExitEx = false;
            animPlaying = false;

        }

        public override void Update()
        {
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical");

            rigidbody.velocity = Vector2.zero;

            if (onGround == false) //공중 상태 일 때 . 
            {
                if (renderer.flipX == true)  //왼쪽 보는중
                {
                    renderer.flipX = true; //애니메이션 추가 하지 말고 그냥 왼쪽으로 돌려주자. 

                    if (axisV == 0 || (axisH == -1f && axisV == 0)) //왼쪽 공중 스트레이트. 
                    {
                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 180);
                            spawnPos.transform.localPosition = new Vector2(-1.5f, 1.2f);
                            animPlaying = true;
                            animator.Play("ExStraightAir");
                            player.ExShoot();
                        }
                    }
                    else if ((axisH == 0.0f && axisV == +1.0f))  // 왼쪽 공중 up
                    {
                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 90);
                            spawnPos.transform.localPosition = new Vector2(-0.4f, 2.7f);
                            animPlaying = true;
                            animator.Play("ExUpAir");
                            player.ExShoot();
                        }


                    }
                    else if (axisH == 0.0f && axisV == -1.0f) //down 
                    {
                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, -90);
                            spawnPos.transform.localPosition = new Vector2(0.16f, 0.25f);
                            animPlaying = true;
                            animator.Play("ExDownAir");
                            player.ExShoot();
                        }


                    }
                    else if (axisH == -1.0f && axisV == 1.0f)  //공중 왼쪽 diag up  
                    {
                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 135);
                            spawnPos.transform.localPosition = new Vector2(-1.5f, 2.1f);
                            animPlaying = true;
                            animator.Play("ExDiagUpAir");
                            player.ExShoot();
                        }


                    }
                    else if (axisH == -1.0f && axisV == -1.0f) //공중 오른쪽 diag down
                    {
                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 225);
                            spawnPos.transform.localPosition = new Vector2(-0.64f, 0.8f);
                            animPlaying = true;
                            animator.Play("ExDiagDownAir");
                            player.ExShoot();
                        }
                    }
                }
                else //오른쪽 보는 중 
                {
                    renderer.flipX = false;
                    if (axisV == 0)
                    {

                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            spawnPos.transform.localPosition = new Vector2(0.9f, 1.2f);
                            animPlaying = true;
                            animator.Play("ExStraightAir");
                            player.ExShoot();
                        }

                    }
                    else if ((axisH == 0.0f && axisV == +1.0f))  //up
                    {


                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 90);
                            spawnPos.transform.localPosition = new Vector2(0.4f, 2.7f);
                            animPlaying = true;
                            animator.Play("ExUpAir");
                            player.ExShoot();
                        }

                    }
                    else if (axisH == 0.0f && axisV == -1.0f) //down 
                    {
                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, -90);
                            spawnPos.transform.localPosition = new Vector2(0.16f, 0.25f);
                            animPlaying = true;
                            animator.Play("ExDownAir");
                            player.ExShoot();
                        }

                    }
                    else if (axisH == 1.0f && axisV == 1.0f)
                    {

                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 45);
                            spawnPos.transform.localPosition = new Vector2(1.5f, 2.1f);
                            animPlaying = true;
                            animator.Play("ExDiagUpAir");
                            player.ExShoot();
                        }
                    }
                    else if (axisH == 1.0f && axisV == -1.0f)
                    {

                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, -45);
                            spawnPos.transform.localPosition = new Vector2(0.57f, 0.7f);
                            animPlaying = true;
                            animator.Play("ExDiagDownAir");
                            player.ExShoot();
                        }
                    }
                }
            }
            else //땅 일 때 애니메이션 재생 
            {
                if (renderer.flipX == true)  //왼쪽 보는중
                {
                    if (axisV == 0 || (axisV == 0 && axisH == 1))
                    {
                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 180);
                            spawnPos.transform.localPosition = new Vector2(-1.5f, 1.2f);
                            animPlaying = true;
                            animator.Play("ExStraight");
                            player.ExShoot();
                        }

                    }
                    else if ((axisH == 0.0f && axisV == +1.0f))  //up
                    {


                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 90);
                            spawnPos.transform.localPosition = new Vector2(-0.4f, 2.7f);
                            animPlaying = true;
                            animator.Play("ExUp");
                            player.ExShoot();
                        }

                    }
                    else if (axisH == 0.0f && axisV == -1.0f) //down 
                    {

                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, -90);
                            spawnPos.transform.localPosition = new Vector2(0.16f, 0.25f);
                            animPlaying = true;
                            animator.Play("ExDown");
                            player.ExShoot();
                        }

                    }
                    else if (axisH == -1.0f && axisV == 1.0f) //diagup
                    {

                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 135);
                            spawnPos.transform.localPosition = new Vector2(-1.5f, 2.1f);
                            animPlaying = true;
                            animator.Play("ExDiagUp");
                            player.ExShoot();
                        }

                    }
                    else if (axisH == -1.0f && axisV == -1.0f) //diagdown
                    {

                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 225);
                            spawnPos.transform.localPosition = new Vector2(-0.64f, 0.8f);
                            animPlaying = true;
                            animator.Play("ExDiagDown");
                            player.ExShoot();
                        }
                    }

                }
                else
                {
                    if (axisV == 0)
                    {
                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 0);
                            spawnPos.transform.localPosition = new Vector2(0.9f, 1.2f);
                            animPlaying = true;
                            animator.Play("ExStraight");
                            player.ExShoot();
                        }


                    }
                    else if ((axisH == 0.0f && axisV == +1.0f))  //up
                    {


                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 90);
                            spawnPos.transform.localPosition = new Vector2(0.4f, 2.7f);
                            animPlaying = true;
                            animator.Play("ExUp");
                            player.ExShoot();
                        }
                    }
                    else if (axisH == 0.0f && axisV == -1.0f) //down 
                    {


                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, -90);
                            spawnPos.transform.localPosition = new Vector2(0.16f, 0.25f);
                            animPlaying = true;
                            animator.Play("ExDown");
                            player.ExShoot();
                        }
                    }
                    else if (axisH == 1.0f && axisV == 1.0f)
                    {

                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 45);
                            spawnPos.transform.localPosition = new Vector2(1.5f, 2.1f);
                            animPlaying = true;
                            animator.Play("ExDiagUp");
                            player.ExShoot();
                        }

                    }
                    else if (axisH == 1.0f && axisV == -1.0f)
                    {
                        if (animPlaying == false)
                        {
                            spawnPos.transform.localRotation = Quaternion.Euler(0, 0, -45);
                            spawnPos.transform.localPosition = new Vector2(0.57f, 0.7f);
                            animPlaying = true;
                            animator.Play("ExDiagDown");
                            player.ExShoot();
                        }

                    }
                }

            }
        }
        public override void LateUpdate()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("ExStraightAir") &&
                        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                ExitEx = true;
                animPlaying = false;
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("ExUpAir") &&
                   animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                ExitEx = true;
                animPlaying = false;

            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("ExDownAir") &&
                        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                ExitEx = true;
                animPlaying = false;

            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("ExDiagUpAir") &&
                        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                ExitEx = true;
                animPlaying = false;

            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("ExDiagDownAir") &&
                        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                ExitEx = true;
                animPlaying = false;

            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("ExStraight") &&
                        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                ExitEx = true;
                animPlaying = false;

            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("ExDown") &&
                        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                ExitEx = true;
                animPlaying = false;

            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("ExUp") &&
                        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                ExitEx = true;
                animPlaying = false;

            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("ExDiagUp") &&
                        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                ExitEx = true;
                animPlaying = false;

            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("ExDiagDown") &&
                        animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                ExitEx = true;
                animPlaying = false;

            }

        }

        public override void Transition()
        {
            if (ExitEx == true)  // 필살기 시전이 끝나면 true로 전환해서 상태 변경 해주기. 
            {
                if (onGround == true) //공중에서 쓰고 나면 떨어지게 
                {
                    ChangeState(State.Fall);
                }
                else
                {
                    ChangeState(State.Idle);
                }
            }
            if (player.takeHit == true && player.invincible == false)
            {
                ChangeState(State.Hit);
            }
        }
    }


    private class GameOverState : PlayerState
    {
        public GameOverState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            Instantiate(player.GhostPrefab, transform.position, Quaternion.identity);
            Debug.Log("게임오버"); //팝업창 열어주는 이벤트를 여기서 실행해주자. 
            player.gameObject.SetActive(false);
        }
    }

    private class DeadState : PlayerState //피격 상태 만들고 그냥 피격 상태에서만 dead로 진입할 수 있도록하자. 
    {
        public DeadState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            Debug.Log("사망상태 진입");         
            animator.Play("Death");


            // 그리고 팝업도 띄워줘야함. --> 다시 하기 팝업 + 머그샷
        }

        public override void Update()
        {
            rigidbody.velocity = Vector2.zero;
        }
        public override void Exit()
        {           
        }

        public override void Transition()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                ChangeState(State.GameOver);
            }

        }


    }

    private class HitState : PlayerState
    {
        
        //이거 아예 못들어오게 해야되는데 어떡하지?? 
        // 
        public HitState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            player.invincible = true;
            
            if (onGround == true) //땅 일때 
            {
                animator.Play("Hit");
                rigidbody.velocity = Vector2.left*2;

            }
            else // 땅이 아닐 때 false 일때 
            {
                animator.Play("HitAir");
                rigidbody.velocity = Vector2.left*2;
                rigidbody.velocity = Vector2.down * 7;

            }
        }

        public override void Exit()
        {
            rigidbody.velocity = Vector2.zero;
        }
        public override void Transition()
        {

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                if (hp > 0)
                {
                    ChangeState(State.Idle);
                }
                else if (hp <= 0)
                {
                    Debug.Log("데드 상태 진입");
                    ChangeState(State.Dead);
                }
            }
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("HitAir") &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                if (hp > 0)
                {
                    ChangeState(State.Fall);
                }
                else if (hp <= 0)
                {
                    Debug.Log("데드 상태 진입");
                    ChangeState(State.Dead);
                }
            }

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
            if (player.takeHit == true && player.invincible == false)
            {
                ChangeState(State.Hit);
            }

            else if (Input.GetKeyUp(KeyCode.UpArrow))
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
            else if (Input.GetKeyDown(KeyCode.V) && barController.manaBarImage[0].fillAmount == 1f
                && player.EXshooting == false)
            {
                ChangeState(State.Ex);
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

            if (player.takeHit == true && player.invincible == false)
            {
                ChangeState(State.Hit);
            }

            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ChangeState(State.Dash);
            }

            if (parrySucess == true)
            {
                ChangeState(State.Jump);
            }
            else if (Input.GetKeyDown(KeyCode.V) && barController.manaBarImage[0].fillAmount == 1f
                && player.EXshooting == false)
            {
                ChangeState(State.Ex);
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

        public override void Exit()
        {

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
            if (player.takeHit == true && player.invincible == false)
            {
                rigidbody.velocity = Vector2.zero;
                ChangeState(State.Hit);
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
        public override void Exit()
        {



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

            if (player.takeHit == true && player.invincible == false)
            {
                rigidbody.velocity = Vector2.zero;
                ChangeState(State.Hit);
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

            if (axisH < 0.0f && rigidbody.velocity.x >= -maxSpeed) //���� �̵�
            {
                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                renderer.flipX = true;  //�������� ��� �ٲ��ֱ�

            }
            else if (axisH > 0.0f && rigidbody.velocity.x <= maxSpeed) //������ �̵� �׻� ������ �ӵ� 
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
            if (onGround && rigidbody.velocity.y > -0.3)
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
            else if (Input.GetKeyDown(KeyCode.V) && barController.manaBarImage[0].fillAmount == 1f
                && player.EXshooting == false)
            {
                ChangeState(State.Ex);
            }

            if (player.takeHit == true && player.invincible == false)
            {
                ChangeState(State.Hit);
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


            if (player.takeHit == true && player.invincible == false)
            {
                ChangeState(State.Hit);
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
            else if (Input.GetKeyDown(KeyCode.V) && barController.manaBarImage[0].fillAmount == 1f
                && player.EXshooting == false)
            {
                ChangeState(State.Ex);
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

            if (player.takeHit == true && player.invincible == false)
            {
                ChangeState(State.Hit);
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


            if (Input.GetKeyUp(KeyCode.C))
            {
                ChangeState(State.Idle);
            }

            if (axisV == 1.0 && Input.GetKeyUp(KeyCode.C))
            {

                ChangeState(State.Up);
            }

            if (Input.GetKeyDown(KeyCode.V) && barController.manaBarImage[0].fillAmount == 1f
                && player.EXshooting == false)
            {
                ChangeState(State.Ex);
            }


            if (player.takeHit == true && player.invincible == false)
            {
                ChangeState(State.Hit);
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

            if (axisH < 0.0f && rigidbody.velocity.x >= -maxSpeed) //���� �̵�
            {
                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                renderer.flipX = true;  //�������� ��� �ٲ��ֱ�
            }
            else if (axisH > 0.0f && rigidbody.velocity.x <= maxSpeed) //������ �̵� �׻� ������ �ӵ� 
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

            if (Input.GetKeyDown(KeyCode.V) && barController.manaBarImage[0].fillAmount == 1f
                && player.EXshooting == false)
            {
                ChangeState(State.Ex);
            }

            if (player.takeHit == true && player.invincible == false)
            {
                ChangeState(State.Hit);
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
            animator.Play("Run"); //런은 반복재생 맞고 
        }

        public override void Update()
        {
            axisH = Input.GetAxisRaw("Horizontal");
            axisV = Input.GetAxisRaw("Vertical"); //�밢�� ���� �޸��°� ����������� 


            if (axisH < 0.0f && rigidbody.velocity.x >= -maxSpeed) //���� �̵�
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
            else if (axisH > 0.0f && rigidbody.velocity.x <= maxSpeed) //������ �̵� �׻� ������ �ӵ� 
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
                    Debug.Log("런샷 false진입");
                    animator.SetBool("RunShoot", false);
                }
            }

            //튜토에서 왜 안나가지 런샷이???

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
            
        }

        public override void Transition() //Ʈ�����ǿ��� �޸��鼭 ��� �޸��鼭 ���� ��� ��ȯ���� 
        {

            if (player.takeHit == true && player.invincible == false)
            {
                ChangeState(State.Hit);
            }

            if (axisH == 0 && axisV == 0) //�ӵ��� 0 �� �� (�������� ���� �� idle�� ü���� ���ֱ� )
            {
                ChangeState(State.Idle);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {

                ChangeState(State.Jump);
            }
            else if (Input.GetKeyDown(KeyCode.C)) //��Ŀ ���� 
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

            if (axisV == -1)
            {
                animator.Play("Down"); //다운 exit에 있길래 여기로 옮김. 
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

            if (Input.GetKeyDown(KeyCode.V) && barController.manaBarImage[0].fillAmount == 1f
                && player.EXshooting == false)
            {
                ChangeState(State.Ex);
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

            if (axisH < 0.0f && rigidbody.velocity.x >= -maxSpeed) //���� �̵�
            {
                rigidbody.velocity = new Vector2(axisH * accelPower, rigidbody.velocity.y);
                renderer.flipX = true;  //�������� ��� �ٲ��ֱ�
                spawnPos.transform.localRotation = Quaternion.Euler(0, 0, 135);
                spawnPos.transform.localPosition = new Vector2(-2.2f, 2.4f);

            }
            else if (axisH > 0.0f && rigidbody.velocity.x <= maxSpeed) //������ �̵� �׻� ������ �ӵ� 
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

            if (Input.GetKeyDown(KeyCode.V) && barController.manaBarImage[0].fillAmount == 1f
                && player.EXshooting == false)
            {
                ChangeState(State.Ex);
            }

            if (player.takeHit == true && player.invincible == false)
            {
                ChangeState(State.Hit);
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
        float coolTime = 0.8f; //바꿔나가자. 
        if (EXshooting == false)
        {
            EXshooting = true;
            yield return new WaitForSecondsRealtime(0.1f);
            bulletSpawner.EXShootSpawn();
            yield return new WaitForSeconds(coolTime);
            barController.EXshoot();
            EXshooting = false;
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


    public void TakeHit()
    {
        StartCoroutine(TakeHitCoroutine());
    }

    private IEnumerator TakeHitCoroutine() //여기서 일정시간 데미지 안 받도록 해주고
                                           // + 로 유닛 흐리게 해주기. 한 2초? 
    {
        if (takeHit == false && hp > 0)
        {
            this.gameObject.layer = 15; //인빈시블 상태로 전환 
            takeHit = true;
            hpui.HpChange();
            hp -= 1;
            yield return StartCoroutine(Twinkle());
            renderer.color = new Color(1, 1, 1, 1);
            takeHit = false;
            invincible = false;
            this.gameObject.layer = 3;
        }
    }

    private IEnumerator Twinkle()
    {
        for (int i = 0; i < 7; i++)
        {

            renderer.color = new Color(1, 1, 1, 0.4f);
            yield return new WaitForSeconds(0.2f);
            renderer.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.2f);
        }
    }



}


