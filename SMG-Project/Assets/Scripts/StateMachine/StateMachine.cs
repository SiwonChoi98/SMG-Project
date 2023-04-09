using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class State<T>
{
    protected int mecanimStateHash;
    protected StateMachine<T> stateMachine;
    protected T context;

    public State()
    {
    }
    public State(string mecanimStateName) : this(Animator.StringToHash(mecanimStateName))
    {
    }
    public State(int mecanimStateHash)
    {
        this.mecanimStateHash = mecanimStateHash;
    }

    internal void SetMachineAndContext(StateMachine<T> stateMachine, T context)
    {
        this.stateMachine = stateMachine;
        this.context = context;

        OnInitialized();
    }
    public virtual void OnInitialized() //셋팅
    { }

    public virtual void OnEnter() //한번 입력
    { }

    public abstract void Update(float deltaTime); //계속업데이트

    public virtual void OnExit() //빠져나온다.
    { }
}
public class StateMachine<T>
{
    private T context;
    public event Action OnChangedState;

    private State<T> currentState; //현재상태
    public State<T> CurrentState => currentState;

    private State<T> previousState; //전상태
    public State<T> PreviousState => previousState;
    private Dictionary<System.Type, State<T>> states = new Dictionary<Type, State<T>>();
    public StateMachine(T context, State<T> initialState)
    {
        this.context = context;

        // Setup our initial state
        AddState(initialState);
        currentState = initialState;
        currentState.OnEnter();
    }
    public void AddState(State<T> state)
    {
        state.SetMachineAndContext(this, context);
        states[state.GetType()] = state;
    }
    public void Update(float deltaTime)
    {
        //elapsedTimeInState += deltaTime; //경과시간 체크
        currentState.Update(deltaTime); //현재 상태 계속 업데이트
    }
    public R ChangeState<R>() where R : State<T>
    {
        // avoid changing to the same state
        var newType = typeof(R);
        if (currentState.GetType() == newType)
        {
            return currentState as R;
        }

        // only call end if we have a currentState
        if (currentState != null)
        {
            currentState.OnExit();
        }

        Debug.Log("ChageState : " + typeof(R).ToString());
        // swap states and call OnEnter
        previousState = currentState;
        currentState = states[newType];

        currentState.OnEnter();
        //elapsedTimeInState = 0.0f;



        // Fire the changed event if we hav a listener
        if (OnChangedState != null)
        {
            OnChangedState();
        }
        return currentState as R;
    }
}
