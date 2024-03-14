using System;
using System.Collections.Generic;
using UnityEngine;

 //�Ϲ�ȭ ���� --> ��Ʈ������ ���� 

public class StateMachine :MonoBehaviour
{
    private Dictionary<string, BaseState> stateDic = new Dictionary<string, BaseState>();
    private BaseState curState;

    

    //AddState �Լ��� ���� ��ųʸ��� ���¸� �������ְ�
    // curState�� ���� �� ��ųʸ��� ���µ��� ������ ����. 

    public void Start()
    {
        curState.Enter();
    }

    public void Update() //�� ������Ʈ�� start�� ���� ������ Ȯ���Ϸ���? 
    {
        curState.Update();
        curState.Transition();
        
    }

    public void LateUpdate()
    {
        curState.LateUpdate();
    }

    public void FixedUpdate()
    {
        curState.FixedUpdate();
    }
    
    public void InitState(string stateName)
    {
        curState = stateDic[stateName];
    }

    public void AddState(string stateEnum, BaseState state)
    {
        state.SetStateMachine(this);
        stateDic.Add(stateEnum, state);
    }

    public void ChangeState(string stateEnum)
    {
        curState.Exit();
        curState = stateDic[stateEnum];
        curState.Enter();
    }

    public void InitState<T>(T stateType) where T : Enum
    {
        InitState(stateType.ToString());
    }

    public void AddState<T>(T stateType, BaseState state) where T : Enum
    {
        AddState(stateType.ToString(), state);
    }

    public void ChangeState<T>(T stateType) where T : Enum
    {
        ChangeState(stateType.ToString());
    }

}

public class BaseState
{
    private StateMachine stateMachine;

    public void SetStateMachine(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void ChangeState(string stateName)
    {
        stateMachine.ChangeState(stateName);
    }

    protected void ChangeState<T>(T stateType) where T : Enum
    {
        ChangeState(stateType.ToString());
    }


    public virtual void Enter() { }  //���¿� ó�� �������� �� �� ���� ȣ��Ǵ� �޼���?
    public virtual void Exit() { }  // ���°� ����Ǵ� ȣ��Ǵ� �޼��� ?? 
    public virtual void Update() { }  // �� �����Ӹ��� ȣ��Ǿ�� �ϴ� �޼��� ??
    public virtual void LateUpdate() { }
    public virtual void FixedUpdate() { }

    public virtual void Transition() { }
}