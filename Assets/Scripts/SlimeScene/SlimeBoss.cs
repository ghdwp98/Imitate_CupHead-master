using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeBoss : LivingEntity
{
    public enum State //�̰� �ֱ� �������̴ϱ� phase�� �ƴ϶� �� ���� ��ǵ�� ��������. 
    {
        Intro, Punch, Jump, Dead
    }

    //health�� ü���ε�? 
    [Header("Slime")]
    [SerializeField] float hp = 300;
    [SerializeField] LayerMask TargetLayer;
    [SerializeField] GameObject target;
    [SerializeField] Vector2 dir;
    [SerializeField] float v_y;
    [SerializeField] float v_x;
    [SerializeField] Vector2 lastVelocity;
    [SerializeField] int jumpCount = 0;
    

    Rigidbody2D slimeRb;
    SpriteRenderer spriteRenderer;
    bool jumpRoutineEnd = false;
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


        stateMachine.InitState(State.Jump); // �ڵ������� ��� �������·� ����. 
    }
    void Start()
    {

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

            slimeRb.velocity = dir * 8;
            
            if (spriteRenderer.flipX == true)  //�ø� ���¿� ���� ���� �Լ��� ������ �ٽ� �������༭ - + �� �޸����� ����. 
            {
                spriteRenderer.flipX = false;

                if(slimeRb.velocity.y<-0.5) //�ϰ� �� �̸� 
                {
                    
                    animator.Play("SlimeAirDownTurn");
                }
                else if(slimeRb.velocity.y>0.5) //��� ���̸� 
                {
                   
                    animator.Play("SlimeAirUpTurn");
                }
            }
            else
            {
                spriteRenderer.flipX = true;
                if (slimeRb.velocity.y < -0.5) //�ϰ� �� �̸� 
                {
                   
                    animator.Play("SlimeAirDownTurn");
                }
                else if (slimeRb.velocity.y > 0.5) //��� ���̸� 
                {
                    

                    animator.Play("SlimeAirUpTurn");

                }

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

        Debug.Log(health);
    }
    

    private class SlimeState : BaseState
    {
        protected SlimeBoss slime;


        protected Transform transform => slime.transform;
        protected float hp { get { return slime.hp; } set { slime.hp = value; } }
        protected Rigidbody2D slimeRb => slime.slimeRb;
        protected SpriteRenderer renderer => slime.spriteRenderer;
        protected Animator animator => slime.animator;

        protected bool onGround { get { return slime.onGround; } set { slime.onGround = value; } }

        protected LayerMask groundCheckLayer => slime.groundCheckLayer;

        protected GameObject target => slime.target;
        protected Vector2 dir { get { return slime.dir; } set { slime.dir = value; } }

        public SlimeState(SlimeBoss slime)
        {
            this.slime = slime;
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
            rand = Random.Range(3,6);
        }
        public override void FixedUpdate()
        {
            
            if (onGround==true) 
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
            if(slime.jumpCount==rand&&onGround==true)
            {
                ChangeState(State.Punch);
            }
        }

    }

    private class PunchState : SlimeState
    {
        public PunchState(SlimeBoss slime) : base(slime) { }


        public override void Enter()
        {
            if(dir.x<0)
            {
                renderer.flipX = false;
            }
            else if(dir.x>=0)
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

        }

    }

    private class DeadState : SlimeState
    {
        public DeadState(SlimeBoss slime) : base(slime) { }




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

    private void JumpForce(Vector2 maxHeightDisplacement)
    {
        v_y = Mathf.Sqrt(2 * slimeRb.gravityScale * -Physics2D.gravity.y * maxHeightDisplacement.y);
        v_x = maxHeightDisplacement.x * v_y / (2 * maxHeightDisplacement.y); //������ � ��Ģ ���� 

        Vector2 force = slimeRb.mass * (new Vector2(v_x, v_y) - slimeRb.velocity);
        slimeRb.AddForce(force, ForceMode2D.Impulse);
    }


}
