using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeBoss : LivingEntity
{



    public enum State //�̰� �ֱ� �������̴ϱ� phase�� �ƴ϶� �� ���� ��ǵ�� ��������. 
    {
        Intro, Punch, Jump, Dead, BigIdle, BigJump, BigPunch, BigDead, TombIdle, TombMove, TombDead
            , TombAttack
    }

    //health�� ü���ε�? 
    [Header("Slime")]
    [SerializeField] float hp = 500;
    [SerializeField] LayerMask TargetLayer;
    [SerializeField] GameObject target;
    [SerializeField] Vector2 dir;
    [SerializeField] float v_y;
    [SerializeField] float v_x;
    [SerializeField] Vector2 lastVelocity;
    [SerializeField] int jumpCount = 0;
    [SerializeField] int bigJumpCount = 0;
    [SerializeField] int tombMoveCount = 0;
    [SerializeField] GameObject questionPrefab1;
    [SerializeField] GameObject questionPrefab2;
    [SerializeField] GameObject questionPrefab3;
    [SerializeField] GameObject tombPrefab;
    [SerializeField] SlimeDustSpawner spawner;
    public bool TombCollid;

    [SerializeField] CircleCollider2D SmallcircleCollider;
    [SerializeField] CircleCollider2D BigcircleCollider;
    [SerializeField] BoxCollider2D BoxCollider;

    [SerializeField] CircleCollider2D TombCircleCollider;
    [SerializeField] BoxCollider2D TombBoxCollider;

    [SerializeField] Sprite BigSlime;
    [SerializeField] Sprite Tomb;

    [SerializeField] Transform Mark1;
    [SerializeField] Transform Mark2;
    [SerializeField] Transform Mark3;
    [SerializeField] GameObject PunchArm;
    [SerializeField] GameObject punchFist;
    GameObject tombInstance;
    Rigidbody2D slimeRb;

    SpriteRenderer spriteRenderer;
    bool jumpRoutineEnd = false;
    bool bigJumpRoutineEnd = false;
    bool tombMoveRoutineEnd = false;
    bool isOverLap = false;
    float overLapCollTime;
    float targetRange = 3f;
    bool isAttack = false;

    [SerializeField] LayerMask TombTarget;

    [SerializeField] bool onGround;
    [SerializeField] LayerMask groundCheckLayer;

    AudioSource SlimeAudio;
    Animator animator;
    [SerializeField] byte red = 233;
    [SerializeField] byte green = 233;
    [SerializeField] byte blue = 217;
    [SerializeField] byte alpha = 255;

    private StateMachine stateMachine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        SlimeAudio = GetComponent<AudioSource>();
        health = hp;
        slimeRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();



        stateMachine = gameObject.AddComponent<StateMachine>();
        stateMachine.AddState(State.Intro, new IntroState(this));
        stateMachine.AddState(State.Jump, new JumpState(this));
        stateMachine.AddState(State.Punch, new PunchState(this));
        stateMachine.AddState(State.Dead, new DeadState(this));
        stateMachine.AddState(State.BigIdle, new BigIdleState(this));
        stateMachine.AddState(State.BigJump, new BigJumpState(this));
        stateMachine.AddState(State.BigPunch, new BigPunchState(this));
        stateMachine.AddState(State.BigDead, new BigDeadState(this));
        stateMachine.AddState(State.TombIdle, new TombIdleState(this));
        stateMachine.AddState(State.TombMove, new TombMoveState(this));
        stateMachine.AddState(State.TombDead, new TombDeadState(this));
        stateMachine.AddState(State.TombAttack, new TombAttackState(this));



        stateMachine.InitState(State.BigDead); // �ڵ������� ��� ���� ���� ���� 
    }
    void Start()
    {
        SmallcircleCollider.enabled = true;
        BigcircleCollider.enabled = false;
        PunchArm.SetActive(false);
        punchFist.SetActive(false);

    }

    void Update()
    {
        stateMachine.Update();

        StartCoroutine(TargetCoroutine());
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            var speed = lastVelocity.magnitude;
            var dir = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            slimeRb.velocity = dir * 9;

            if (spriteRenderer.flipX == true)  //�ø� ���¿� ���� ���� �Լ��� ������ �ٽ� �������༭ - + �� �޸����� ����. 
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;

            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision) //Ÿ���� Ʈ���Ÿ� ������. 
    {
        if (!dead)
        {
            //�ǰ�ȿ����
            if (collision.gameObject.tag == "Bullet")
            {
                StartCoroutine(OnHit()); //�ǰ� �� ���� ȿ��
            }
        }


    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundCheckLayer);

        lastVelocity = slimeRb.velocity;




    }

    private class SlimeState : BaseState
    {
        protected SlimeBoss slime;

        protected Transform transform => slime.transform;

        protected float hp { get { return slime.hp; } set { slime.hp = value; } }
        protected Rigidbody2D slimeRb => slime.slimeRb;
        protected SpriteRenderer renderer => slime.spriteRenderer;
        protected Animator animator => slime.animator;
        protected float health { get { return slime.health; } set { slime.health = value; } }

        protected bool onGround { get { return slime.onGround; } set { slime.onGround = value; } }

        protected LayerMask groundCheckLayer => slime.groundCheckLayer;

        protected GameObject target => slime.target;
        protected Vector2 dir { get { return slime.dir; } set { slime.dir = value; } }

        public SlimeState(SlimeBoss slime)
        {
            this.slime = slime;
        }
    }


    private class BigIdleState : SlimeState
    {
        public BigIdleState(SlimeBoss slime) : base(slime) { }

        public float idle = 0f;
        public override void Enter()
        {

            renderer.sprite = slime.BigSlime;
            slime.SmallcircleCollider.enabled = false;
            slime.BigcircleCollider.enabled = true;
            slime.BigcircleCollider.offset = new Vector2(0.068f, 2.9f);
            slime.BigcircleCollider.radius = 2.86f;

            animator.Play("BigSlimeIdle");
        }

        public override void Update()
        {

            idle += Time.deltaTime;
        }

        public override void Transition()
        {
            if (idle > 3)
            {
                Destroy(slime.Mark1.gameObject);
                Destroy(slime.Mark2.gameObject);
                Destroy(slime.Mark3.gameObject);
                ChangeState(State.BigJump);
            }


        }
    }


    private class BigJumpState : SlimeState
    {
        public BigJumpState(SlimeBoss slime) : base(slime) { }

        public int rand;
        public override void Enter()
        {
            rand = Random.Range(3, 6);
        }
        public override void FixedUpdate()
        {

            if (onGround == true)
            {
                slime.StartCoroutine(slime.BigJumpRoutine());
                if (slimeRb.velocity.y < -5)
                {
                    slime.spawner.BigSlimeDustSpawn();
                }
            }
            else //�±׶��尡 �ƴ� �� (���� ) 
            {
                if (slimeRb.velocity.y > 1) //��� �ִ� 
                {
                    animator.Play("BigSlimeAirUp");
                }
                else if (slimeRb.velocity.y < 1 && slimeRb.velocity.y > -1) //���� �ִ� 
                {
                    animator.Play("BigSlimeDownTrans");
                }
                else if (slimeRb.velocity.y < -1) //�ϰ� �ִ� 
                {
                    animator.Play("BigSlimeAirDown");
                }
            }

        }
        public override void Exit()
        {
            slime.bigJumpCount = 0;
        }

        public override void Transition()
        {
            if (slime.bigJumpCount == rand && onGround == true)
            {
                ChangeState(State.BigPunch);
            }

            if (health <= 150 && (transform.position.x <= 6 && transform.position.x >= -6)
                && (onGround == true))
            {
                ChangeState(State.BigDead);
            }
        }
    }

    private class BigPunchState : SlimeState //��ġ �ִϸ��̼� ��ġ ���������... 
    {
        public BigPunchState(SlimeBoss slime) : base(slime) { }

        public override void Enter()
        {
            if (dir.x < 0)
            {
                renderer.flipX = false;
            }
            else if (dir.x >= 0)
            {
                renderer.flipX = true; //������ ���� 

            }

            animator.Play("BigSlimePunch");
        }

        public override void Update()
        {
            slimeRb.velocity = Vector2.zero;

        }

        public override void Exit()
        {
            slime.PunchArm.SetActive(false);
            slime.punchFist.SetActive(false);
        }
        public override void Transition()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("BigSlimePunch") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                ChangeState(State.BigJump);
            }
            if (health <= 150 && (transform.position.x <= 6 && transform.position.x >= -6) &&
                (onGround == true))
            {
                ChangeState(State.BigDead);
            }

        }

    }
    private class BigDeadState : SlimeState
    {

        public int count = 0;
        public BigDeadState(SlimeBoss slime) : base(slime) { }

        public float deathAnimPlaying = 0;

        public override void Enter()
        {
            animator.Play("BigSlimeDeath");
            Vector2 pos = new Vector2(transform.position.x, transform.position.y + 100);
            slime.tombInstance = Instantiate(slime.tombPrefab, pos, Quaternion.identity);

        }

        public override void Update()
        {


            slimeRb.velocity = Vector2.zero;

            if (slime.TombCollid == true) //�浹 ������. 
            {
                //�ͽ��÷ε� �ִϸ��̼� ����ϰ� ��� �ݶ��̴� ���� ���� ������. 
                // �� ���·� �̵��ϰ� �ݶ��̴� �� ���� 

                slime.SmallcircleCollider.enabled = false;
                slime.BigcircleCollider.enabled = false;
                animator.Play("BigSlimeExplode");

                if (count == 0)
                {
                    slime.spawner.TombFallSpawn();
                    count = 1;
                }


            }

        }

        public override void Exit()
        {
            slime.TombCollid = false;
        }

        public override void Transition()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("BigSlimeExplode") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                ChangeState(State.TombIdle);
            }
        }

    }

    private class TombIdleState : SlimeState
    {
        public TombIdleState(SlimeBoss slime) : base(slime) { }

        public float idleTime = 0f;
        public override void Enter()
        {
            animator.Play("TombTrans");
            Destroy(slime.tombInstance);
            slimeRb.gravityScale = 0f;
            slime.BoxCollider.enabled = false;

            slime.TombCircleCollider.enabled = true;
            //slime.TombBoxCollider.enabled = true; -->��� ���ݽÿ��� . 

            slime.gameObject.layer = 0; //�ϴ� ����Ʈ�� �غ���? 

        }

        public override void FixedUpdate()
        {

            idleTime += Time.deltaTime;
        }


        public override void Transition()
        {
            if (idleTime >= 3f)
            {
                ChangeState(State.TombMove);
            }
        }

    }

    private class TombMoveState : SlimeState
    {
        public TombMoveState(SlimeBoss slime) : base(slime) { }

        public float rand; //time�ϰ� ���ؾ� �� �� ������ �������� �ؼ�.. ?

        public float wallTime;
        public float dealyTime;

        public override void Enter()
        {


            slimeRb.velocity = Vector2.zero;
            rand = Random.Range(10, 15); //�� ���̷� ������ ������ time�� �� ���� �Ѿ������
            // �� �� �������� ��ȯ // + �÷��̾� ��ó�� ��. 

            //slime.TombCollid �̰� ��Ȱ������. wall�̶� �ε����� wall�� slime���� �̰� �Ѱ��ְ�
            // �׷��� ���� ���� �ٲٴ� ����. 
        }

        public override void Update()
        {

            dealyTime += Time.deltaTime;

            if (renderer.flipX == true) //������ ���� �ִ� ��� �������̵� 
            {
                if (transform.position.x < 7.5)  // �����̹Ƿ� �̵� �ؾ���. 
                {
                    animator.Play("TombLeftMove");
                    slimeRb.velocity = new Vector2(15, slimeRb.velocity.y);
                }
                else if (transform.position.x >= 7.5) //���� ���� ������. 
                {
                    animator.Play("TombLeftMove");
                    slimeRb.velocity = Vector2.zero;
                    wallTime += Time.deltaTime;
                    if (wallTime >= 2f)
                    {
                        renderer.flipX = false;
                        wallTime = 0f;
                    }

                }
            }
            else //���� ���� �ִ� ��� ���� �̵� 
            {
                if (transform.position.x <= -7.8)
                {
                    animator.Play("TombLeftMove");
                    slimeRb.velocity = Vector2.zero;
                    wallTime += Time.deltaTime;
                    if (wallTime >= 2f)
                    {
                        renderer.flipX = true;
                        wallTime = 0f;
                    }
                }
                else if (transform.position.x > -7.8)
                {
                    animator.Play("TombLeftMove");

                    slimeRb.velocity = new Vector2(-15, slimeRb.velocity.y);
                }
            }
        }

        public override void FixedUpdate()
        {
            slime.StartCoroutine(slime.TargetTombCoroutine());

        }

        public override void Exit()
        {
            dealyTime = 0f;

            if (renderer.flipX == true)
            {
                renderer.flipX = false;
            }
            else
            {
                renderer.flipX = true;
            }
        }

        public override void Transition()
        {

            if (rand <= dealyTime && slime.isAttack == true)
            {
                ChangeState(State.TombAttack);
            }

            if (hp <= 0)
            {
                ChangeState(State.TombDead);
            }
        }

    }

    private class TombAttackState : SlimeState
    {
        public TombAttackState(SlimeBoss slime) : base(slime) { }

       

        public override void Enter()
        {
            
            slimeRb.velocity = Vector2.zero;
            animator.Play("TombAttack");
        }

        public override void Update()
        {
           

            
        }

        public override void Exit()
        {
            
        }

        public override void Transition()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("TombAttack") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                ChangeState(State.TombMove);
            }
            if (hp <= 0)
            {
                ChangeState(State.TombDead);
            }
        }
    }
    private class TombDeadState : SlimeState
    {
        public TombDeadState(SlimeBoss slime) : base(slime) { }

        public override void Enter()
        {
            //��¦ �ð� ���߰� --> �˾ƿ� ������
            // �״� �ִϸ��̼� �ݺ� + �ı��Ǵ� ���� �ִϸ��̼� ��� (�� �� �ݺ�)
            // �ݺ��Ǹ鼭 ���� ���̵� �ƿ� �� --> ���̵� �ƿ� �Ǹ鼭 �� ��ȯ �ϸ� �ɵ�


            slimeRb.velocity = Vector2.zero;
            animator.Play("TombDeath"); //�״� �ִϸ��̼� ��� �� Ŭ���� �ִϸ��̼� 
            

        }

        public override void Update()
        {

        }

        public override void Transition()
        {

        }

    }


    private class IntroState : SlimeState
    {
        public IntroState(SlimeBoss slime) : base(slime) { }

        public override void Enter()
        {
            animator.Play("SlimeIntro");
        }

        public override void Transition()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("SlimeIntro") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {

                ChangeState(State.Jump);
            }
        }
    }

    private class JumpState : SlimeState
    {
        public JumpState(SlimeBoss slime) : base(slime) { }

        public int rand;


        public override void Enter()
        {
            rand = Random.Range(3, 6);

        }
        public override void FixedUpdate()
        {

            if (onGround == true)
            {
                slime.StartCoroutine(slime.JumpRoutine());

            }
            else //�±׶��尡 �ƴ� �� (���� ) 
            {
                if (slimeRb.velocity.y > 1) //��� �ִ� 
                {
                    animator.Play("SlimeAirUp");
                }
                else if (slimeRb.velocity.y < 1 && slimeRb.velocity.y > -1) //���� �ִ� 
                {
                    animator.Play("SlimeUpDownTrans");
                }
                else if (slimeRb.velocity.y < -1) //�ϰ� �ִ� 
                {
                    animator.Play("SlimeAirDown");
                }
            }

        }
        public override void Exit()
        {
            slime.jumpCount = 0;
        }

        public override void Transition()
        {
            if (slime.jumpCount == rand && onGround == true)
            {
                ChangeState(State.Punch);
            }

            if (health <= 400 && (transform.position.x <= 6 && transform.position.x >= -6))
            {
                ChangeState(State.Dead);
            }
        }

    }

    private class PunchState : SlimeState
    {
        public PunchState(SlimeBoss slime) : base(slime) { }

        public override void Enter()
        {
            if (dir.x < 0)
            {
                renderer.flipX = false;
            }
            else if (dir.x >= 0)
            {
                renderer.flipX = true; //������ ���� 

            }

            animator.Play("SlimePunch");
        }

        public override void Update()
        {
            slimeRb.velocity = Vector2.zero; //��ġ ���� ������ �־����. 
            // Ÿ�� ��ġ�� ������� �ϴµ� ȸ�� ��Ű�� �� �ȵ� �� ������.. 

        }
        public override void Transition()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("SlimePunch") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                ChangeState(State.Jump);
            }
            if (health <= 400 && (transform.position.x <= 6 && transform.position.x >= -6))
            {
                ChangeState(State.Dead);
            }

        }

    }

    private class DeadState : SlimeState
    {
        public DeadState(SlimeBoss slime) : base(slime) { }

        public override void Enter()
        {
            animator.Play("SlimeToBigSlime");
        }

        public override void Update()
        {

        }
        public override void Transition()  //���⼭ �̰� ��ȭ�����ְ� animator�� ��ȯ���� �� �ֳ�? 
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("SlimeToBigSlime") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                ChangeState(State.BigIdle);

            }

        }

    }


    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
        health -= (float)damage;
    }


    public override void Die() //�̺�Ʈ ���� �ʿ� 
    {
        base.Die();

    }

    IEnumerator OnHit()
    {

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color32(red, green, blue, alpha);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = new Color(1, 1, 1, 1f);

    }
    IEnumerator TargetCoroutine()
    {
        if (!dead)
        {
            dir = (target.transform.position - transform.position).normalized;

            yield return new WaitForSecondsRealtime(0.2f);
        }

    }

    // �ϴ� ���� �������� ���� �����ϸ� �Ϳ� ���� �� ���� �����ϰ�
    // ���� ��� ���� flip�� �ٲ��༭ �ݴ�� ������ �� �ֵ��� ����. 

    IEnumerator JumpRoutine()  //�ϴ� �¿�� �Դٰ��� �ϴ°� ���� �غ���.. 
    {
        if (jumpRoutineEnd == false)
        {

            jumpRoutineEnd = true;
            jumpCount++;
            int randX = Random.Range(3, 5);
            int randY = Random.Range(5, 7);
            animator.Play("SlimeJumpReady");
            yield return new WaitForSeconds(1f);  //update�� addforece ��� �� 

            if (spriteRenderer.flipX == true) // �������� ���� ���� --> x�� �÷��� �̵� 
            {
                JumpForce(new Vector2(randX, randY));

            }
            else
            {
                JumpForce(new Vector2(-randX, randY));

            }

            yield return new WaitForSeconds(1f);

            jumpRoutineEnd = false;
        }

    }

    IEnumerator BigJumpRoutine()
    {
        if (bigJumpRoutineEnd == false)
        {

            bigJumpRoutineEnd = true;
            bigJumpCount++;
            int randX = Random.Range(3, 5);
            int randY = Random.Range(4, 6);
            animator.Play("BigSlimeJump"); //���� ��� ��� 
            yield return new WaitForSeconds(1f);  //update�� addforece ��� �� 

            if (spriteRenderer.flipX == true) // �������� ���� ���� --> x�� �÷��� �̵� 
            {
                JumpForce(new Vector2(randX, randY));

            }
            else
            {
                JumpForce(new Vector2(-randX, randY));

            }
            yield return new WaitForSeconds(1f);


            bigJumpRoutineEnd = false;
        }
    }

    IEnumerator TombMoveRoutine()
    {
        if (tombMoveRoutineEnd == false)
        {
            tombMoveRoutineEnd = true;
            tombMoveCount++;

            if (spriteRenderer.flipX == true) //������ ���� �ִ� ��� �������̵� 
            {
                animator.Play("TombRightMove");
                slimeRb.velocity = new Vector2(5, slimeRb.velocity.y);

                if (transform.position.x >= 7.5)
                {
                    slimeRb.velocity = Vector2.zero;
                }

            }
            else //���� ���� �ִ� ��� ���� �̵� 
            {
                animator.Play("TombLeftMove");
                slimeRb.velocity = new Vector2(-5, slimeRb.velocity.y);

                if (transform.position.x <= -7.8)
                {
                    slimeRb.velocity = Vector2.zero;
                }
            }
            yield return new WaitForSeconds(0.5f);
            tombMoveRoutineEnd = false;

        }
    }


    private void JumpForce(Vector2 maxHeightDisplacement)
    {
        v_y = Mathf.Sqrt(2 * slimeRb.gravityScale * -Physics2D.gravity.y * maxHeightDisplacement.y);
        v_x = maxHeightDisplacement.x * v_y / (2 * maxHeightDisplacement.y); //������ � ��Ģ ���� 

        Vector2 force = slimeRb.mass * (new Vector2(v_x, v_y) - slimeRb.velocity);
        slimeRb.AddForce(force, ForceMode2D.Impulse);
    }

    public void CreateMark()
    {
        GameObject instance1 = Instantiate(questionPrefab1, Mark1.position, Quaternion.identity);
        GameObject instance2 = Instantiate(questionPrefab2, Mark2.position, Quaternion.identity);
        GameObject instance3 = Instantiate(questionPrefab3, Mark3.position, Quaternion.identity);
        instance1.transform.SetParent(Mark1);
        instance2.transform.SetParent(Mark2);
        instance3.transform.SetParent(Mark3);
    }


    public void PunchCollider() //�ָ� �ݶ��̴� ���ְ� 
    {
        PunchArm.SetActive(true);
        punchFist.SetActive(true);
    }

    IEnumerator TargetTombCoroutine()
    {
        if (isOverLap == false)
        {
            isOverLap = true;
            Collider2D collider = Physics2D.OverlapCircle(transform.position
                + new Vector3(0, 2, 0), targetRange, TombTarget);

            if (collider != null)
            {
                if (collider.gameObject.tag == "Player")
                {
                    isAttack = true;
                }
            }
            else
            {
                isAttack = false;
            }
            yield return new WaitForSecondsRealtime(0.1f);
            isOverLap = false;

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 2, 0), targetRange);
    }

    // slime.gameObject.layer = 0; //�ϴ� ����Ʈ�� �غ���? 

    public void ChangeMonsterLayer() //�̺�Ʈ ������ֱ�. 
    {
        Debug.Log("���ͷ��̾�");
        gameObject.layer = 13; //���� ���̾�. 
        TombBoxCollider.enabled = true; //�ڽ� �ݶ��̴� ���ֱ�.       
        TombCircleCollider.enabled = false; // ���ݶ��̴� ���ֱ�.
    }

    public void ChangeZeroLayer()
    {
        Debug.Log("�������̾�");
        gameObject.layer = 0;
        TombBoxCollider.enabled = false;
        TombCircleCollider.enabled = true;
    }

    IEnumerator TombDelay()
    {
        yield return new WaitForSeconds(0.1f);
    }

    public void DelayTomb()
    {
        Debug.Log("�ڷ�ƾ�Լ�����");
        StartCoroutine(TombDelay());
    }

}
