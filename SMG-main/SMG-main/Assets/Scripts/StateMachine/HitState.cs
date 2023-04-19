using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : State<Monster>
{
    public Animator animator;
    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        Debug.Log("Hit 상태로 진입");
        animator?.SetTrigger("doHit");
    }

    public override void Update(float deltaTime)
    {   
        if(context.CurHealth > 0) // 여기에 애니메이션 이벤트 추가해서 작업하는 것이 어떤지, Hit 모션이 다 끝나면 Idle로 가도록
        {
            stateMachine.ChangeState<IdleState>();
        }
        else
        {
            stateMachine.ChangeState<DeadState>();
        }
    }

    public override void OnExit()
    {
  
        context.isHit = false;
    }
    
}
