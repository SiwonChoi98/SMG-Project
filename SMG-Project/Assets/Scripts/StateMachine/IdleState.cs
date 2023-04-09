using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class IdleState : State<Monster>
{
    public Animator animator;

    public override void OnInitialized() //셋팅
    {
        animator = context.GetComponent<Animator>();
    }

    public override void OnEnter() //한번실행
    {
        context.transform.LookAt(context.target.position);
        //context.GetComponent<NavMeshAgent>().speed = 0f; //임시
        animator?.SetBool("isIdle", true);
        Debug.Log("idle 상태로 진입");
        context.GetComponent<NavMeshAgent>().speed = 3.5f; //임시
    }

    public override void Update(float deltaTime) //게속업데이트
    {
        if (context.target)
        {
            if (context.isHit)
            {
                stateMachine.ChangeState<HitState>();
                return;
            }

            if (context.isAttackRange)
            {
                stateMachine.ChangeState<AttackState>();
                return;
            }
            stateMachine.ChangeState<MoveState>();
        }
    }
    public override void OnExit() //나가기
    {
        animator?.SetBool("isIdle", false);
    }
}
