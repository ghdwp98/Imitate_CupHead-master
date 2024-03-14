using System;
using System.Collections.Generic;
using UnityEngine;

 //일반화 포기 --> 스트링으로 쓰자 

public class StateMachine :MonoBehaviour
{
    private Dictionary<string, BaseState> stateDic = new Dictionary<string, BaseState>();
    private BaseState curState;

    

    //AddState 함수를 통해 딕셔너리에 상태를 삽입해주고
    // curState를 통해 그 딕셔너리의 상태들을 가져다 쓴다. 

    public void Start()
    {
        curState.Enter();
    }

    public void Update() //이 업데이트나 start등 에서 죽음을 확인하려면? 
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


    public virtual void Enter() { }  //상태에 처음 진입했을 때 한 번만 호출되는 메서드?
    public virtual void Exit() { }  // 상태가 변경되는 호출되는 메서드 ?? 
    public virtual void Update() { }  // 매 프레임마다 호출되어야 하는 메서드 ??
    public virtual void LateUpdate() { }
    public virtual void FixedUpdate() { }

    public virtual void Transition() { }
}