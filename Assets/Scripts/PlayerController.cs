using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //각각의 상태 클래스에서 뭘 할지 결정하고 어떤 상황이 되면 다른 상태로 트랜지션 할지 결정 
    //이동은 그냥 고전시스템으로 처리하자 그게 훨씬 쉬울것 같다. 

    public enum State { Idle, Run, Attack, Jump ,attackRun,JumpAttack}

    [Header("Player")]
    [SerializeField] int hp;
    [SerializeField] int mp;




    [Header("Component")]
    [SerializeField] new Rigidbody2D rigidbody;
    [SerializeField] new SpriteRenderer renderer;
    [SerializeField] Animator animator;

    [Header("Spec")]
    [SerializeField] float maxSpeed;
    [SerializeField] float accelPower;
    [SerializeField] float decelPower;
    [SerializeField] float jumpSpeed;
    [SerializeField] LayerMask groundCheckLayer; //땅위에서만 점프 가능 or 패리 위에서만 점프가능 
    [SerializeField] Vector2 playerPos;

    private Vector2 inputDir;
    private bool isGround;
    private int gorundCount;

    private StateMachine stateMachine;

    //이동 앉기 대시 점프 조준 슈팅 -->상태머신 구현 



    void Awake() //시작 시에 상태를 추가 하고 시작 (딕셔너리에 add함 )
    {
        stateMachine = gameObject.GetComponent<StateMachine>();
        stateMachine.AddState(State.Idle, new IdleState(this));
        stateMachine.AddState(State.Run, new RunState(this));
        stateMachine.AddState(State.Attack, new AttackState(this));
        stateMachine.AddState(State.Jump, new JumpState(this));
        stateMachine.InitState(State.Idle); //최초 상태를 Idle 상태로 시작 


    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

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
    }

    private class PlayerState : BaseState //베이스스테이트 상속해서 뼈대가 될 클래스 
    {
        protected PlayerController player; //Player로 이를 상속하는 state클래스들에서
        // player.hp 등으로 플레이어의 변수를 이용할 수 있도록 한다. 

       /* protected Transform transform => player.transform;
        float maxSpeed => player.maxSpeed;
        float accelPower => player.accelPower;
        float decelPower => player.decelPower;
        float jumpSpeed => player.jumpSpeed;
        LayerMask groundCheckLayer => player.groundCheckLayer;
        Vector2 playerPos => player.playerPos;*/
        public PlayerState(PlayerController player)
        {
            this.player=player;
        }


    }

    private class IdleState : PlayerState  //이러면 필수매개변수 player가 없다고 나온다. 위에서 생성자를 만들면 
    {
        //자식이 부모 클래스의 생성자를 강제로 호출 Base 이용 
        public IdleState(PlayerController player) : base(player) { }

    }

    private class JumpState : PlayerState
    {
        public JumpState(PlayerController player) : base(player) { }
    }

    private class AttackState : PlayerState
    {
        public AttackState(PlayerController player) : base(player) { }

    }

    private class RunState : PlayerState
    {
        public RunState(PlayerController player) : base(player) { }

    }

   
    
    

}
