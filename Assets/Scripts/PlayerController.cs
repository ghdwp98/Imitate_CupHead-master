using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //������ ���� Ŭ�������� �� ���� �����ϰ� � ��Ȳ�� �Ǹ� �ٸ� ���·� Ʈ������ ���� ���� 
    //�̵��� �׳� �����ý������� ó������ �װ� �ξ� ����� ����. 

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
    [SerializeField] LayerMask groundCheckLayer; //���������� ���� ���� or �и� �������� �������� 
    [SerializeField] Vector2 playerPos;

    private Vector2 inputDir;
    private bool isGround;
    private int gorundCount;

    private StateMachine stateMachine;

    //�̵� �ɱ� ��� ���� ���� ���� -->���¸ӽ� ���� 



    void Awake() //���� �ÿ� ���¸� �߰� �ϰ� ���� (��ųʸ��� add�� )
    {
        stateMachine = gameObject.GetComponent<StateMachine>();
        stateMachine.AddState(State.Idle, new IdleState(this));
        stateMachine.AddState(State.Run, new RunState(this));
        stateMachine.AddState(State.Attack, new AttackState(this));
        stateMachine.AddState(State.Jump, new JumpState(this));
        stateMachine.InitState(State.Idle); //���� ���¸� Idle ���·� ���� 


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
        stateMachine.Update(); //������Ʈ���� ������Ʈ�ӽ��� ������Ʈ ��������� 
        //curState�� update�� transition�� ����ϰ� �ִ� statemachine�� update �Լ� 
    }

    private void FixedUpdate()
    {
        //�÷��̾ ������ٵ� ���� �Ƚ��������Ʈ�� �ʿ��� ��찡 ����
        stateMachine.FixedUpdate();
    }

    private class PlayerState : BaseState //���̽�������Ʈ ����ؼ� ���밡 �� Ŭ���� 
    {
        protected PlayerController player; //Player�� �̸� ����ϴ� stateŬ�����鿡��
        // player.hp ������ �÷��̾��� ������ �̿��� �� �ֵ��� �Ѵ�. 

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

    private class IdleState : PlayerState  //�̷��� �ʼ��Ű����� player�� ���ٰ� ���´�. ������ �����ڸ� ����� 
    {
        //�ڽ��� �θ� Ŭ������ �����ڸ� ������ ȣ�� Base �̿� 
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
