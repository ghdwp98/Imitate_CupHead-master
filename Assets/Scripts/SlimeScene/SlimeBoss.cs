using System.Collections;
using UnityEngine;

public class SlimeBoss : LivingEntity
{
    public enum State //�̰� �ֱ� �������̴ϱ� phase�� �ƴ϶� �� ���� ��ǵ�� ��������. 
    {
        Intro, Punch,Jump, Dead
    }







    [Header("Slime")]
    [SerializeField] float hp = 300;
    [SerializeField] LayerMask TargetLayer;
    Rigidbody2D slimeRb;
    SpriteRenderer spriteRenderer;
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

    //�� �� ���� ������ �ݴ�� �پ���� Wall�� check; 

    void Start()
    {

    }

    void Update()
    {
        stateMachine.Update();
    }


    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundCheckLayer);
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
                
            }
        }
    }

    private class JumpState : SlimeState
    {
        public JumpState(SlimeBoss slime) : base(slime) { }

        public override void Enter()
        {
            Debug.Log("���� ������Ʈ");
        }

        public override void FixedUpdate()
        {
            
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


    IEnumerator OnHit()
    {

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color32(red, green, blue, alpha);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = new Color(1, 1, 1, 1f);


    }


}
