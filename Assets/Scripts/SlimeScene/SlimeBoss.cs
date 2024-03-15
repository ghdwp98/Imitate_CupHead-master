using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class SlimeBoss : LivingEntity
{
    public enum State
    {
        Intro,Phase1,Phase2,Phase3,Dead
    }

    





    [Header("Slime")]
    [SerializeField] float hp = 300;
    [SerializeField] LayerMask TargetLayer;
    Rigidbody2D slimeRb;
    SpriteRenderer spriteRenderer;

    AudioSource SlimeAudio;
    Animator animator;
    [SerializeField] byte red = 233 ;
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


        stateMachine=gameObject.AddComponent<StateMachine>();
        stateMachine.AddState(State.Intro, new IntroState(this));
        stateMachine.AddState(State.Phase1, new Phase1State(this));
        stateMachine.AddState(State.Phase2, new Phase2State(this));
        stateMachine.AddState(State.Phase3, new Phase3State(this));
        stateMachine.AddState(State.Dead, new DeadState(this));


        stateMachine.InitState(State.Intro);
    }


    void Start()
    {

    }

    void Update()
    {

    }

    private class SlimeState : BaseState
    {
        protected SlimeBoss slime;


        protected Transform transform => slime.transform;
        protected float hp { get { return slime.hp; }set { slime.hp = value; } }
        protected Rigidbody2D slimeRb => slime.slimeRb;
        protected SpriteRenderer renderer => slime.spriteRenderer;
        protected Animator animator => slime.animator;


        public SlimeState(SlimeBoss slime)
        {
            this.slime = slime;
        }
    }

    private class IntroState : SlimeState
    {
        public IntroState(SlimeBoss slime) : base(slime) { }



    }

    private class Phase1State : SlimeState
    {
        public Phase1State(SlimeBoss slime) : base(slime) { }


    }

    private class Phase2State : SlimeState
    {
        public Phase2State(SlimeBoss slime) : base(slime) { }




    }

    private class Phase3State :SlimeState
    {
        public Phase3State(SlimeBoss slime) : base(slime) { }

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

        }

    }


    IEnumerator OnHit()
    {

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color32(red,green,blue,alpha);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = new Color(1, 1, 1, 1f);
        

    }
    

}
