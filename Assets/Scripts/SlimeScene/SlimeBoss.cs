using System.Collections;
using UnityEngine;

public class SlimeBoss : LivingEntity
{
    public enum State //이거 애기 슬라임이니까 phase가 아니라 각 공격 모션들로 나눠주자. 
    {
        Intro, Punch, Jump, Dead
    }







    [Header("Slime")]
    [SerializeField] float hp = 300;
    [SerializeField] LayerMask TargetLayer;
    [SerializeField] GameObject target;
    [SerializeField] Vector2 dir;

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


        stateMachine.InitState(State.Intro);
    }
    void Start()
    {

    }

    void Update()
    {
        stateMachine.Update();
        StartCoroutine(TargetCoroutine());


    }


    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundCheckLayer);
    }
    //양 옆 벽에 닿으면 반대로 뛰어야함 Wall로 check; 

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

        public override void Enter()
        {
            Debug.Log("점프 스테이트");
            
        }

        public override void FixedUpdate()
        {
            slime.StartCoroutine(slime.JumpRoutine());
        }

        public override void Exit()
        {

        }


        public override void Transition()
        {

        }

    }

    private class PunchState : SlimeState
    {
        public PunchState(SlimeBoss slime) : base(slime) { }


        public override void Enter()
        {

            
        }

        public override void Update()
        {
            


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


    public override void Die() //이벤트 구현 필요 
    {
        base.Die();

    }

    private void OnTriggerEnter2D(Collider2D collision) //타겟은 트리거를 가지자. 
    {
        if (!dead)
        {
            //피격효과음
            if (collision.gameObject.tag == "Bullet")
            {
                StartCoroutine(OnHit()); //피격 색 변경 효과
            }
            else if(collision.gameObject.tag =="Wall") //벽에 부딪히면
            {
                
            }


        }

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

    // 일단 한쪽 방향으로 점프 시작하면 벾에 닿을 때 까지 진행하고
    // 벽에 닿는 순간 flip을 바꿔줘서 반대로 진행할 수 있도록 하자. 

    IEnumerator JumpRoutine()
    {
        if(jumpRoutineEnd==false)
        {
            jumpRoutineEnd = true;
            
            var rand = Random.Range(0, 3); // --> 벽 만나면 반대로 움직여줘야하는데
            Debug.Log(rand);
            if (rand == 0)  //각 랜덤에 따라서 뛰는 높이가 다름. 
            {
                slimeRb.AddForce(new Vector2(100,100));
                yield return new WaitForSeconds(1f);

            }
            else if(rand==1)
            {
                slimeRb.AddForce(new Vector2(200,200));
                yield return new WaitForSeconds(1f);


            }
            else if(rand==2)
            {
                slimeRb.AddForce(new Vector2(300,300));
                yield return new WaitForSeconds(1f);


            }

            yield return new WaitForSeconds(1f);
            jumpRoutineEnd=false;
        }
       
    }


}
